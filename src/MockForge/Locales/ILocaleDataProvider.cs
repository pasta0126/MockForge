namespace MockForge.Locales;

/// <summary>
/// Interface for locale data loading and management.
/// </summary>
public interface ILocaleDataProvider
{
    /// <summary>
    /// Gets data for the specified locale and category.
    /// Falls back to default locale if the requested locale is not available.
    /// </summary>
    /// <param name="locale">The locale code (e.g., "en", "es").</param>
    /// <param name="category">The data category (e.g., "names", "addresses").</param>
    /// <returns>The data for the specified locale and category.</returns>
    Task<T?> GetDataAsync<T>(string locale, string category) where T : class;

    /// <summary>
    /// Checks if a locale is supported.
    /// </summary>
    /// <param name="locale">The locale code to check.</param>
    /// <returns>True if the locale is supported, false otherwise.</returns>
    bool IsLocaleSupported(string locale);

    /// <summary>
    /// Gets the list of supported locales.
    /// </summary>
    /// <returns>Array of supported locale codes.</returns>
    string[] GetSupportedLocales();
}