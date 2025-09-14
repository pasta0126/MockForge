using MockForge.Core;
using MockForge.Data;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Interface for internet-related data generation.
/// </summary>
public interface IInternetProvider : IDataProvider
{
    /// <summary>
    /// Generates an email address.
    /// </summary>
    Task<string> EmailAsync();

    /// <summary>
    /// Generates a username.
    /// </summary>
    Task<string> UsernameAsync();

    /// <summary>
    /// Generates a domain name.
    /// </summary>
    Task<string> DomainNameAsync();

    /// <summary>
    /// Generates a URL.
    /// </summary>
    Task<string> UrlAsync();

    /// <summary>
    /// Generates an IP address.
    /// </summary>
    string IpAddress();

    /// <summary>
    /// Generates a MAC address.
    /// </summary>
    string MacAddress();
}

/// <summary>
/// Provider for generating internet-related data.
/// </summary>
public class InternetProvider : BaseDataProvider, IInternetProvider
{
    private InternetData? _internetData;

    public InternetProvider(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
        : base(random, localeDataProvider, templateEngine, locale)
    {
    }

    /// <summary>
    /// Generates an email address.
    /// </summary>
    public async Task<string> EmailAsync()
    {
        var internetData = await GetInternetDataAsync();
        var username = await UsernameAsync();
        var domain = internetData.EmailProviders.Length > 0
            ? GetRandomElement(internetData.EmailProviders)
            : GetRandomElement(internetData.Domains);
        
        return $"{username}@{domain}";
    }

    /// <summary>
    /// Generates a username.
    /// </summary>
    public async Task<string> UsernameAsync()
    {
        var internetData = await GetInternetDataAsync();
        if (internetData.UsernameFormats.Length > 0)
        {
            var format = GetRandomElement(internetData.UsernameFormats);
            var tokens = new Dictionary<string, string>
            {
                ["word"] = GenerateWord(),
                ["number"] = Random.Next(10, 999).ToString(),
                ["year"] = Random.Next(1980, DateTime.Now.Year + 1).ToString()
            };
            return TemplateEngine.Process(format, tokens);
        }

        return GenerateWord() + Random.Next(10, 999);
    }

    /// <summary>
    /// Generates a domain name.
    /// </summary>
    public async Task<string> DomainNameAsync()
    {
        var internetData = await GetInternetDataAsync();
        return GetRandomElement(internetData.Domains);
    }

    /// <summary>
    /// Generates a URL.
    /// </summary>
    public async Task<string> UrlAsync()
    {
        var internetData = await GetInternetDataAsync();
        var protocol = internetData.Protocols.Length > 0
            ? GetRandomElement(internetData.Protocols)
            : "https";
        var domain = await DomainNameAsync();
        var path = Random.Next(2) == 0 ? "/" + GenerateWord() : "";
        
        return $"{protocol}://{domain}{path}";
    }

    /// <summary>
    /// Generates an IP address.
    /// </summary>
    public string IpAddress()
    {
        return $"{Random.Next(1, 255)}.{Random.Next(0, 255)}.{Random.Next(0, 255)}.{Random.Next(1, 255)}";
    }

    /// <summary>
    /// Generates a MAC address.
    /// </summary>
    public string MacAddress()
    {
        var bytes = new byte[6];
        Random.NextBytes(bytes);
        return string.Join(":", bytes.Select(b => b.ToString("X2")));
    }

    private async Task<InternetData> GetInternetDataAsync()
    {
        _internetData ??= await LocaleDataProvider.GetDataAsync<InternetData>(Locale, "internet")
            ?? throw new InvalidOperationException($"Unable to load internet data for locale '{Locale}'");

        return _internetData;
    }

    private string GenerateWord()
    {
        var consonants = "bcdfghjklmnpqrstvwxyz";
        var vowels = "aeiou";
        var length = Random.Next(4, 9);
        var word = "";

        for (int i = 0; i < length; i++)
        {
            if (i % 2 == 0)
                word += consonants[Random.Next(consonants.Length)];
            else
                word += vowels[Random.Next(vowels.Length)];
        }

        return word;
    }
}