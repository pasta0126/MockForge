using MockForge.Core.Abstractions;
using MockForge.Core.Random;
using System.Linq;

namespace MockForge;

internal sealed class MockForgeImpl(MockForge.Core.MockForgeOptions opts) : IMockForge
{
    private readonly IRandomizer _rnd = new ThreadSafeRandomizer(opts.Seed);
    private readonly Dictionary<Type, IProvider> _cache = [];

    public string Locale => "static"; 

    public T Get<T>() where T : IProvider
    {
        return (T)ResolveProvider(typeof(T));
    }

    private IProvider ResolveProvider(Type type)
    {
        if (_cache.TryGetValue(type, out var existing)) return existing;

        var ctors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length);
        foreach (var ctor in ctors)
        {
            var parameters = ctor.GetParameters();
            var args = new object?[parameters.Length];
            var canSatisfy = true;

            for (int i = 0; i < parameters.Length; i++)
            {
                var pt = parameters[i].ParameterType;
                if (pt == typeof(IRandomizer))
                {
                    args[i] = _rnd;
                    continue;
                }
                if (typeof(IProvider).IsAssignableFrom(pt))
                {
                    args[i] = ResolveProvider(pt);
                    continue;
                }

                canSatisfy = false;
                break;
            }

            if (!canSatisfy) continue;

            var created = (IProvider)ctor.Invoke(args);
            _cache[type] = created;
            return created;
        }

        var fallback = (IProvider)Activator.CreateInstance(type, _rnd)!;
        _cache[type] = fallback;
        return fallback;
    }
}
