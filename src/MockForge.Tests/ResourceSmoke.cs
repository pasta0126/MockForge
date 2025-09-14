using FluentAssertions;
using MockForge.Core.Locales;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace MockForge.Tests;

public class ResourceSmoke
{
    [Fact]
    public async Task Loads_Es_NameFirst()
    {
        var asm = Assembly.Load("MockForge.Locales");
        asm.GetManifestResourceNames().Should().Contain("MockForge.Locales.es.json");

        var store = new EmbeddedLocaleStore(asm);
        var list = await store.GetListAsync("es", "name.first");
        list.Should().NotBeEmpty("es.json debe contener 'name.first'");
    }
}
