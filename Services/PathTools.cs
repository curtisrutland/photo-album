using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using PhotoAlbum.Models.Options;

namespace PhotoAlbum.Services
{
    public interface IPathService
    {
        void AddPath(string hash, string path);
        bool IsFileAnImage(FileSystemInfo file);
        bool IsPathAllowed(FileSystemInfo file);
        void RemovePath(string hash);
        string GetPath(string hash);
    }

    public class PathService : IPathService
    {
        private readonly Dictionary<string, string> _hashesToPaths = new Dictionary<string, string>();
        private readonly string[] _imageExtensions;
        private readonly Regex[] _blacklistedPaths;
        public PathService(IOptions<AlbumSettings> settings)
        {
            _imageExtensions = settings.Value.ImageExtensions;
            _blacklistedPaths = settings.Value.BlacklistedPaths
              .Select(p => new Regex(p, RegexOptions.Multiline))
              .ToArray();
        }

        public bool IsFileAnImage(FileSystemInfo file) => file.Extension.EqualsAny(_imageExtensions, true);

        public bool IsPathAllowed(FileSystemInfo file) => !file.FullName.MatchesAny(_blacklistedPaths);

        public void AddPath(string hash, string path) => _hashesToPaths[hash] = path;

        public void RemovePath(string hash) => _hashesToPaths.Remove(hash);

        public string GetPath(string hash) => _hashesToPaths.ContainsKey(hash) ? _hashesToPaths[hash] : null;

    }
}