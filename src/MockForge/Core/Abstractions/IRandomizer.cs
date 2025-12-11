namespace MockForge.Core.Abstractions;
public interface IRandomizer
{
    int Next(int min, int max);
    double NextDouble();
    T Pick<T>(IReadOnlyList<T> items);
    T[] Shuffle<T>(T[]? array);
}
