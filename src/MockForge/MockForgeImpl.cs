using MockForge.Core.Abstractions;
using MockForge.Core.Random;

namespace MockForge;

internal sealed class MockForgeImpl(MockForge.Core.MockForgeOptions opts) : IMockForge
{
    private readonly IRandomizer _rnd = new ThreadSafeRandomizer(opts.Seed);
    private readonly Dictionary<Type, IProvider> _cache = [];

    public string Locale => "static"; // locale removed

    public T Get<T>() where T : IProvider
    {
        if (_cache.TryGetValue(typeof(T), out var p)) return (T)p;
        var created = (T)Activator.CreateInstance(typeof(T), _rnd)!;
        _cache[typeof(T)] = created;
        return created;
    }
}
