using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StarWarsController : ControllerBase
    {
        private readonly IMockForge _mockForge;

        public StarWarsController(IMockForge mockForge)
        {
            _mockForge = mockForge;
        }

        #region Jedi Names

        [HttpPost("jedi/classic")]
        public ActionResult<string> GetJediNameClassic([FromBody] JediNameRequest request)
        {
            if (!IsValidJediRequest(request))
                return BadRequest("All fields (firstName, lastName, motherName, birthCity) are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.JediNameClassic(request.FirstName, request.LastName, request.MotherName, request.BirthCity));
        }

        [HttpPost("jedi/real-form")]
        public ActionResult<string> GetJediNameRealForm([FromBody] JediNameRequest request)
        {
            if (!IsValidJediRequest(request))
                return BadRequest("All fields (firstName, lastName, motherName, birthCity) are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.JediNameRealForm(request.FirstName, request.LastName, request.MotherName, request.BirthCity));
        }

        [HttpPost("jedi/2332")]
        public ActionResult<string> GetJediName2332([FromBody] JediNameRequest request)
        {
            if (!IsValidJediRequest(request))
                return BadRequest("All fields (firstName, lastName, motherName, birthCity) are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.JediName2332(request.FirstName, request.LastName, request.MotherName, request.BirthCity));
        }

        [HttpPost("jedi/from-ends")]
        public ActionResult<string> GetJediNameFromEnds([FromBody] JediNameRequest request)
        {
            if (!IsValidJediRequest(request))
                return BadRequest("All fields (firstName, lastName, motherName, birthCity) are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.JediNameFromEnds(request.FirstName, request.LastName, request.MotherName, request.BirthCity));
        }

        [HttpPost("jedi/astrology")]
        public ActionResult<string> GetJediNameAstrology([FromBody] JediAstrologyRequest request)
        {
            if (!IsValidJediAstrologyRequest(request))
                return BadRequest("All fields (firstName, lastName, birthCity, zodiacSign, zodiacElement) are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.JediNameAstrology(request.FirstName, request.LastName, request.BirthCity, 
                request.ZodiacSign, request.ZodiacElement));
        }

        [HttpPost("jedi/random")]
        public ActionResult<string> GetRandomJediName([FromBody] JediRandomRequest request)
        {
            if (!IsValidJediRandomRequest(request))
                return BadRequest("Required fields (firstName, lastName, motherName, birthCity) must be provided");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.RandomJediName(request.FirstName, request.LastName, request.MotherName, 
                request.BirthCity, request.ZodiacSign ?? "Aries", request.ZodiacElement ?? "Fire"));
        }

        #endregion

        #region Sith Names

        [HttpPost("sith/classic")]
        public ActionResult<string> GetSithNameClassic([FromBody] SithClassicRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PetName) || string.IsNullOrWhiteSpace(request.StreetName))
                return BadRequest("PetName and StreetName are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.SithNameClassic(request.PetName, request.StreetName));
        }

        [HttpPost("sith/method1")]
        public ActionResult<string> GetSithNameMethod1([FromBody] SithMethod1Request request)
        {
            if (string.IsNullOrWhiteSpace(request.RealName) || string.IsNullOrWhiteSpace(request.Emotion) || 
                string.IsNullOrWhiteSpace(request.Virtue))
                return BadRequest("RealName, Emotion, and Virtue are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.SithNameMethod1(request.RealName, request.Emotion, request.Virtue));
        }

        [HttpPost("sith/method2")]
        public ActionResult<string> GetSithNameMethod2([FromBody] SithMethod2Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Ambition) || string.IsNullOrWhiteSpace(request.RealName) || 
                string.IsNullOrWhiteSpace(request.Weakness) || string.IsNullOrWhiteSpace(request.ParentName))
                return BadRequest("All fields (ambition, realName, weakness, parentName) are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.SithNameMethod2(request.Ambition, request.RealName, request.Weakness, request.ParentName));
        }

        [HttpPost("sith/method3")]
        public ActionResult<string> GetSithNameMethod3([FromBody] SithMethod3Request request)
        {
            if (string.IsNullOrWhiteSpace(request.RealName) || string.IsNullOrWhiteSpace(request.Emotion))
                return BadRequest("RealName and Emotion are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.SithNameMethod3(request.RealName, request.Emotion));
        }

        [HttpPost("sith/random")]
        public ActionResult<string> GetRandomSithName([FromBody] SithRandomRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RealName))
                return BadRequest("RealName is required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.RandomSithName(request.RealName, request.PetName, request.StreetName, 
                request.Emotion ?? "anger", request.Virtue ?? "power", request.Ambition ?? "rule", 
                request.Weakness ?? "pride", request.ParentName));
        }

        #endregion

        #region Droid Names

        [HttpPost("droid/astromech")]
        public ActionResult<string> GetDroidNameAstromech([FromBody] DroidAstromechRequest request)
        {
            if (request.BirthMonth < 1 || request.BirthMonth > 12 || request.BirthDay < 1 || request.BirthDay > 31)
                return BadRequest("BirthMonth must be 1-12 and BirthDay must be 1-31");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.DroidNameAstromech(request.BirthMonth, request.BirthDay));
        }

        [HttpPost("droid/protocol")]
        public ActionResult<string> GetDroidNameProtocol([FromBody] DroidProtocolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName) || request.Age < 0)
                return BadRequest("FirstName is required and Age must be >= 0");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.DroidNameProtocol(request.FirstName, request.Age));
        }

        [HttpPost("droid/random")]
        public ActionResult<string> GetDroidNameRandom([FromBody] DroidRandomRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
                return BadRequest("FirstName and LastName are required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.DroidNameRandom(request.FirstName, request.LastName));
        }

        [HttpPost("droid/full-serial")]
        public ActionResult<string> GetDroidNameFullSerial([FromBody] DroidSerialRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SeriesPrefix))
                return BadRequest("SeriesPrefix is required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.DroidNameFullSerial(request.SeriesPrefix));
        }

        [HttpPost("droid/shortened")]
        public ActionResult<string> GetDroidNameShortened([FromBody] DroidSerialRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SeriesPrefix))
                return BadRequest("SeriesPrefix is required");

            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.DroidNameShortened(request.SeriesPrefix));
        }

        [HttpPost("droid/random-type")]
        public ActionResult<string> GetRandomDroidName([FromBody] DroidRandomTypeRequest request)
        {
            var provider = _mockForge.Get<StarWarsProvider>();
            return Ok(provider.RandomDroidName(request.FirstName, request.LastName, request.SeriesPrefix ?? "BB"));
        }

        #endregion

        #region Validation Methods

        private static bool IsValidJediRequest(JediNameRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.FirstName) &&
                   !string.IsNullOrWhiteSpace(request.LastName) &&
                   !string.IsNullOrWhiteSpace(request.MotherName) &&
                   !string.IsNullOrWhiteSpace(request.BirthCity);
        }

        private static bool IsValidJediAstrologyRequest(JediAstrologyRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.FirstName) &&
                   !string.IsNullOrWhiteSpace(request.LastName) &&
                   !string.IsNullOrWhiteSpace(request.BirthCity) &&
                   !string.IsNullOrWhiteSpace(request.ZodiacSign) &&
                   !string.IsNullOrWhiteSpace(request.ZodiacElement);
        }

        private static bool IsValidJediRandomRequest(JediRandomRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.FirstName) &&
                   !string.IsNullOrWhiteSpace(request.LastName) &&
                   !string.IsNullOrWhiteSpace(request.MotherName) &&
                   !string.IsNullOrWhiteSpace(request.BirthCity);
        }

        #endregion
    }

    #region Request Models

    public class JediNameRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string BirthCity { get; set; } = string.Empty;
    }

    public class JediAstrologyRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string BirthCity { get; set; } = string.Empty;
        public string ZodiacSign { get; set; } = string.Empty;
        public string ZodiacElement { get; set; } = string.Empty;
    }

    public class JediRandomRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string BirthCity { get; set; } = string.Empty;
        public string? ZodiacSign { get; set; }
        public string? ZodiacElement { get; set; }
    }

    public class SithClassicRequest
    {
        public string PetName { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
    }

    public class SithMethod1Request
    {
        public string RealName { get; set; } = string.Empty;
        public string Emotion { get; set; } = string.Empty;
        public string Virtue { get; set; } = string.Empty;
    }

    public class SithMethod2Request
    {
        public string Ambition { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;
        public string Weakness { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
    }

    public class SithMethod3Request
    {
        public string RealName { get; set; } = string.Empty;
        public string Emotion { get; set; } = string.Empty;
    }

    public class SithRandomRequest
    {
        public string RealName { get; set; } = string.Empty;
        public string? PetName { get; set; }
        public string? StreetName { get; set; }
        public string? Emotion { get; set; }
        public string? Virtue { get; set; }
        public string? Ambition { get; set; }
        public string? Weakness { get; set; }
        public string? ParentName { get; set; }
    }

    public class DroidAstromechRequest
    {
        public int BirthMonth { get; set; }
        public int BirthDay { get; set; }
    }

    public class DroidProtocolRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public class DroidRandomRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class DroidSerialRequest
    {
        public string SeriesPrefix { get; set; } = string.Empty;
    }

    public class DroidRandomTypeRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SeriesPrefix { get; set; }
    }

    #endregion
}