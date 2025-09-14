﻿using MockForge.Core.Abstractions;

namespace MockForge.Core.Random;
public sealed class ThreadSafeRandomizer(int? seed = null) : IRandomizer
{
    readonly ThreadLocal<System.Random> _rnd = new(() => seed.HasValue ? new System.Random(seed.Value) : new System.Random());

    public int Next(int min, int max) => _rnd.Value!.Next(min, max);
    public double NextDouble() => _rnd.Value!.NextDouble();
    public T Pick<T>(IReadOnlyList<T> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Empty list for random pick.", nameof(items));
        return _rnd.Value!.Next(items.Count) is var i ? items[i] : items[0];
    }
}
