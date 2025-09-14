using MockForge.Core;
using MockForge.Locales;
using MockForge.Providers;
using MockForge.Templates;

namespace MockForge;

/// <summary>
/// Main MockForge implementation.
/// </summary>
public class MockForgeCore : IMockForge
{
    private readonly IRandomGenerator _random;
    private readonly ILocaleDataProvider _localeDataProvider;
    private readonly ITemplateEngine _templateEngine;

    /// <summary>
    /// Initializes a new instance of MockForge with the specified locale and seed.
    /// </summary>
    /// <param name="locale">The locale to use (default: "en").</param>
    /// <param name="seed">Optional seed for random generation.</param>
    public MockForgeCore(string locale = "en", int? seed = null)
    {
        Locale = locale;
        _random = seed.HasValue ? new RandomGenerator(seed.Value) : new RandomGenerator();
        _localeDataProvider = new EmbeddedLocaleDataProvider();
        _templateEngine = new TemplateEngine();

        InitializeProviders();
    }

    /// <summary>
    /// Initializes a new instance of MockForge with custom dependencies.
    /// </summary>
    /// <param name="random">The random generator to use.</param>
    /// <param name="localeDataProvider">The locale data provider.</param>
    /// <param name="templateEngine">The template engine.</param>
    /// <param name="locale">The locale to use.</param>
    public MockForgeCore(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
        _localeDataProvider = localeDataProvider ?? throw new ArgumentNullException(nameof(localeDataProvider));
        _templateEngine = templateEngine ?? throw new ArgumentNullException(nameof(templateEngine));
        Locale = locale ?? throw new ArgumentNullException(nameof(locale));

        InitializeProviders();
    }

    /// <summary>
    /// Gets the Name provider for generating names.
    /// </summary>
    public INameProvider Name { get; private set; } = null!;

    /// <summary>
    /// Gets the Address provider for generating addresses.
    /// </summary>
    public IAddressProvider Address { get; private set; } = null!;

    /// <summary>
    /// Gets the Internet provider for generating internet-related data.
    /// </summary>
    public IInternetProvider Internet { get; private set; } = null!;

    /// <summary>
    /// Gets the Number provider for generating numbers.
    /// </summary>
    public INumberProvider Number { get; private set; } = null!;

    /// <summary>
    /// Gets the Lorem provider for generating lorem ipsum text.
    /// </summary>
    public ILoremProvider Lorem { get; private set; } = null!;

    /// <summary>
    /// Gets the locale used by this MockForge instance.
    /// </summary>
    public string Locale { get; }

    /// <summary>
    /// Gets the seed used by the random generator.
    /// </summary>
    public int Seed => _random.Seed;

    private void InitializeProviders()
    {
        Name = new NameProvider(_random, _localeDataProvider, _templateEngine, Locale);
        Address = new AddressProvider(_random, _localeDataProvider, _templateEngine, Locale);
        Internet = new InternetProvider(_random, _localeDataProvider, _templateEngine, Locale);
        Number = new NumberProvider(_random, _localeDataProvider, _templateEngine, Locale);
        Lorem = new LoremProvider(_random, _localeDataProvider, _templateEngine, Locale);
    }
}