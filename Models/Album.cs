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
        public Album(string path, Image[] images, Album[] subAlbums)
        {
            Path = path;
            Images = images;
            SubAlbums = subAlbums;
            LoadDetails();
        }

        public string Path { get; set; }
        public Image[] Images { get; set; } = new Image[0];
        public Album[] SubAlbums { get; set; } = new Album[0];
        public AlbumDetails Details { get; set; }

        [JsonIgnore]
        public bool IsEmpty => !Images.Any() && SubAlbums.All(a => a.IsEmpty);

        public void LoadDetails()
        {
            var details = AlbumDetails.Load(DetailsFilePath);
            if (string.IsNullOrWhiteSpace(details.CoverImageUrl))
                details.CoverImageUrl = Images?.FirstOrDefault()?.Url;
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