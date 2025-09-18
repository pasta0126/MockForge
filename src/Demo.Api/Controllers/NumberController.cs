using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("random-decimal")]
        public ActionResult<decimal> GetRandomDecimal(
            [FromQuery] decimal? min = 0, 
            [FromQuery] decimal? max = 1, 
            [FromQuery] int? decimals = 2)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomDecimal(min, max, decimals));
        }

        [HttpGet("random-int")]
        public ActionResult<int> GetRandomInt([FromQuery] int? min = 0, [FromQuery] int? max = 100)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomNumber(min, max));
        }

        [HttpGet("random-double")]
        public ActionResult<double> GetRandomDouble([FromQuery] double? min = 0, [FromQuery] double? max = 1)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomNumber(min, max));
        }

        [HttpGet("random-float")]
        public ActionResult<float> GetRandomFloat([FromQuery] float? min = 0, [FromQuery] float? max = 1)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomNumber(min, max));
        }

        [HttpGet("random-long")]
        public ActionResult<long> GetRandomLong([FromQuery] long? min = 0, [FromQuery] long? max = 1000000)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomNumber(min, max));
        }

        [HttpGet("random-short")]
        public ActionResult<short> GetRandomShort([FromQuery] short? min = 0, [FromQuery] short? max = 1000)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomNumber(min, max));
        }

        [HttpGet("random-byte")]
        public ActionResult<byte> GetRandomByte([FromQuery] byte? min = 0, [FromQuery] byte? max = 255)
        {
            var provider = mockForge.Get<NumberProvider>();
            return Ok(provider.RandomNumber(min, max));
        }
    }
}