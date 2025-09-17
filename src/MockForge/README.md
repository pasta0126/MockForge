# MockForge

A lightweight .NET 8 library to generate fake data with localization support, deterministic seeds, and DI-friendly APIs.

## Features
- Locale-aware data via embedded JSON resources (e.g., `en`, `es`) with fallback (requested ? base language ? `en`).
- Deterministic output by optional seed (great for tests and reproducibility).
- Simple, composable providers (e.g., `NameProvider`).
- Works with DI or standalone factory.

## Requirements
- .NET 8 SDK

## Getting Started

### Install
- Add a project reference to the libraries under `src/`, or use the NuGet package if/when published.

### Usage with DI
```csharp
using Microsoft.Extensions.DependencyInjection;
using MockForge;
using MockForge.Core;
using MockForge.Core.Abstractions;
using MockForge.Providers.Name;

var services = new ServiceCollection()
    .AddMockForge(o =>
    {
        o.Locale = "es";   // or "en", "es-MX", etc.
        o.Seed = 123;       // optional for deterministic output
    })
    .BuildServiceProvider();

var forge = services.GetRequiredService<IMockForge>();
var fullName = await forge.Get<NameProvider>().FullAsync();
```

### Usage without DI
```csharp
using MockForge;
using MockForge.Core;
using MockForge.Providers.Name;

var forge = MockForgeFactory.Create(new MockForgeOptions
{
    Locale = "en",
    Seed = 42
});

var fullName = await forge.Get<NameProvider>().FullAsync();
```

## Project Structure
- `MockForge.Core`: abstractions and core utilities (`IMockForge`, `IRandomizer`, `ILocaleStore`, `ITemplateEngine`, `MockForgeOptions`).
- `MockForge`: service registration and facade (`AddMockForge`, `MockForgeFactory`).
- `MockForge.Providers`: concrete providers (e.g., `NameProvider`).
- `MockForge.Locales`: embedded locale data (`Locales/en.json`, `Locales/es.json`).
- `MockForge.Tests`: xUnit tests with FluentAssertions.

## Running Tests
```bash
dotnet test
```

## Extending
- Add new providers under `MockForge.Providers` that implement `IProvider` and accept `(IRandomizer, ILocaleStore, string locale)` in the constructor.
- Add locale keys to `MockForge.Locales` JSON files and consume them via `ILocaleStore`.