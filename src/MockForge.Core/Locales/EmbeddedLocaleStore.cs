using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using MockForge.Core.Abstractions;

namespace MockForge.Core.Locales;

public sealed class EmbeddedLocaleStore : ILocaleStore
{
    readonly Assembly _asm;
    readonly HashSet<string> _resourceNames;
    readonly ConcurrentDictionary<string, IReadOnlyDictionary<string, List<string>>> _cache = new();
    static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public EmbeddedLocaleStore(Assembly? dataAssembly = null)
    {
        _asm = dataAssembly ?? Assembly.Load("MockForge.Locales");
        _resourceNames = _asm.GetManifestResourceNames().ToHashSet(StringComparer.Ordinal);
    }

    public async ValueTask<IReadOnlyList<string>> GetListAsync(string locale, string key, CancellationToken ct = default)
    {
        var dict = await GetLocaleDictAsync(locale, ct);
        return dict.TryGetValue(key, out var list) ? list : Array.Empty<string>();
    }

    async ValueTask<IReadOnlyDictionary<string, List<string>>> GetLocaleDictAsync(string locale, CancellationToken ct)
    {
        if (_cache.TryGetValue(locale, out var cached)) return cached;

        var chain = BuildFallbackChain(locale);
        foreach (var loc in chain)
        {
            var loaded = await LoadLocaleAsync(loc, ct);
            if (loaded is not null) return _cache.GetOrAdd(locale, loaded);
        }
        return _cache.GetOrAdd(locale, new Dictionary<string, List<string>>(StringComparer.Ordinal));
    }

    static IEnumerable<string> BuildFallbackChain(string locale)
    {
        yield return locale;
        var dash = locale.IndexOf('-');
        if (dash > 0) yield return locale[..dash];
        yield return "en";
    }

    async Task<IReadOnlyDictionary<string, List<string>>?> LoadLocaleAsync(string loc, CancellationToken ct)
    {
        var logicalName = $"MockForge.Locales.{loc}.json";
        if (!_resourceNames.Contains(logicalName)) return null;

        await using var s = _asm.GetManifestResourceStream(logicalName)!;
        var dict = await JsonSerializer.DeserializeAsync<Dictionary<string, List<string>>>(s, _json, ct);
        return dict is null
            ? new Dictionary<string, List<string>>(StringComparer.Ordinal)
            : new Dictionary<string, List<string>>(dict, StringComparer.Ordinal);
    }
}
