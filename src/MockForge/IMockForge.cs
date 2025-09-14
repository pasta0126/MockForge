using MockForge.Providers;

namespace MockForge;

/// <summary>
/// Main interface for the MockForge library, suitable for dependency injection.
/// </summary>
public interface IMockForge
{
    /// <summary>
    /// Gets the Name provider for generating names.
    /// </summary>
    INameProvider Name { get; }

    /// <summary>
    /// Gets the Address provider for generating addresses.
    /// </summary>
    IAddressProvider Address { get; }

    /// <summary>
    /// Gets the Internet provider for generating internet-related data.
    /// </summary>
    IInternetProvider Internet { get; }

    /// <summary>
    /// Gets the Number provider for generating numbers.
    /// </summary>
    INumberProvider Number { get; }

    /// <summary>
    /// Gets the Lorem provider for generating lorem ipsum text.
    /// </summary>
    ILoremProvider Lorem { get; }

    /// <summary>
    /// Gets the locale used by this MockForge instance.
    /// </summary>
    string Locale { get; }

    /// <summary>
    /// Gets the seed used by the random generator.
    /// </summary>
    int Seed { get; }
}