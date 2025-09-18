using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("yes-no")]
        public ActionResult<string> GetYesNo()
        {
            var provider = mockForge.Get<AnswerProvider>();
            return Ok(provider.YesNo());
        }

        [HttpGet("true-false")]
        public ActionResult<bool> GetTrueFalse()
        {
            var provider = mockForge.Get<AnswerProvider>();
            return Ok(provider.TrueFalse());
        }

        [HttpGet("magic-8-ball")]
        public ActionResult<string> GetMagic8Ball()
        {
            var provider = mockForge.Get<AnswerProvider>();
            return Ok(provider.Magic8Ball());
        }
    }
}