using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using PhotoAlbum.Models.Options;

namespace PhotoAlbum.Services
{
    public interface IPathTools
    {
        bool IsFileAnImage(FileSystemInfo file);
        bool IsPathAllowed(FileSystemInfo file);
    }

    public class PathTools : IPathTools
    {
        private readonly string[] _imageExtensions;
        private readonly Regex[] _blacklistedPaths;
        public PathTools(IOptions<AlbumSettings> settings)
        {
            _imageExtensions = settings.Value.ImageExtensions;
            _blacklistedPaths = settings.Value.BlacklistedPaths
              .Select(p => new Regex(p, RegexOptions.Multiline))
              .ToArray();
        }

        public bool IsFileAnImage(FileSystemInfo file) => file.Extension.EqualsAny(_imageExtensions, true);

        public bool IsPathAllowed(FileSystemInfo file) => !file.FullName.MatchesAny(_blacklistedPaths);

    }
}