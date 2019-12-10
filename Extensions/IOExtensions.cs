using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace PhotoAlbum
{
    public static class IOExtensions
    {
        public static string ReadAllText(this FileInfo info) => File.ReadAllText(info.FullName);
        public static byte[] ReadAllBytes(this FileInfo info) => File.ReadAllBytes(info.FullName);
        public static string GetMimeType(this FileInfo info)
        {
            if(!new FileExtensionContentTypeProvider().TryGetContentType(info.Name, out var mimeType)) 
                return null;
            return mimeType;
        }
    }
}