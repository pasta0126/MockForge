using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace MockForge.Tests;

public class FacadeTests
{
    [Fact]
    public async Task FirstName_Works()
    {
        var name = await MF.FirstName();
        name.Should().NotBeNullOrWhiteSpace();
    }
}
