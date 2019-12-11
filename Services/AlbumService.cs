using Microsoft.Extensions.Options;
using PhotoAlbum.Models.Options;
using PhotoAlbum.Models;
using System.IO;
using System.Linq;

namespace PhotoAlbum.Services
{
    public interface IAlbumService
    {
        Album[] GetAlbums();
        //Album GetAlbum(DirectoryInfo directory, bool recursive);
        void UpdateAlbumDetails(string path, AlbumDetails details);
    }

    public class AlbumService : IAlbumService
    {
        private readonly IPathTools _pathTools;
        private readonly string[] _rootPaths;
        private readonly IImageService _imageService;

        public AlbumService(IOptions<AlbumSettings> settings, IPathTools pathTools, IImageService imageService)
        {
            _rootPaths = settings.Value.AlbumRootPaths;
            _pathTools = pathTools;
            _imageService = imageService;
        }

        public Album[] GetAlbums() => _rootPaths.Select(p => GetAlbum(p.AsDirectoryPath())).ToArray();

        private Album GetAlbum(DirectoryInfo directory, bool recursive = true)
        {
            var subAlbums = recursive ? GetSubAlbums(directory) : new Album[0];
            var imageFiles = _imageService.GetImagesFromDirectory(directory);
            return new Album(directory.FullName, imageFiles, subAlbums);
        }

        public void UpdateAlbumDetails(string path, AlbumDetails details)
        {
            var album = GetAlbum(path.AsDirectoryPath(), false);
            album.UpdateDetails(details);
        }

        private Album[] GetSubAlbums(DirectoryInfo directory) => directory
            .GetDirectories()
            .Where(_pathTools.IsPathAllowed)
            .Select(dir => GetAlbum(dir))
            .Where(dir => !dir.IsEmpty)
            .ToArray();

    }
}