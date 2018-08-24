using System;
using System.Collections.Generic;

namespace TumblrV2
{
    public class Post
    {
        public string Blog { get; set; }
        public long PostId { get; set; }
        public Media Media { get; set; }
        public PostStatus Status { get; set; }
        public DateTime PostedOn { get; set; }
        public Uri VideoUri { get; set; }
        public List<Uri> PhotoUris { get; set; }

        public override string ToString() =>
            $"{PostId} ({Media}, {Status}, PostedOn: {PostedOn:MM/dd/yyyy HH:mm:ss.fff})";
    }
}
