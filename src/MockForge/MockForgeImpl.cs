using System.Reflection;
using MockForge.Core;
using MockForge.Core.Abstractions;
using MockForge.Core.Locales;
using MockForge.Core.Random;
using MockForge.Core.Templates;

namespace MockForge;

internal sealed class MockForgeImpl(MockForgeOptions opts) : IMockForge
{
    private readonly string _locale = opts.Locale;
    private readonly IRandomizer _rnd = new ThreadSafeRandomizer(opts.Seed);
    private readonly ITemplateEngine _tpl = new SimpleTemplateEngine();
    private readonly ILocaleStore _store = new EmbeddedLocaleStore(Assembly.Load("MockForge.Locales"));
    private readonly Dictionary<Type, IProvider> _cache = [];

    public string Locale => _locale;

    public T Get<T>() where T : IProvider
    {
        if (_cache.TryGetValue(typeof(T), out var p)) return (T)p;
        var created = (T)Activator.CreateInstance(typeof(T), _rnd, _store, _locale)!;
        _cache[typeof(T)] = created;
        return created;
    }
}
