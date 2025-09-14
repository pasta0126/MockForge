using MockForge.Providers;

namespace MockForge;

/// <summary>
/// Static facade for MockForge library providing easy access to all providers.
/// </summary>
public static class MockForge
{
    private static IMockForge? _instance;
    private static readonly object Lock = new();

    /// <summary>
    /// Gets the current MockForge instance. Creates a default instance if none exists.
    /// </summary>
    private static IMockForge Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    _instance ??= new MockForgeCore();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Gets the Name provider for generating names.
    /// </summary>
    public static INameProvider Name => Instance.Name;

    /// <summary>
    /// Gets the Address provider for generating addresses.
    /// </summary>
    public static IAddressProvider Address => Instance.Address;

    /// <summary>
    /// Gets the Internet provider for generating internet-related data.
    /// </summary>
    public static IInternetProvider Internet => Instance.Internet;

    /// <summary>
    /// Gets the Number provider for generating numbers.
    /// </summary>
    public static INumberProvider Number => Instance.Number;

    /// <summary>
    /// Gets the Lorem provider for generating lorem ipsum text.
    /// </summary>
    public static ILoremProvider Lorem => Instance.Lorem;

    /// <summary>
    /// Gets the locale used by the current instance.
    /// </summary>
    public static string Locale => Instance.Locale;

    /// <summary>
    /// Gets the seed used by the current instance.
    /// </summary>
    public static int Seed => Instance.Seed;

    /// <summary>
    /// Configures MockForge with a specific locale and seed.
    /// </summary>
    /// <param name="locale">The locale to use (default: "en").</param>
    /// <param name="seed">Optional seed for deterministic random generation.</param>
    public static void Configure(string locale = "en", int? seed = null)
    {
        lock (Lock)
        {
            _instance = new MockForgeCore(locale, seed);
        }
    }

    /// <summary>
    /// Sets a custom MockForge instance.
    /// </summary>
    /// <param name="mockForgeInstance">The MockForge instance to use.</param>
    public static void SetInstance(IMockForge mockForgeInstance)
    {
        lock (Lock)
        {
            _instance = mockForgeInstance ?? throw new ArgumentNullException(nameof(mockForgeInstance));
        }
    }

    /// <summary>
    /// Resets to the default configuration.
    /// </summary>
    public static void Reset()
    {
        lock (Lock)
        {
            _instance = null;
        }
    }
}