using System.Reflection;
using System.Text.Json;

namespace MockForge.Locales;

/// <summary>
/// Embedded JSON-based locale data provider with fallback support.
/// </summary>
public class EmbeddedLocaleDataProvider : ILocaleDataProvider
{
    private const string DefaultLocale = "en";
    private const string DataResourcePrefix = "MockForge.Data.";
    private static readonly string[] SupportedLocales = { "en", "es" };
    private static readonly Dictionary<string, object> Cache = new();
    private static readonly SemaphoreSlim CacheLock = new(1, 1);

    /// <summary>
    /// Gets data for the specified locale and category with fallback support.
    /// </summary>
    public async Task<T?> GetDataAsync<T>(string locale, string category) where T : class
    {
        var cacheKey = $"{locale}:{category}:{typeof(T).Name}";
        
        await CacheLock.WaitAsync();
        try
        {
            if (Cache.TryGetValue(cacheKey, out var cached))
                return cached as T;

            var data = await LoadDataAsync<T>(locale, category);
            if (data != null)
            {
                Cache[cacheKey] = data;
                return data;
            }

            // Fallback to default locale if requested locale fails
            if (locale != DefaultLocale)
            {
                var fallbackCacheKey = $"{DefaultLocale}:{category}:{typeof(T).Name}";
                if (Cache.TryGetValue(fallbackCacheKey, out var fallbackCached))
                    return fallbackCached as T;

                var fallbackData = await LoadDataAsync<T>(DefaultLocale, category);
                if (fallbackData != null)
                {
                    Cache[fallbackCacheKey] = fallbackData;
                    return fallbackData;
                }
            }

            return null;
        }
        finally
        {
            CacheLock.Release();
        }
    }

    /// <summary>
    /// Checks if a locale is supported.
    /// </summary>
    public bool IsLocaleSupported(string locale) => SupportedLocales.Contains(locale);

    /// <summary>
    /// Gets the list of supported locales.
    /// </summary>
    public string[] GetSupportedLocales() => SupportedLocales.ToArray();

    private async Task<T?> LoadDataAsync<T>(string locale, string category) where T : class
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{DataResourcePrefix}{locale}.{category}.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            return null;

        var json = await new StreamReader(stream).ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}