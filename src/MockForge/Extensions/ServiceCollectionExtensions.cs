using Microsoft.Extensions.DependencyInjection;
using MockForge.Core;
using MockForge.Locales;
using MockForge.Providers;
using MockForge.Templates;

namespace MockForge.Extensions;

/// <summary>
/// Extension methods for registering MockForge services with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds MockForge services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="locale">The locale to use (default: "en").</param>
    /// <param name="seed">Optional seed for deterministic random generation.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMockForge(this IServiceCollection services, string locale = "en", int? seed = null)
    {
        // Register core services
        services.AddSingleton<IRandomGenerator>(provider => 
            seed.HasValue ? new RandomGenerator(seed.Value) : new RandomGenerator());
        services.AddSingleton<ILocaleDataProvider, EmbeddedLocaleDataProvider>();
        services.AddSingleton<ITemplateEngine, TemplateEngine>();

        // Register providers
        services.AddSingleton<INameProvider>(provider => new NameProvider(
            provider.GetRequiredService<IRandomGenerator>(),
            provider.GetRequiredService<ILocaleDataProvider>(),
            provider.GetRequiredService<ITemplateEngine>(),
            locale));

        services.AddSingleton<IAddressProvider>(provider => new AddressProvider(
            provider.GetRequiredService<IRandomGenerator>(),
            provider.GetRequiredService<ILocaleDataProvider>(),
            provider.GetRequiredService<ITemplateEngine>(),
            locale));

        services.AddSingleton<IInternetProvider>(provider => new InternetProvider(
            provider.GetRequiredService<IRandomGenerator>(),
            provider.GetRequiredService<ILocaleDataProvider>(),
            provider.GetRequiredService<ITemplateEngine>(),
            locale));

        services.AddSingleton<INumberProvider>(provider => new NumberProvider(
            provider.GetRequiredService<IRandomGenerator>(),
            provider.GetRequiredService<ILocaleDataProvider>(),
            provider.GetRequiredService<ITemplateEngine>(),
            locale));

        services.AddSingleton<ILoremProvider>(provider => new LoremProvider(
            provider.GetRequiredService<IRandomGenerator>(),
            provider.GetRequiredService<ILocaleDataProvider>(),
            provider.GetRequiredService<ITemplateEngine>(),
            locale));

        // Register main MockForge instance
        services.AddSingleton<IMockForge>(provider => new MockForgeCore(
            provider.GetRequiredService<IRandomGenerator>(),
            provider.GetRequiredService<ILocaleDataProvider>(),
            provider.GetRequiredService<ITemplateEngine>(),
            locale));

        return services;
    }

    /// <summary>
    /// Adds MockForge services with custom configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureAction">Action to configure MockForge options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMockForge(this IServiceCollection services, Action<MockForgeOptions> configureAction)
    {
        var options = new MockForgeOptions();
        configureAction(options);

        return services.AddMockForge(options.Locale, options.Seed);
    }
}

/// <summary>
/// Configuration options for MockForge.
/// </summary>
public class MockForgeOptions
{
    /// <summary>
    /// Gets or sets the locale to use (default: "en").
    /// </summary>
    public string Locale { get; set; } = "en";

    /// <summary>
    /// Gets or sets the optional seed for deterministic random generation.
    /// </summary>
    public int? Seed { get; set; }
}