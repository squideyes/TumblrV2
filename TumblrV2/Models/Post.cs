using System;
using System.Collections.Generic;

namespace TumblrV2
{
    public class Post
    {
        public long PostId { get; set; }
        public PostKind Kind { get; set; }
        public PostStatus Status { get; set; }
        public DateTime PostedOn { get; set; }
        public Uri VideoUri { get; set; }
        public string VideoType { get; set; }
        public List<Uri> PhotoUris { get; set; }
    }
}
