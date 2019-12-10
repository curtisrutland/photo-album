using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace PhotoAlbum.Models
{
    public class Album
    {
        private const string DETAILS_FILE_NAME = ".albumdetails.json";
        private string DetailsFilePath => System.IO.Path.Combine(Path, DETAILS_FILE_NAME);

        public Album() { }
        public Album(string path, string[] imagePaths, Album[] subAlbums)
        {
            Path = path;
            ImagePaths = imagePaths;
            SubAlbums = subAlbums;
            LoadDetails();
        }

        public string Path { get; set; }
        public string[] ImagePaths { get; set; } = new string[0];
        public Album[] SubAlbums { get; set; } = new Album[0];
        public AlbumDetails Details { get; set; }

        [JsonIgnore]
        public bool IsEmpty => !ImagePaths.Any() && SubAlbums.All(a => a.IsEmpty);

        public void LoadDetails()
        {
            var details = AlbumDetails.Load(DetailsFilePath);
            if (string.IsNullOrWhiteSpace(details.CoverImagePath))
                details.CoverImagePath = ImagePaths?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(details.Name))
                details.Name = new DirectoryInfo(Path).Name;
            this.Details = details;
        }

        public void UpdateDetails(AlbumDetails incoming)
        {
            Details.Merge(incoming);
            Details.Save(DetailsFilePath);
        }
    }
}