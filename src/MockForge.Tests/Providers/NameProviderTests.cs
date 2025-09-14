using FluentAssertions;
using MockForge.Core.Locales;
using MockForge.Core.Random;
using MockForge.Providers.Name;
using Xunit;
namespace MockForge.Tests;
public class NameProviderTests
{
    [Fact]
    public async Task FullAsync_NotEmpty()
    {
        var rnd = new ThreadSafeRandomizer(123);
        var store = new EmbeddedLocaleStore(typeof(NameProviderTests).Assembly);
        var p = new NameProvider(rnd, store, "es");
        var full = await p.FullAsync();
        full.Should().NotBeNullOrWhiteSpace();
    }
}
