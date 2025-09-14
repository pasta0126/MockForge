using MockForge.Core;
using MockForge.Data;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Interface for number generation.
/// </summary>
public interface INumberProvider : IDataProvider
{
    /// <summary>
    /// Generates a random integer between min and max (inclusive).
    /// </summary>
    int Integer(int min = 0, int max = 100);

    /// <summary>
    /// Generates a random decimal number.
    /// </summary>
    decimal Decimal(decimal min = 0, decimal max = 100, int decimals = 2);

    /// <summary>
    /// Generates a random double.
    /// </summary>
    double Double(double min = 0, double max = 100);

    /// <summary>
    /// Generates a phone number.
    /// </summary>
    Task<string> PhoneNumberAsync();

    /// <summary>
    /// Generates a credit card number.
    /// </summary>
    Task<string> CreditCardNumberAsync();

    /// <summary>
    /// Generates a social security number (format varies by locale).
    /// </summary>
    Task<string> SocialSecurityNumberAsync();

    /// <summary>
    /// Generates a percentage (0-100).
    /// </summary>
    int Percentage();
}

/// <summary>
/// Provider for generating various types of numbers.
/// </summary>
public class NumberProvider : BaseDataProvider, INumberProvider
{
    private NumberData? _numberData;

    public NumberProvider(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
        : base(random, localeDataProvider, templateEngine, locale)
    {
    }

    /// <summary>
    /// Generates a random integer between min and max (inclusive).
    /// </summary>
    public int Integer(int min = 0, int max = 100)
    {
        return Random.Next(min, max + 1);
    }

    /// <summary>
    /// Generates a random decimal number.
    /// </summary>
    public decimal Decimal(decimal min = 0, decimal max = 100, int decimals = 2)
    {
        var range = max - min;
        var randomValue = (decimal)Random.NextDouble() * range + min;
        return Math.Round(randomValue, decimals);
    }

    /// <summary>
    /// Generates a random double.
    /// </summary>
    public double Double(double min = 0, double max = 100)
    {
        var range = max - min;
        return Random.NextDouble() * range + min;
    }

    /// <summary>
    /// Generates a phone number.
    /// </summary>
    public async Task<string> PhoneNumberAsync()
    {
        var numberData = await GetNumberDataAsync();
        var format = numberData.PhoneFormats.Length > 0
            ? GetRandomElement(numberData.PhoneFormats)
            : "###-###-####";
        
        return GenerateFromPattern(format);
    }

    /// <summary>
    /// Generates a credit card number.
    /// </summary>
    public async Task<string> CreditCardNumberAsync()
    {
        var numberData = await GetNumberDataAsync();
        var format = numberData.CreditCardFormats.Length > 0
            ? GetRandomElement(numberData.CreditCardFormats)
            : "####-####-####-####";
        
        return GenerateFromPattern(format);
    }

    /// <summary>
    /// Generates a social security number (format varies by locale).
    /// </summary>
    public async Task<string> SocialSecurityNumberAsync()
    {
        var numberData = await GetNumberDataAsync();
        var format = numberData.SocialSecurityFormats.Length > 0
            ? GetRandomElement(numberData.SocialSecurityFormats)
            : "###-##-####";
        
        return GenerateFromPattern(format);
    }

    /// <summary>
    /// Generates a percentage (0-100).
    /// </summary>
    public int Percentage()
    {
        return Random.Next(0, 101);
    }

    private async Task<NumberData> GetNumberDataAsync()
    {
        _numberData ??= await LocaleDataProvider.GetDataAsync<NumberData>(Locale, "numbers")
            ?? new NumberData(); // Fallback to empty data with default patterns

        return _numberData;
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