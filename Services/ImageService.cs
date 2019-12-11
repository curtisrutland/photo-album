using System;
using System.Collections.Generic;
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
        void ResetImageMap();
    }

    public class ImageService : IImageService
    {
        private readonly Dictionary<string, string> _hashesToPaths = new Dictionary<string, string>();
        private readonly IPathTools _pathTools;
        public ImageService(IPathTools pathTools) => _pathTools = pathTools;

        public void ResetImageMap() => _hashesToPaths.Clear();

        public string GetPath(string hash) => _hashesToPaths.ContainsKey(hash) ? _hashesToPaths[hash] : null;

        public Image RenameImageAtHash(string hash, string newFileName)
        {
            if (!_hashesToPaths.ContainsKey(hash))
                throw new HashNotFoundException();
            var path = _hashesToPaths[hash];
            var file = path.AsFilePath();
            var newFullPath = Path.Combine(file.Directory.FullName, $"{newFileName}{file.Extension}");
            if (File.Exists(newFullPath))
                throw new FileAlreadyExistsException();
            file.MoveTo(newFullPath);
            _hashesToPaths.Remove(hash);
            return CreateImage(file);
        }

        public Image[] GetImagesFromDirectory(DirectoryInfo directory) => directory
            .GetFiles()
            .Where(_pathTools.IsPathAllowed)
            .Where(_pathTools.IsFileAnImage)
            .Select(CreateImage)
            .ToArray();

        private Image CreateImage(FileInfo file)
        {
            var image = new Image
            {
                Path = file.FullName,
                Hash = file.FullName.HashMD5()
            };
            _hashesToPaths[image.Hash] = image.Path;
            return image;
        }
    }
}