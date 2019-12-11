using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Services;
using PhotoAlbum.Models;

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

        [HttpPatch("{path}/details")]
        public IActionResult PatchDetails(string path, [FromBody] AlbumDetails details)
        {
            path = path.UrlDecode();
            if (string.IsNullOrWhiteSpace(path)) return BadRequest("Path is not optional");
            _service.UpdateAlbumDetails(path, details);
            return Ok("details updated");
        }
    }
}