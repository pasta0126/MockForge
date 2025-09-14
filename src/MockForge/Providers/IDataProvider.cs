using MockForge.Core;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Base interface for all data providers.
/// </summary>
public interface IDataProvider
{
    /// <summary>
    /// Gets the locale used by this provider.
    /// </summary>
    string Locale { get; }
}