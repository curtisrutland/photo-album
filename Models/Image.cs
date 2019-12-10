using System.IO;

namespace PhotoAlbum.Models
{
    public class Image
    {
        public string Path { get; set; }
        public string Name => new FileInfo(Path).Name;
        public string Url => $"/image/{Path.UrlEncode()}";
    }
}