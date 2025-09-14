using MockForge.Core;
using MockForge.Data;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Interface for address generation.
/// </summary>
public interface IAddressProvider : IDataProvider
{
    /// <summary>
    /// Generates a street name.
    /// </summary>
    Task<string> StreetNameAsync();

    /// <summary>
    /// Generates a street address (with building number).
    /// </summary>
    Task<string> StreetAddressAsync();

    /// <summary>
    /// Generates a city name.
    /// </summary>
    Task<string> CityAsync();

    /// <summary>
    /// Generates a state or province name.
    /// </summary>
    Task<string> StateAsync();

    /// <summary>
    /// Generates a country name.
    /// </summary>
    Task<string> CountryAsync();

    /// <summary>
    /// Generates a postal code.
    /// </summary>
    Task<string> PostalCodeAsync();

    /// <summary>
    /// Generates a full address.
    /// </summary>
    Task<string> FullAddressAsync();
}

/// <summary>
/// Provider for generating addresses.
/// </summary>
public class AddressProvider : BaseDataProvider, IAddressProvider
{
    private AddressData? _addressData;

    public AddressProvider(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
        : base(random, localeDataProvider, templateEngine, locale)
    {
    }

    /// <summary>
    /// Generates a street name.
    /// </summary>
    public async Task<string> StreetNameAsync()
    {
        var addressData = await GetAddressDataAsync();
        return GetRandomElement(addressData.Streets);
    }

    /// <summary>
    /// Generates a street address (with building number).
    /// </summary>
    public async Task<string> StreetAddressAsync()
    {
        var addressData = await GetAddressDataAsync();
        var buildingNumber = addressData.BuildingNumbers.Length > 0
            ? GetRandomElement(addressData.BuildingNumbers)
            : Random.Next(1, 9999).ToString();
        var streetName = GetRandomElement(addressData.Streets);
        return $"{buildingNumber} {streetName}";
    }

    /// <summary>
    /// Generates a city name.
    /// </summary>
    public async Task<string> CityAsync()
    {
        var addressData = await GetAddressDataAsync();
        return GetRandomElement(addressData.Cities);
    }

    /// <summary>
    /// Generates a state or province name.
    /// </summary>
    public async Task<string> StateAsync()
    {
        var addressData = await GetAddressDataAsync();
        return GetRandomElement(addressData.States);
    }

    /// <summary>
    /// Generates a country name.
    /// </summary>
    public async Task<string> CountryAsync()
    {
        var addressData = await GetAddressDataAsync();
        return GetRandomElement(addressData.Countries);
    }

    /// <summary>
    /// Generates a postal code.
    /// </summary>
    public async Task<string> PostalCodeAsync()
    {
        var addressData = await GetAddressDataAsync();
        var format = addressData.PostalCodeFormats.Length > 0
            ? GetRandomElement(addressData.PostalCodeFormats)
            : "#####";
        
        return GenerateFromPattern(format);
    }

    /// <summary>
    /// Generates a full address.
    /// </summary>
    public async Task<string> FullAddressAsync()
    {
        var streetAddress = await StreetAddressAsync();
        var city = await CityAsync();
        var state = await StateAsync();
        var postalCode = await PostalCodeAsync();
        
        return $"{streetAddress}, {city}, {state} {postalCode}";
    }

    private async Task<AddressData> GetAddressDataAsync()
    {
        _addressData ??= await LocaleDataProvider.GetDataAsync<AddressData>(Locale, "addresses")
            ?? throw new InvalidOperationException($"Unable to load address data for locale '{Locale}'");

        return _addressData;
    }

    private string GenerateFromPattern(string pattern)
    {
        var result = "";
        foreach (char c in pattern)
        {
            result += c switch
            {
                '#' => Random.Next(0, 10).ToString(),
                'A' => ((char)Random.Next(65, 91)).ToString(), // A-Z
                'a' => ((char)Random.Next(97, 123)).ToString(), // a-z
                _ => c.ToString()
            };
        }
        return result;
    }
}