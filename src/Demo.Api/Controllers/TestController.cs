using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers.Name;
using MockForge.Providers.Address;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IMockForge forge) : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("Hola mundo");
    }

    [HttpGet("first-name")]
    public async Task<ActionResult<string>> FirstName()
    {
        return Ok(await forge.Get<NameProvider>().FirstAsync());
    }

    [HttpGet("last-name")]
    public async Task<ActionResult<string>> LastName()
    {
        return Ok(await forge.Get<NameProvider>().LastAsync());
    }

    [HttpGet("full-name")]
    public async Task<ActionResult<string>> FullName()
    {
        return Ok(await forge.Get<NameProvider>().FullAsync());
    }

    [HttpGet("city")]
    public async Task<ActionResult<string>> City()
    {
        return Ok(await forge.Get<AddressProvider>().CityAsync());
    }
}
