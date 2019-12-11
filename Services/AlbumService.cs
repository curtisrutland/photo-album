using Microsoft.Extensions.Options;
using PhotoAlbum.Models.Options;
using PhotoAlbum.Models;
using System.IO;
using System.Linq;
using PhotoAlbum.Exceptions;

namespace PhotoAlbum.Services
{
    public interface IAlbumService
    {
        Album[] GetAlbums();
        //Album GetAlbum(DirectoryInfo directory, bool recursive);
        AlbumDetails UpdateAlbumDetails(string path, AlbumDetails details);
    }

    public class AlbumService : IAlbumService
    {
        private readonly IPathService _pathService;
        private readonly string[] _rootPaths;
        private readonly IImageService _imageService;

        public AlbumService(IOptions<AlbumSettings> settings, IPathService pathTools, IImageService imageService)
        {
            _rootPaths = settings.Value.AlbumRootPaths;
            _pathService = pathTools;
            _imageService = imageService;
        }

        public Album[] GetAlbums() => _rootPaths.Select(p => GetAlbumAtDirectory(p.AsDirectoryPath())).ToArray();

        public AlbumDetails UpdateAlbumDetails(string hash, AlbumDetails details)
        {
            var path = _pathService.GetPath(hash);
            if(path == null)
                throw new HashNotFoundException();
            return Album.UpdateDetails(path, details);
        }

        private Album GetAlbumAtDirectory(DirectoryInfo directory)
        {
            var subAlbums = GetSubAlbums(directory);
            var imageFiles = _imageService.GetImagesFromDirectory(directory);
            var album = new Album(directory.FullName, imageFiles, subAlbums);
            _pathService.AddPath(album.Hash, album.Path);
            return album;
        }

        private Album[] GetSubAlbums(DirectoryInfo directory) => directory
            .GetDirectories()
            .Where(_pathService.IsPathAllowed)
            .Select(GetAlbumAtDirectory)
            .Where(dir => !dir.IsEmpty)
            .ToArray();

    }
}