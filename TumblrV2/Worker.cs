using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TumblrV2.Helpers;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;

namespace TumblrV2
{
    public class Worker
    {
        private static AppInfo appInfo = new AppInfo(typeof(Program).Assembly);

        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        public int Run(string[] args)
        {
            var app = new CommandLineApplication
            {
                AllowArgumentSeparator = false,
                Name = "TumblrV2.exe",
                FullName = $"{appInfo.Title}: A simple Tumblr backup utility for media but not texts, etc."
            };

            app.HelpOption("-?|--help");

            var blogNameArg = app.Argument("blogname",
                "The name of a Tumblr blog (i.e. \"funnyhalloffame[.tumblr.com]\")",
                true);

            var mediaOption = app.Option("--media",
                "Media type(s) to download (defaults to \"PHOTO,AUDIO,VIDEO\")",
                CommandOptionType.MultipleValue);

            var planOption = app.Option("--plan",
                "File-handling plan (\"PURGE\", \"ADD\" or the default \"REPLACE\")",
                CommandOptionType.SingleValue);

            var tagOption = app.Option("--tag",
                "A single tag to filter posts by (i.e. \"DOGS\" or \"NEW YORK\")",
                CommandOptionType.MultipleValue);

            var folderOption = app.Option("--folder",
                "The [base] folder to save files to (defaults to \"Downloads\")",
                CommandOptionType.SingleValue);

            app.OnExecute(async () =>
            {
                var blog = blogNameArg.Value;

                if (string.IsNullOrWhiteSpace(blog))
                    return ShowHelpAndExit(app, ExitCode.BadBlogName);

                if (!blog.Contains('.'))
                    blog += ".tumblr.com";

                if (!Uri.CheckHostName(blog).Equals(UriHostNameType.Dns))
                    return ShowHelpAndExit(app, ExitCode.BadBlogName);

                //???????????????????????????????????????????????????????????
                if (!mediaOption.Values.ToEnumList(out List<Media> medias))
                    medias = new List<Media> { Media.Photo, Media.Audio, Media.Video };

                if (!Enum.TryParse(planOption.Value(), true, out FilePlan plan))
                {
                    if (planOption.HasValue())
                        return ShowHelpAndExit(app, ExitCode.BadFilePlan);
                    else
                        plan = FilePlan.Add;
                }

                var tag = string.Join(" ", tagOption.Values);

                var folder = folderOption.Value();

                if (string.IsNullOrWhiteSpace(folder))
                    folder = @"Downloads\";
                else if (!folder.IsFolderName())
                    return ShowHelpAndExit(app, ExitCode.BadFolder);

                folder.EnsurePathExists();

                var fetcher = new Fetcher(WellKnown.ApiKey);

                // neeeds db integration
                var mediasWithLastPostIds = medias.ToDictionary(m => m, m => 0L);

                var posts = await GetUnfetchedPostsAsync(
                    fetcher, blog, tag, plan, mediasWithLastPostIds);

                await FetchMediasAsync(posts);

                return ExitCode.Success;
            });

            try
            {
                if (args.Length == 0)
                {
                    app.ShowHelp();

                    return ExitCode.NoArgs;
                }
                else
                {
                    return app.Execute(args);
                }
            }
            catch (CommandParsingException error)
            {
                Console.WriteLine();
                Console.WriteLine(error.Message);

                return ExitCode.ParseError;
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);

                return ExitCode.InternalError;
            }
        }

        private static int ShowHelpAndExit(
            CommandLineApplication app, int exitCode)
        {
            Console.Clear();

            app.ShowHelp();

            return exitCode;
        }

        private FetchJob GetJob(Post post, Uri uri, int? photoNumber)
        {
            return new FetchJob()
            {
                Blog = post.Blog,
                Media = post.Media,
                PostId = post.PostId,
                PostedOn = post.PostedOn,
                Uri = uri,
                PhotoNumber = photoNumber
            };
        }

        private async Task FetchMediasAsync(List<Post> posts)
        {
            var client = new HttpClient();

            var jobber = new TransformManyBlock<Post, FetchJob>(
                post =>
                {
                    var jobs = new List<FetchJob>();

                    if (post.Media != Media.Photo)
                    {
                        jobs.Add(GetJob(post, post.VideoUri, null));
                    }
                    else
                    {
                        for (int i = 0; i < post.PhotoUris.Count; i++)
                            jobs.Add(GetJob(post, post.PhotoUris[i], i));
                    }

                    return jobs;
                });

            var fetcher = new ActionBlock<FetchJob>(
                async job =>
                {
                    var response = await client.GetAsync(job.Uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        await job.SaveToFileAsync(stream, "Downloads");

                        logger.LogInformation($"Fetched and saved {job.FileName}");
                    }
                    else
                    {
                        var sb = new StringBuilder();

                        sb.Append(response.ReasonPhrase);
                        sb.Append(" (StatusCode: ");
                        sb.Append(response.StatusCode);
                        sb.Append(", FileName: ");
                        sb.Append(job.FileName);
                        sb.Append(", Uri: ");
                        sb.Append(job.Uri);
                        sb.Append(")");

                        logger.LogWarning(sb.ToString());
                    }
                },
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

            jobber.LinkTo(fetcher, new DataflowLinkOptions { PropagateCompletion = true });

            posts.ForEach(post => jobber.Post(post));

            jobber.Complete();

            await fetcher.Completion;
        }

        private async Task<List<Post>> GetUnfetchedPostsAsync(Fetcher fetcher,
            string blog, string tag, FilePlan plan, Dictionary<Media, long> medias)
        {
            var posts = new ConcurrentBag<Post>();

            var block = new ActionBlock<Media>(
                async media =>
                {
                    int offset = 0;

                    while (true)
                    {
                        var postSet = await fetcher.GetPostsAsync(
                            blog, media, tag, offset);

                        logger.LogTrace($"Got {postSet} (Offset: {offset}, TotalPosts: {postSet.TotalPosts:N0})");

                        if (postSet.Count == 0)
                            return;

                        foreach (var post in postSet)
                            posts.Add(post);

                        if (postSet.Last().PostId <= medias[media])
                            return;

                        offset += 20;
                    }
                },
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

            medias.Keys.ToList().ForEach(m => block.Post(m));

            block.Complete();

            await block.Completion;

            return posts.OrderByDescending(p => p.PostId).ToList();
        }
    }
}
