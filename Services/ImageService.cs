using System.IO;
using System.Linq;
using PhotoAlbum.Exceptions;
using PhotoAlbum.Models;

namespace PhotoAlbum.Services
{
    public interface IImageService
    {
        Image[] GetImagesFromDirectory(DirectoryInfo directory);
        string GetPath(string hash);
        Image RenameImageAtHash(string hash, string newFileName);
    }

    public class ImageService : IImageService
    {
        private readonly IPathService _pathService;
        public ImageService(IPathService pathTools) => _pathService = pathTools;

        public string GetPath(string hash) => _pathService.GetPath(hash);

        public Image RenameImageAtHash(string hash, string newFileName)
        {
            var path = _pathService.GetPath(hash);
            if(path == null) 
                throw new HashNotFoundException();
            var file = path.AsFilePath();
            var newFullPath = Path.Combine(file.Directory.FullName, $"{newFileName}{file.Extension}");
            if (File.Exists(newFullPath))
                throw new FileAlreadyExistsException();
            file.MoveTo(newFullPath);
            _pathService.RemovePath(hash);
            return CreateImage(file);
        }

        public Image[] GetImagesFromDirectory(DirectoryInfo directory) => directory
            .GetFiles()
            .Where(_pathService.IsPathAllowed)
            .Where(_pathService.IsFileAnImage)
            .Select(CreateImage)
            .ToArray();

        private Image CreateImage(FileInfo file)
        {
            var image = new Image
            {
                Path = file.FullName,
                Hash = file.FullName.HashMD5()
            };
            _pathService.AddPath(image.Hash, image.Path);
            return image;
        }
    }
}