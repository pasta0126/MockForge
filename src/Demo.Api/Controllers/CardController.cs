using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("poker-card")]
        public ActionResult<string> GetPokerCard()
        {
            var provider = mockForge.Get<CardProvider>();
            return Ok(provider.PokerCard());
        }

        [HttpGet("spanish-card")]
        public ActionResult<string> GetSpanishCard()
        {
            var provider = mockForge.Get<CardProvider>();
            return Ok(provider.SpanishCard());
        }

        [HttpGet("uno-card")]
        public ActionResult<string> GetUnoCard()
        {
            var provider = mockForge.Get<CardProvider>();
            return Ok(provider.UnoCard());
        }

        [HttpGet("tarot-major-arcana")]
        public ActionResult<string> GetTarotMajorArcana()
        {
            var provider = mockForge.Get<CardProvider>();
            return Ok(provider.TarotMajorArcana());
        }

        [HttpGet("tarot-arcano-mayor")]
        public ActionResult<string> GetTarotArcanoMayor()
        {
            var provider = mockForge.Get<CardProvider>();
            return Ok(provider.TarotArcanoMayor());
        }
    }
}