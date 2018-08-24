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
            public Data Data { get; set; }
        }

        private class Meta
        {
            [JsonProperty(PropertyName = "status")]
            public int Status { get; set; }

            [JsonProperty(PropertyName = "msg")]
            public string Message { get; set; }
        }

        private class Data
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

        public async Task<PostSet> GetPostsAsync(
            string blog, Media media, string tag, int offset)
        {
            const string BASEURI = "https://api.tumblr.com/v2/blog/";

            var m = media.ToString().ToLower();

            var url = $"{BASEURI}{blog}/posts/{m}?api_key={apiKey}";

            if (!string.IsNullOrWhiteSpace(tag))
                url += "&tag=" + tag;

            url += "&offset=" + offset;

            var uri = new Uri(url);

            var client = new HttpClient();

            var json = await client.GetStringAsync(uri);

            var root = JsonConvert.DeserializeObject<Root>(json);

            if (root.Meta.Status != 200)
            {
                throw new Exception("??????????????????????");
            }

            var posts = new PostSet(blog, media, root.Data.TotalPosts);

            foreach (var p in root.Data.Posts)
            {
                if (!Enum.TryParse(p.Type, true, out Media pk))
                    continue;

                if (pk != media)
                    continue;

                if (pk == Media.Video && p.VideoType != "tumblr")
                    continue;

                posts.Add(new Post()
                {
                    Blog = blog,
                    PostId = p.Id,
                    PostedOn = p.PostedOn,
                    Media = media,
                    Status = PostStatus.Queued,
                    VideoUri = p.VideoUri,
                    PhotoUris = p.Photos?.Select(x => x.OriginalSize.Uri).ToList()
                });
            }

            return posts;
        }
    }
}
