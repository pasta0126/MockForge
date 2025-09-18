using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressionController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("fibonacci/{n}")]
        public ActionResult<string> GetFibonacci(int n)
        {
            if (n < 0 || n > 1000)
                return BadRequest("n must be between 0 and 1000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.Fibonacci(n).ToString());
        }

        [HttpGet("jacobsthal/{n}")]
        public ActionResult<string> GetJacobsthal(int n)
        {
            if (n < 0 || n > 1000)
                return BadRequest("n must be between 0 and 1000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.Jacobsthal(n).ToString());
        }

        [HttpGet("lucas/{n}")]
        public ActionResult<string> GetLucas(int n)
        {
            if (n < 0 || n > 1000)
                return BadRequest("n must be between 0 and 1000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.Lucas(n).ToString());
        }

        [HttpGet("pell/{n}")]
        public ActionResult<string> GetPell(int n)
        {
            if (n < 0 || n > 1000)
                return BadRequest("n must be between 0 and 1000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.Pell(n).ToString());
        }

        [HttpGet("hofstadter-q/{n}")]
        public ActionResult<string> GetHofstadterQ(int n)
        {
            if (n <= 0 || n > 1000)
                return BadRequest("n must be between 1 and 1000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.HofstadterQ(n).ToString());
        }

        [HttpGet("exotic/{n}")]
        public ActionResult<string> GetExotic(int n)
        {
            if (n < 0 || n > 1000)
                return BadRequest("n must be between 0 and 1000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.Exotic(n).ToString());
        }

        [HttpGet("logistic-map/{n}")]
        public ActionResult<double> GetLogisticMap(int n, [FromQuery] double r = 4.0, [FromQuery] double x0 = 0.2)
        {
            if (n < 0 || n > 10000)
                return BadRequest("n must be between 0 and 10000");

            var provider = mockForge.Get<ProgressionProvider>();
            return Ok(provider.LogisticMap(n, r, x0));
        }
    }
}