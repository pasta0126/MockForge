using Microsoft.Extensions.DependencyInjection;
using MockForge.Extensions;
using MockForge.Providers;

namespace MockForge.Tests.Integration;

public class DependencyInjectionTests
{
    [Fact]
    public void AddMockForge_RegistersAllServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMockForge();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IMockForge>());
        Assert.NotNull(serviceProvider.GetService<INameProvider>());
        Assert.NotNull(serviceProvider.GetService<IAddressProvider>());
        Assert.NotNull(serviceProvider.GetService<IInternetProvider>());
        Assert.NotNull(serviceProvider.GetService<INumberProvider>());
        Assert.NotNull(serviceProvider.GetService<ILoremProvider>());
    }

    [Fact]
    public void AddMockForge_WithLocaleAndSeed_ConfiguresCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        const string locale = "es";
        const int seed = 12345;

        // Act
        services.AddMockForge(locale, seed);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var mockForge = serviceProvider.GetRequiredService<IMockForge>();
        Assert.Equal(locale, mockForge.Locale);
        Assert.Equal(seed, mockForge.Seed);
    }

    [Fact]
    public void AddMockForge_WithOptions_ConfiguresCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMockForge(options =>
        {
            options.Locale = "es";
            options.Seed = 54321;
        });
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var mockForge = serviceProvider.GetRequiredService<IMockForge>();
        Assert.Equal("es", mockForge.Locale);
        Assert.Equal(54321, mockForge.Seed);
    }

    [Fact]
    public async Task DependencyInjection_GeneratesData()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMockForge("en", 12345);
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var mockForge = serviceProvider.GetRequiredService<IMockForge>();
        var name = await mockForge.Name.FirstNameAsync();

        // Assert
        Assert.NotNull(name);
        Assert.NotEmpty(name);
    }

    [Fact]
    public void DependencyInjection_ProvidersAreSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMockForge();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var provider1 = serviceProvider.GetRequiredService<INameProvider>();
        var provider2 = serviceProvider.GetRequiredService<INameProvider>();

        // Assert
        Assert.Same(provider1, provider2);
    }
}