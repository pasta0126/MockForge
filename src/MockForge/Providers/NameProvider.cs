using MockForge.Core;
using MockForge.Data;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Interface for name generation.
/// </summary>
public interface INameProvider : IDataProvider
{
    /// <summary>
    /// Generates a first name.
    /// </summary>
    Task<string> FirstNameAsync();

    /// <summary>
    /// Generates a last name.
    /// </summary>
    Task<string> LastNameAsync();

    /// <summary>
    /// Generates a full name (first and last).
    /// </summary>
    Task<string> FullNameAsync();

    /// <summary>
    /// Generates a full name with middle name.
    /// </summary>
    Task<string> FullNameWithMiddleAsync();

    /// <summary>
    /// Generates a name with prefix and suffix.
    /// </summary>
    Task<string> FormalNameAsync();
}

/// <summary>
/// Provider for generating names.
/// </summary>
public class NameProvider : BaseDataProvider, INameProvider
{
    private NameData? _nameData;

    public NameProvider(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
        : base(random, localeDataProvider, templateEngine, locale)
    {
    }

    /// <summary>
    /// Generates a first name.
    /// </summary>
    public async Task<string> FirstNameAsync()
    {
        var nameData = await GetNameDataAsync();
        return GetRandomElement(nameData.FirstNames);
    }

    /// <summary>
    /// Generates a last name.
    /// </summary>
    public async Task<string> LastNameAsync()
    {
        var nameData = await GetNameDataAsync();
        return GetRandomElement(nameData.LastNames);
    }

    /// <summary>
    /// Generates a full name (first and last).
    /// </summary>
    public async Task<string> FullNameAsync()
    {
        var firstName = await FirstNameAsync();
        var lastName = await LastNameAsync();
        return $"{firstName} {lastName}";
    }

    /// <summary>
    /// Generates a full name with middle name.
    /// </summary>
    public async Task<string> FullNameWithMiddleAsync()
    {
        var nameData = await GetNameDataAsync();
        var firstName = GetRandomElement(nameData.FirstNames);
        var middleName = nameData.MiddleNames.Length > 0 
            ? GetRandomElement(nameData.MiddleNames) 
            : GetRandomElement(nameData.FirstNames);
        var lastName = GetRandomElement(nameData.LastNames);
        return $"{firstName} {middleName} {lastName}";
    }

    /// <summary>
    /// Generates a name with prefix and suffix.
    /// </summary>
    public async Task<string> FormalNameAsync()
    {
        var nameData = await GetNameDataAsync();
        var fullName = await FullNameAsync();
        
        var prefix = nameData.Prefixes.Length > 0 && Random.Next(2) == 0
            ? GetRandomElement(nameData.Prefixes) + " "
            : "";
        
        var suffix = nameData.Suffixes.Length > 0 && Random.Next(3) == 0
            ? " " + GetRandomElement(nameData.Suffixes)
            : "";

        return $"{prefix}{fullName}{suffix}";
    }

    private async Task<NameData> GetNameDataAsync()
    {
        _nameData ??= await LocaleDataProvider.GetDataAsync<NameData>(Locale, "names")
            ?? throw new InvalidOperationException($"Unable to load name data for locale '{Locale}'");

        return _nameData;
    }
}