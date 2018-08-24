using System;
using TumblrV2.Helpers;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TumblrV2
{
    public class FetchJob
    {
        public string Blog { get; set; }
        public long PostId { get; set; }
        public int? PhotoNumber { get; set; }
        public Uri Uri { get; set; }
        public Media Media { get; set; }
        public DateTime PostedOn { get; set; }

        public string FileName
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(PostId);

                if (PhotoNumber.HasValue)
                {
                    sb.Append('_');
                    sb.Append(PhotoNumber.Value.ToString("000"));
                }

                sb.Append(Path.GetExtension(Uri.AbsoluteUri));

                return sb.ToString();
            }
        }

        public string GetFullPath(string basePath) => Path.Combine(basePath, Blog, FileName);

        public bool Exists(string basePath) => File.Exists(GetFullPath(basePath));

        public async Task SaveToFileAsync(Stream source, string basePath)
        {
            var fullPath = GetFullPath(basePath);

            fullPath.EnsurePathExists();

            using (var target = File.OpenWrite(fullPath))
                await source.CopyToAsync(target);
        }
    }
}
