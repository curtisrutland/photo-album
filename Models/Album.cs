using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PhotoAlbum.Models
{
    public class Album
    {
        private const string DETAILS_FILE_NAME = ".albumdetails.json";

        [JsonIgnore]
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
        public string[] ImagePaths { get; set; }
        public Album[] SubAlbums { get; set; }
        public AlbumDetails Details { get; set; }

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