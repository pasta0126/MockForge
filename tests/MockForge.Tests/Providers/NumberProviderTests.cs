using MockForge.Core;
using MockForge.Locales;
using MockForge.Providers;
using MockForge.Templates;

namespace MockForge.Tests.Providers;

public class NumberProviderTests
{
    private const int FixedSeed = 12345;

    [Fact]
    public void Integer_WithDefaultRange_ReturnsValueInRange()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var result = provider.Integer();

        // Assert
        Assert.True(result >= 0 && result <= 100);
    }

    [Fact]
    public void Integer_WithCustomRange_ReturnsValueInRange()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var result = provider.Integer(10, 20);

        // Assert
        Assert.True(result >= 10 && result <= 20);
    }

    [Fact]
    public void Decimal_WithDefaultRange_ReturnsValueInRange()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var result = provider.Decimal();

        // Assert
        Assert.True(result >= 0 && result <= 100);
    }

    [Fact]
    public void Decimal_WithCustomRange_ReturnsValueInRange()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var result = provider.Decimal(10.5m, 20.5m);

        // Assert
        Assert.True(result >= 10.5m && result <= 20.5m);
    }

    [Fact]
    public void Double_WithDefaultRange_ReturnsValueInRange()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var result = provider.Double();

        // Assert
        Assert.True(result >= 0.0 && result <= 100.0);
    }

    [Fact]
    public void Double_WithCustomRange_ReturnsValueInRange()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var result = provider.Double(10.5, 20.5);

        // Assert
        Assert.True(result >= 10.5 && result <= 20.5);
    }

    [Fact]
    public async Task PhoneNumberAsync_ReturnsValidFormat()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var phoneNumber = await provider.PhoneNumberAsync();

        // Assert
        Assert.NotNull(phoneNumber);
        Assert.NotEmpty(phoneNumber);
        Assert.Matches(@"[\d\-\(\)\s\.+]+", phoneNumber);
    }

    [Fact]
    public async Task CreditCardNumberAsync_ReturnsValidFormat()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var creditCard = await provider.CreditCardNumberAsync();

        // Assert
        Assert.NotNull(creditCard);
        Assert.NotEmpty(creditCard);
        Assert.Matches(@"[\d\-\s]+", creditCard);
    }

    [Fact]
    public async Task SocialSecurityNumberAsync_ReturnsValidFormat()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act
        var ssn = await provider.SocialSecurityNumberAsync();

        // Assert
        Assert.NotNull(ssn);
        Assert.NotEmpty(ssn);
        Assert.Matches(@"[\d\-\s]+", ssn);
    }

    [Fact]
    public void Percentage_ReturnsValueBetween0And100()
    {
        // Arrange
        var provider = CreateNumberProvider();

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var percentage = provider.Percentage();
            Assert.True(percentage >= 0 && percentage <= 100);
        }
    }

    [Fact]
    public void Integer_WithSameSeed_ReturnsSameResult()
    {
        // Arrange
        var provider1 = CreateNumberProvider();
        var provider2 = CreateNumberProvider();

        // Act
        var result1 = provider1.Integer();
        var result2 = provider2.Integer();

        // Assert
        Assert.Equal(result1, result2);
    }

    private static INumberProvider CreateNumberProvider()
    {
        var random = new RandomGenerator(FixedSeed);
        var localeDataProvider = new EmbeddedLocaleDataProvider();
        var templateEngine = new TemplateEngine();
        
        return new NumberProvider(random, localeDataProvider, templateEngine, "en");
    }
}