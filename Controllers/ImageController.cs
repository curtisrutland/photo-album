using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Exceptions;
using PhotoAlbum.Models;
using PhotoAlbum.Services;

namespace PhotoAlbum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService) => _imageService = imageService;

        [HttpGet("{hash}")]
        public IActionResult Get(string hash)
        {
            var path = _imageService.GetPath(hash.UrlDecode());
            if (string.IsNullOrWhiteSpace(path)) return NotFound();
            var file = new FileInfo(path);
            if (!file.Exists) return NotFound();
            var mimeType = file.GetMimeType();
            if (mimeType == null) return StatusCode(500, "Cannot determine type of file at hash");
            if (!mimeType.Contains("image")) return StatusCode(400, "Requested file was not a ");
            return File(file.ReadAllBytes(), mimeType);
        }

        [HttpPatch("{hash}")]
        public IActionResult Patch(string hash, [FromBody] RenameImageRequest request)
        {
            if(string.IsNullOrWhiteSpace(request?.Name))
                return NotFound();
            try
            {
                var newImage = _imageService.RenameImageAtHash(hash.UrlDecode(), request.Name);
                return Ok(newImage);
            }
            catch (HashNotFoundException)
            {
                return NotFound();
            }
            catch (FileAlreadyExistsException)
            {
                return StatusCode(400, new { Error = $"File with name '{request.Name}' already exists in album" });
            }
        }
    }
}