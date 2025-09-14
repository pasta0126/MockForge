using FluentAssertions;
using MockForge.Core.Locales;
using MockForge.Core.Random;
using MockForge.Providers.Name;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

public class NameProviderTests
{
    [Fact]
    public async Task FullAsync_NotEmpty()
    {
        var asm = Assembly.Load("MockForge.Locales");
        // diagnóstico: confirma que el recurso existe
        asm.GetManifestResourceNames().Should().Contain("MockForge.Locales.es.json");

        var rnd = new ThreadSafeRandomizer(123);
        var store = new EmbeddedLocaleStore(asm);
        var p = new NameProvider(rnd, store, "es");

        var firsts = await store.GetListAsync("es", "name.first");
        firsts.Should().NotBeEmpty("es.json debe tener 'name.first'");

        var full = await p.FullAsync();
        full.Should().NotBeNullOrWhiteSpace();
    }
}
