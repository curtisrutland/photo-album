using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Services;
using PhotoAlbum.Models;
using PhotoAlbum.Exceptions;

namespace PhotoAlbum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _service;

        public AlbumsController(IAlbumService service) => _service = service;

        [HttpGet]
        public Album[] Get() => _service.GetAlbums();

        [HttpPatch("{hash}")]
        public IActionResult PatchDetails(string hash, [FromBody] AlbumDetails details)
        {
            if (string.IsNullOrWhiteSpace(hash)) return BadRequest("Path is not optional");
            try
            {
                var updatedDetails = _service.UpdateAlbumDetails(hash.UrlDecode(), details);
                return Ok(updatedDetails);
            }
            catch (HashNotFoundException)
            {
                return NotFound();
            }
        }
    }
}