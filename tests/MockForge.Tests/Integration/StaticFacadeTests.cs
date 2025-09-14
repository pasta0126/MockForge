namespace MockForge.Tests.Integration;

public class StaticFacadeTests
{
    [Fact]
    public void MockForge_StaticAccess_WorksCorrectly()
    {
        // Arrange & Act
        var name = MockForge.Name;
        var address = MockForge.Address;
        var internet = MockForge.Internet;
        var number = MockForge.Number;
        var lorem = MockForge.Lorem;

        // Assert
        Assert.NotNull(name);
        Assert.NotNull(address);
        Assert.NotNull(internet);
        Assert.NotNull(number);
        Assert.NotNull(lorem);
    }

    [Fact]
    public void Configure_WithLocaleAndSeed_UpdatesInstance()
    {
        try
        {
            // Arrange
            const string expectedLocale = "es";
            const int expectedSeed = 99999;

            // Act
            MockForge.Configure(expectedLocale, expectedSeed);

            // Assert
            Assert.Equal(expectedLocale, MockForge.Locale);
            Assert.Equal(expectedSeed, MockForge.Seed);
        }
        finally
        {
            MockForge.Reset();
        }
    }

    [Fact]
    public void SetInstance_WithCustomInstance_UpdatesStaticAccess()
    {
        try
        {
            // Arrange
            var customInstance = new MockForgeCore("es", 12345);

            // Act
            MockForge.SetInstance(customInstance);

            // Assert
            Assert.Equal("es", MockForge.Locale);
            Assert.Equal(12345, MockForge.Seed);
        }
        finally
        {
            MockForge.Reset();
        }
    }

    [Fact]
    public void Reset_ResetsToDefault()
    {
        try
        {
            // Arrange
            MockForge.Configure("es", 12345);

            // Act
            MockForge.Reset();

            // Assert
            Assert.Equal("en", MockForge.Locale);
        }
        finally
        {
            MockForge.Reset();
        }
    }

    [Fact]
    public async Task IntegrationTest_GenerateCompleteProfile()
    {
        try
        {
            // Arrange
            MockForge.Configure("en", 12345);

            // Act
            var fullName = await MockForge.Name.FullNameAsync();
            var streetAddress = await MockForge.Address.StreetAddressAsync();
            var email = await MockForge.Internet.EmailAsync();
            var phoneNumber = await MockForge.Number.PhoneNumberAsync();
            var bio = await MockForge.Lorem.SentenceAsync();

            // Assert
            Assert.NotNull(fullName);
            Assert.NotEmpty(fullName);
            
            Assert.NotNull(streetAddress);
            Assert.NotEmpty(streetAddress);
            
            Assert.NotNull(email);
            Assert.Contains("@", email);
            
            Assert.NotNull(phoneNumber);
            Assert.NotEmpty(phoneNumber);
            
            Assert.NotNull(bio);
            Assert.NotEmpty(bio);
        }
        finally
        {
            MockForge.Reset();
        }
    }
}