using BenchmarkDotNet.Attributes;
using MockForge.Core.Random;
public class PickBench
{
    readonly ThreadSafeRandomizer _rnd = new(42);
    readonly List<int> _data = Enumerable.Range(0, 10_000).ToList();
    [Benchmark] public int Pick() => _rnd.Pick(_data);
}
