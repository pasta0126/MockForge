using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("person")]
        public ActionResult<Person> GetPerson([FromQuery] bool withNobleTitle = false, [FromQuery] bool withTitle = false, [FromQuery] bool withMiddleName = false, [FromQuery] int? maxAge = null)
        {
            var provider = mockForge.Get<IdentityProvider>();
            var person = provider.Person(withNobleTitle, withTitle, withMiddleName, maxAge);
            return Ok(person);
        }

        [HttpGet("person/male")]
        public ActionResult<Person> GetMalePerson([FromQuery] bool withNobleTitle = false, [FromQuery] bool withTitle = false, [FromQuery] bool withMiddleName = false, [FromQuery] int? maxAge = null)
        {
            var provider = mockForge.Get<IdentityProvider>();
            var person = provider.MalePerson("Male", "Human", withNobleTitle, withTitle, withMiddleName, maxAge);
            return Ok(person);
        }

        [HttpGet("person/female")]
        public ActionResult<Person> GetFemalePerson([FromQuery] bool withNobleTitle = false, [FromQuery] bool withTitle = false, [FromQuery] bool withMiddleName = false, [FromQuery] int? maxAge = null)
        {
            var provider = mockForge.Get<IdentityProvider>();
            var person = provider.FemalePerson("Female", "Human", withNobleTitle, withTitle, withMiddleName, maxAge);
            return Ok(person);
        }
    }
}
