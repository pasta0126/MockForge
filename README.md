# MockForge

MockForge is a modular and extensible .NET 8 library for generating mock data. It provides a comprehensive set of providers for generating names, addresses, internet data, numbers, and lorem ipsum text with support for multiple locales.

## Features

- 🎯 **Static Facade**: Easy-to-use static API for quick access
- 🔧 **Dependency Injection**: Full DI support with `AddMockForge` extension
- 🌍 **Multi-locale Support**: Built-in support for English (`en`) and Spanish (`es`) locales with fallback
- 🎲 **Thread-safe RNG**: Deterministic random generation with seed support
- 📝 **Template Engine**: Simple `{{token}}` replacement system
- 📦 **Modular Providers**: Separate providers for different data types
- 🧪 **Extensible**: Easy to add new providers and locales
- ⚡ **High Performance**: Optimized for speed with embedded JSON datasets

## Quick Start

### Using Static API

```csharp
using MockForge;

// Generate data using the static facade
var name = await MockForge.Name.FullNameAsync();
var email = await MockForge.Internet.EmailAsync();
var address = await MockForge.Address.FullAddressAsync();
var phone = await MockForge.Number.PhoneNumberAsync();

Console.WriteLine($"{name} - {email}");
Console.WriteLine($"{address} - {phone}");
```

### Configure with Locale and Seed

```csharp
// Configure for Spanish locale with fixed seed for deterministic results
MockForge.Configure("es", seed: 12345);

var nombre = await MockForge.Name.FullNameAsync();
var correo = await MockForge.Internet.EmailAsync();
Console.WriteLine($"{nombre} - {correo}");
```

### Using Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using MockForge.Extensions;

var services = new ServiceCollection();

// Register MockForge with DI container
services.AddMockForge("en", seed: 12345);

var serviceProvider = services.BuildServiceProvider();
var mockForge = serviceProvider.GetRequiredService<IMockForge>();

// Use the injected instance
var name = await mockForge.Name.FirstNameAsync();
```

### Advanced DI Configuration

```csharp
services.AddMockForge(options =>
{
    options.Locale = "es";
    options.Seed = 98765;
});
```

## Available Providers

### Name Provider

```csharp
// Basic names
var firstName = await MockForge.Name.FirstNameAsync();
var lastName = await MockForge.Name.LastNameAsync();
var fullName = await MockForge.Name.FullNameAsync();

// Extended names
var fullNameWithMiddle = await MockForge.Name.FullNameWithMiddleAsync();
var formalName = await MockForge.Name.FormalNameAsync(); // May include prefixes/suffixes
```

### Address Provider

```csharp
// Address components
var street = await MockForge.Address.StreetNameAsync();
var streetAddress = await MockForge.Address.StreetAddressAsync(); // With building number
var city = await MockForge.Address.CityAsync();
var state = await MockForge.Address.StateAsync();
var country = await MockForge.Address.CountryAsync();
var postalCode = await MockForge.Address.PostalCodeAsync();

// Complete address
var fullAddress = await MockForge.Address.FullAddressAsync();
```

### Internet Provider

```csharp
// Internet data
var email = await MockForge.Internet.EmailAsync();
var username = await MockForge.Internet.UsernameAsync();
var domain = await MockForge.Internet.DomainNameAsync();
var url = await MockForge.Internet.UrlAsync();
var ipAddress = MockForge.Internet.IpAddress();
var macAddress = MockForge.Internet.MacAddress();
```

### Number Provider

```csharp
// Basic numbers
var integer = MockForge.Number.Integer(1, 100);
var decimal = MockForge.Number.Decimal(0, 1000, decimals: 2);
var double = MockForge.Number.Double(0.0, 100.0);
var percentage = MockForge.Number.Percentage();

// Formatted numbers
var phoneNumber = await MockForge.Number.PhoneNumberAsync();
var creditCard = await MockForge.Number.CreditCardNumberAsync();
var ssn = await MockForge.Number.SocialSecurityNumberAsync();
```

### Lorem Provider

```csharp
// Text generation
var word = await MockForge.Lorem.WordAsync();
var words = await MockForge.Lorem.WordsAsync(5);
var sentence = await MockForge.Lorem.SentenceAsync();
var sentences = await MockForge.Lorem.SentencesAsync(3);
var paragraph = await MockForge.Lorem.ParagraphAsync();
var paragraphs = await MockForge.Lorem.ParagraphsAsync(2);
var text = await MockForge.Lorem.TextAsync(200); // Approximate length
```

## Supported Locales

- **English (`en`)**: Default locale with comprehensive datasets
- **Spanish (`es`)**: Full Spanish locale support

When a locale doesn't have specific data, it automatically falls back to the default English locale.

## Deterministic Generation

Use seeds for reproducible results, perfect for testing:

```csharp
MockForge.Configure("en", seed: 12345);

// These will always produce the same results
var name1 = await MockForge.Name.FullNameAsync();
var name2 = await MockForge.Name.FullNameAsync();
```

## Creating Custom Providers

Extend MockForge with custom providers:

```csharp
public interface ICustomProvider : IDataProvider
{
    Task<string> CustomDataAsync();
}

public class CustomProvider : BaseDataProvider, ICustomProvider
{
    public CustomProvider(IRandomGenerator random, ILocaleDataProvider localeDataProvider, 
                         ITemplateEngine templateEngine, string locale = "en")
        : base(random, localeDataProvider, templateEngine, locale) { }

    public async Task<string> CustomDataAsync()
    {
        // Your implementation here
        return "Custom data";
    }
}
```

## Template System

Use the built-in template engine for dynamic content:

```csharp
var template = "Hello {{name}}, you have {{count}} messages";
var tokens = new Dictionary<string, string>
{
    { "name", await MockForge.Name.FirstNameAsync() },
    { "count", MockForge.Number.Integer(1, 100).ToString() }
};

var engine = new TemplateEngine();
var result = engine.Process(template, tokens);
```

## Performance

MockForge is optimized for performance:
- Embedded JSON datasets for fast loading
- Singleton pattern for DI scenarios
- Efficient caching and lazy loading
- Thread-safe random generation

Run benchmarks:

```bash
dotnet run --project benchmarks/MockForge.Benchmarks --configuration Release
```

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package MockForge
```

## Requirements

- .NET 8.0 or later
- No external dependencies (except Microsoft.Extensions.DependencyInjection for DI features)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.