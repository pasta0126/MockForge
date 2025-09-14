using MockForge.Core;
using MockForge.Locales;
using MockForge.Providers;
using MockForge.Templates;

namespace MockForge.Tests.Providers;

public class NameProviderTests
{
    private const int FixedSeed = 12345;

    [Fact]
    public async Task FirstNameAsync_WithEnglishLocale_ReturnsValidName()
    {
        // Arrange
        var provider = CreateNameProvider("en");

        // Act
        var firstName = await provider.FirstNameAsync();

        // Assert
        Assert.NotNull(firstName);
        Assert.NotEmpty(firstName);
    }

    [Fact]
    public async Task LastNameAsync_WithEnglishLocale_ReturnsValidName()
    {
        // Arrange
        var provider = CreateNameProvider("en");

        // Act
        var lastName = await provider.LastNameAsync();

        // Assert
        Assert.NotNull(lastName);
        Assert.NotEmpty(lastName);
    }

    [Fact]
    public async Task FullNameAsync_WithEnglishLocale_ReturnsFirstAndLastName()
    {
        // Arrange
        var provider = CreateNameProvider("en");

        // Act
        var fullName = await provider.FullNameAsync();

        // Assert
        Assert.NotNull(fullName);
        Assert.Contains(" ", fullName);
        var parts = fullName.Split(' ');
        Assert.Equal(2, parts.Length);
    }

    [Fact]
    public async Task FullNameWithMiddleAsync_WithEnglishLocale_ReturnsThreeNames()
    {
        // Arrange
        var provider = CreateNameProvider("en");

        // Act
        var fullName = await provider.FullNameWithMiddleAsync();

        // Assert
        Assert.NotNull(fullName);
        var parts = fullName.Split(' ');
        Assert.Equal(3, parts.Length);
    }

    [Fact]
    public async Task FormalNameAsync_WithEnglishLocale_ReturnsValidName()
    {
        // Arrange
        var provider = CreateNameProvider("en");

        // Act
        var formalName = await provider.FormalNameAsync();

        // Assert
        Assert.NotNull(formalName);
        Assert.NotEmpty(formalName);
    }

    [Fact]
    public async Task FirstNameAsync_WithSpanishLocale_ReturnsValidName()
    {
        // Arrange
        var provider = CreateNameProvider("es");

        // Act
        var firstName = await provider.FirstNameAsync();

        // Assert
        Assert.NotNull(firstName);
        Assert.NotEmpty(firstName);
    }

    [Fact]
    public async Task FirstNameAsync_WithSameSeed_ReturnsSameResult()
    {
        // Arrange
        var provider1 = CreateNameProvider("en");
        var provider2 = CreateNameProvider("en");

        // Act
        var name1 = await provider1.FirstNameAsync();
        var name2 = await provider2.FirstNameAsync();

        // Assert
        Assert.Equal(name1, name2);
    }

    [Fact]
    public void Locale_ReturnsConfiguredLocale()
    {
        // Arrange & Act
        var provider = CreateNameProvider("es");

        // Assert
        Assert.Equal("es", provider.Locale);
    }

    private static INameProvider CreateNameProvider(string locale)
    {
        var random = new RandomGenerator(FixedSeed);
        var localeDataProvider = new EmbeddedLocaleDataProvider();
        var templateEngine = new TemplateEngine();
        
        return new NameProvider(random, localeDataProvider, templateEngine, locale);
    }
}