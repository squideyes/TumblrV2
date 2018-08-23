using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TumblrV2
{
    public class Fetcher
    {
        private class Root
        {
            [JsonProperty(PropertyName = "meta")]
            public Meta Meta { get; set; }

            [JsonProperty(PropertyName = "response")]
            public Response Data { get; set; }
        }

        private class Meta
        {
            [JsonProperty(PropertyName = "status")]
            public int Status { get; set; }

            [JsonProperty(PropertyName = "msg")]
            public string Message { get; set; }
        }

        private class Response
        {
            [JsonProperty(PropertyName = "blog")]
            public Blog Blog { get; set; }

            [JsonProperty(PropertyName = "posts")]
            public PostData[] Posts { get; set; }

            [JsonProperty(PropertyName = "total_posts")]
            public int TotalPosts { get; set; }
        }

        private class Blog
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        private class PostData
        {
            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }

            [JsonProperty(PropertyName = "id")]
            public long Id { get; set; }

            [JsonProperty(PropertyName = "date")]
            public DateTime PostedOn { get; set; }

            [JsonProperty(PropertyName = "photos")]
            public Photo[] Photos { get; set; }

            [JsonProperty(PropertyName = "video_url")]
            public Uri VideoUri { get; set; }

            [JsonProperty(PropertyName = "video_type")]
            public string VideoType { get; set; }
        }

        private class Photo
        {
            [JsonProperty(PropertyName = "original_size")]
            public OriginalSize OriginalSize { get; set; }
        }

        private class OriginalSize
        {
            [JsonProperty(PropertyName = "url")]
            public Uri Uri { get; set; }
        }

        private readonly string apiKey;

        public Fetcher(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<PostSet> GetPostsAsync(string blogName, PostKind postKind)
        {
            const string BASEURI = "https://api.tumblr.com/v2/blog/";

            var uri = new Uri($"{BASEURI}{blogName}/posts?api_key={apiKey}");

            var client = new HttpClient();

            var json = await client.GetStringAsync(uri);

            var root = JsonConvert.DeserializeObject<Root>(json);

            if (root.Meta.Status != 200)
            {
                throw new Exception("??????????????????????");
            }

            var posts = new PostSet(root.Data.Blog.Name);

            foreach (var p in root.Data.Posts)
            {
                if (!Enum.TryParse(p.Type, true, out PostKind pk))
                    continue;

                if (pk != postKind)
                    continue;

                posts.Add(new Post()
                {
                    PostId = p.Id,
                    PostedOn = p.PostedOn,
                    Kind = postKind,
                    Status = PostStatus.Queued,
                    VideoUri = p.VideoUri,
                    VideoType = p.VideoType,
                    PhotoUris = p.Photos?.Select(x => x.OriginalSize.Uri).ToList()
                });
            }

            return posts;
        }
    }
}
