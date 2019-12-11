using System.Text.Json.Serialization;

namespace PhotoAlbum.Models
{
    public class Image
    {
        [JsonIgnore] public string Path { get; set; }
        [JsonIgnore] public string Hash { get; set; }
        public string Name => Path.AsFilePath().NameWithoutExtension();
        public string Url => $"/images/{Hash.UrlEncode()}";
    }
}