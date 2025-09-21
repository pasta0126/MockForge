using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("bmp")]
        [Produces("image/bmp")]
        public IActionResult GetBmp([FromQuery] int width = 256, [FromQuery] int height = 256, [FromQuery] int tileSize = 32)
        {
            var provider = mockForge.Get<ImageProvider>();
            var bytes = provider.GenerateRandomBitmap(width, height, tileSize);
            return File(bytes, "image/bmp");
        }

        [HttpGet("png/rgb")]
        [Produces("image/png")]
        public IActionResult GetPngRgb([FromQuery] int width = 256, [FromQuery] int height = 256, [FromQuery] int tileSize = 32, [FromQuery] int delta = 30)
        {
            var provider = mockForge.Get<ImageProvider>();
            var bytes = provider.GeneratePngRGBNative(width, height, tileSize, delta);
            return File(bytes, "image/png");
        }

        [HttpGet("png/hsv")]
        [Produces("image/png")]
        public IActionResult GetPngHsv([FromQuery] int width = 256, [FromQuery] int height = 256, [FromQuery] int tileSize = 32, [FromQuery] float maxHueStep = 15f)
        {
            var provider = mockForge.Get<ImageProvider>();
            var bytes = provider.GeneratePngHSVNative(width, height, tileSize, maxHueStep);
            return File(bytes, "image/png");
        }
    }
}
