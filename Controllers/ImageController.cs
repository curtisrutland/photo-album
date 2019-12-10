using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace PhotoAlbum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpGet("{path}")]
        public IActionResult Get(string path)
        {
            var file = new FileInfo(path.UrlDecode());
            if(!file.Exists) return NotFound();
            var mimeType = file.GetMimeType();
            if(mimeType == null) return StatusCode(500, "Cannot determine type of file at path");
            if(!mimeType.Contains("image")) return StatusCode(400, "Requested file was not an image");
            return File(file.ReadAllBytes(), mimeType);
        }
    }
}