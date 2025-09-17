using FluentAssertions;
using MockForge.Core.Random;
using MockForge.Providers.Name;
using System.Threading.Tasks;
using Xunit;

namespace MockForge.Tests;

public class NameProviderTests
{
    [Fact]
    public async Task FullAsync_NotEmpty()
    {
        var rnd = new ThreadSafeRandomizer(123);
        var p = new NameProvider(rnd);
        var full = await p.FullAsync();
        full.Should().NotBeNullOrWhiteSpace();
        full.Split(' ').Length.Should().Be(2);
    }
}
