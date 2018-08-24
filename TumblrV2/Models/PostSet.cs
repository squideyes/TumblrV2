using System.Collections;
using System.Collections.Generic;

namespace TumblrV2
{
    public class PostSet : IEnumerable<Post>
    {
        private List<Post> posts = new List<Post>();

        internal PostSet(string blog, Media media, int totalPosts)
        {
            Blog = blog;
            Media = media;
            TotalPosts = totalPosts;
        }

        public string Blog { get; }
        public Media Media { get; }
        public int TotalPosts { get;  }

        public int Count => posts.Count;

        public void Add(Post post) => posts.Add(post);

        public override string ToString() =>
            $"{posts.Count} {Media.ToString().ToLower()} posts from \"{Blog}\"";

        public IEnumerator<Post> GetEnumerator() => posts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
