using Microsoft.Extensions.Options;
using PhotoAlbum.Models.Options;
using PhotoAlbum.Models;
using System.IO;
using System.Linq;

namespace PhotoAlbum.Services
{
    public interface IAlbumService
    {
        Album GetAlbum();
        Album GetAlbum(DirectoryInfo directory, bool recursive);
        void UpdateAlbumDetails(string path, AlbumDetails details);
    }

    public class AlbumService : IAlbumService
    {
        private readonly IPathTools _pathTools;
        private readonly string _rootPath;

        public AlbumService(IOptions<AlbumSettings> settings, IPathTools pathTools)
        {
            _rootPath = settings.Value.AlbumRootPath;
            _pathTools = pathTools;
        }

        public Album GetAlbum() => GetAlbum(new DirectoryInfo(_rootPath));

        public Album GetAlbum(DirectoryInfo directory, bool recursive = true)
        {
            var subAlbums = recursive ? GetSubAlbums(directory) : new Album[0];
            var imageFiles = GetImageFiles(directory);
            return new Album(directory.FullName, imageFiles, subAlbums);
        }

        public void UpdateAlbumDetails(string path, AlbumDetails details)
        {
            var album = GetAlbum(new DirectoryInfo(path), false);
            album.UpdateDetails(details);
        }

        private string[] GetImageFiles(DirectoryInfo directory) => directory
            .GetFiles()
            .Where(_pathTools.IsPathAllowed)
            .Where(_pathTools.IsFileAnImage)
            .Select(file => file.Name)
            .ToArray();

        private Album[] GetSubAlbums(DirectoryInfo directory) => directory
            .GetDirectories()
            .Where(_pathTools.IsPathAllowed)
            .Select(dir => GetAlbum(dir))
            .ToArray();

    }
}