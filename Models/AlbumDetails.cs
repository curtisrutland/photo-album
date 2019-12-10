using System.IO;
using Newtonsoft.Json;

namespace PhotoAlbum.Models
{
    public class AlbumDetails
    {
        public string Name { get; set; }
        public string CoverImagePath { get; set; }

        public void Merge(AlbumDetails incoming)
        {
            if (!string.IsNullOrWhiteSpace(incoming.Name))
                Name = incoming.Name;
            if (!string.IsNullOrWhiteSpace(incoming.CoverImagePath))
                CoverImagePath = incoming.CoverImagePath;
        }

        public void Save(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this));
            File.SetAttributes(path, FileAttributes.Hidden);
        }

        public static AlbumDetails Load(string path) => File.Exists(path)
          ? JsonConvert.DeserializeObject<AlbumDetails>(File.ReadAllText(path))
          : new AlbumDetails();
    }
}