using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArrayController(IMockForge mockForge) : ControllerBase
    {
        [HttpPost("pick-string")]
        public ActionResult<string> PickString([FromBody] string[] array)
        {
            if (array == null || array.Length == 0)
                return BadRequest("Array cannot be null or empty");

            var provider = mockForge.Get<ArrayProvider>();
            var result = provider.Pick(array);
            return Ok(result);
        }

        [HttpPost("pick-int")]
        public ActionResult<int?> PickInt([FromBody] int[] array)
        {
            if (array == null || array.Length == 0)
                return BadRequest("Array cannot be null or empty");

            var provider = mockForge.Get<ArrayProvider>();
            var result = provider.Pick(array);
            return Ok(result);
        }

        [HttpPost("pick-decimal")]
        public ActionResult<decimal?> PickDecimal([FromBody] decimal[] array)
        {
            if (array == null || array.Length == 0)
                return BadRequest("Array cannot be null or empty");

            var provider = mockForge.Get<ArrayProvider>();
            var result = provider.Pick(array);
            return Ok(result);
        }

        [HttpPost("pick-double")]
        public ActionResult<double?> PickDouble([FromBody] double[] array)
        {
            if (array == null || array.Length == 0)
                return BadRequest("Array cannot be null or empty");

            var provider = mockForge.Get<ArrayProvider>();
            var result = provider.Pick(array);
            return Ok(result);
        }
    }
}