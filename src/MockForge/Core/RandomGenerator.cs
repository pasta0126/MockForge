namespace MockForge.Core;

/// <summary>
/// Thread-safe random number generator with seed support.
/// </summary>
public class RandomGenerator : IRandomGenerator
{
    private readonly Random _random;
    private readonly int _seed;
    private readonly object _lock = new();
    
    /// <summary>
    /// Initializes a new instance with a random seed.
    /// </summary>
    public RandomGenerator()
    {
        _seed = GenerateSeed();
        _random = new Random(_seed);
    }

    /// <summary>
    /// Initializes a new instance with the specified seed.
    /// </summary>
    /// <param name="seed">The seed for random number generation.</param>
    public RandomGenerator(int seed)
    {
        _seed = seed;
        _random = new Random(_seed);
    }

    /// <summary>
    /// Gets the seed used to initialize the random generator.
    /// </summary>
    public int Seed => _seed;

    /// <summary>
    /// Returns a random integer.
    /// </summary>
    public int Next()
    {
        lock (_lock)
        {
            return _random.Next();
        }
    }

    /// <summary>
    /// Returns a random integer between 0 (inclusive) and maxValue (exclusive).
    /// </summary>
    public int Next(int maxValue)
    {
        lock (_lock)
        {
            return _random.Next(maxValue);
        }
    }

    /// <summary>
    /// Returns a random integer between minValue (inclusive) and maxValue (exclusive).
    /// </summary>
    public int Next(int minValue, int maxValue)
    {
        lock (_lock)
        {
            return _random.Next(minValue, maxValue);
        }
    }

    /// <summary>
    /// Returns a random double between 0.0 and 1.0.
    /// </summary>
    public double NextDouble()
    {
        lock (_lock)
        {
            return _random.NextDouble();
        }
    }

    /// <summary>
    /// Fills the elements of the specified array of bytes with random numbers.
    /// </summary>
    public void NextBytes(byte[] buffer)
    {
        lock (_lock)
        {
            _random.NextBytes(buffer);
        }
    }

    private static int GenerateSeed() => Environment.TickCount;
}