namespace MockForge.Data;

/// <summary>
/// Data model for name-related information.
/// </summary>
public class NameData
{
    public string[] FirstNames { get; set; } = Array.Empty<string>();
    public string[] LastNames { get; set; } = Array.Empty<string>();
    public string[] MiddleNames { get; set; } = Array.Empty<string>();
    public string[] Prefixes { get; set; } = Array.Empty<string>();
    public string[] Suffixes { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Data model for address-related information.
/// </summary>
public class AddressData
{
    public string[] Streets { get; set; } = Array.Empty<string>();
    public string[] Cities { get; set; } = Array.Empty<string>();
    public string[] States { get; set; } = Array.Empty<string>();
    public string[] Countries { get; set; } = Array.Empty<string>();
    public string[] PostalCodeFormats { get; set; } = Array.Empty<string>();
    public string[] BuildingNumbers { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Data model for internet-related information.
/// </summary>
public class InternetData
{
    public string[] Domains { get; set; } = Array.Empty<string>();
    public string[] EmailProviders { get; set; } = Array.Empty<string>();
    public string[] UsernameFormats { get; set; } = Array.Empty<string>();
    public string[] Protocols { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Data model for lorem ipsum text.
/// </summary>
public class LoremData
{
    public string[] Words { get; set; } = Array.Empty<string>();
    public string[] Sentences { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Data model for number formatting patterns.
/// </summary>
public class NumberData
{
    public string[] PhoneFormats { get; set; } = Array.Empty<string>();
    public string[] CreditCardFormats { get; set; } = Array.Empty<string>();
    public string[] SocialSecurityFormats { get; set; } = Array.Empty<string>();
}