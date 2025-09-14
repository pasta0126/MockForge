using MockForge.Core;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Abstract base class for all data providers.
/// </summary>
public abstract class BaseDataProvider : IDataProvider
{
    /// <summary>
    /// The random generator used by this provider.
    /// </summary>
    protected readonly IRandomGenerator Random;

    /// <summary>
    /// The locale data provider for accessing localized data.
    /// </summary>
    protected readonly ILocaleDataProvider LocaleDataProvider;

    /// <summary>
    /// The template engine for processing template strings.
    /// </summary>
    protected readonly ITemplateEngine TemplateEngine;

    /// <summary>
    /// The locale used by this provider.
    /// </summary>
    public string Locale { get; }

    /// <summary>
    /// Initializes a new instance of the BaseDataProvider class.
    /// </summary>
    /// <param name="random">The random generator to use.</param>
    /// <param name="localeDataProvider">The locale data provider.</param>
    /// <param name="templateEngine">The template engine.</param>
    /// <param name="locale">The locale for this provider.</param>
    protected BaseDataProvider(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
    {
        Random = random ?? throw new ArgumentNullException(nameof(random));
        LocaleDataProvider = localeDataProvider ?? throw new ArgumentNullException(nameof(localeDataProvider));
        TemplateEngine = templateEngine ?? throw new ArgumentNullException(nameof(templateEngine));
        Locale = locale ?? throw new ArgumentNullException(nameof(locale));
    }

    /// <summary>
    /// Gets a random element from the provided array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="items">The array to select from.</param>
    /// <returns>A random element from the array.</returns>
    protected T GetRandomElement<T>(T[] items)
    {
        if (items == null || items.Length == 0)
            throw new ArgumentException("Array cannot be null or empty", nameof(items));

        return items[Random.Next(items.Length)];
    }
}