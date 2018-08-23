using System;
using System.Collections;
using System.Collections.Generic;

namespace TumblrV2
{
    public class PostSet : IEnumerable<Post>
    {
        private List<Post> posts = new List<Post>();

        internal PostSet(string blogName)
        {
            BlogName = blogName;
        }

        public string BlogName { get; }

        public int Count => posts.Count;

        public void Add(Post post) => posts.Add(post);

        public IEnumerator<Post> GetEnumerator() => posts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
