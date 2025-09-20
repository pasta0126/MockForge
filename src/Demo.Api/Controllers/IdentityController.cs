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
        public ActionResult<Person> GetPerson([FromQuery] bool withNobleTitle = false, [FromQuery] bool withTitle = false, [FromQuery] bool withMiddleName = false, [FromQuery] int? age = null)
        {
            var provider = mockForge.Get<IdentityProvider>();
            var person = provider.Person(withNobleTitle, withTitle, withMiddleName, age);
            return Ok(person);
        }

        [HttpGet("person/male")]
        public ActionResult<Person> GetMalePerson([FromQuery] bool withNobleTitle = false, [FromQuery] bool withTitle = false, [FromQuery] bool withMiddleName = false, [FromQuery] int? age = null)
        {
            var provider = mockForge.Get<IdentityProvider>();
            var person = provider.MalePerson("Male", withNobleTitle, withTitle, withMiddleName, age);
            return Ok(person);
        }

        [HttpGet("person/female")]
        public ActionResult<Person> GetFemalePerson([FromQuery] bool withNobleTitle = false, [FromQuery] bool withTitle = false, [FromQuery] bool withMiddleName = false, [FromQuery] int? age = null)
        {
            var provider = mockForge.Get<IdentityProvider>();
            var person = provider.FemalePerson("Female", withNobleTitle, withTitle, withMiddleName, age);
            return Ok(person);
        }
    }
}
