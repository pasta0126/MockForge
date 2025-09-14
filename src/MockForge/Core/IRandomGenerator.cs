namespace MockForge.Core;

/// <summary>
/// Interface for thread-safe random number generation with seed support.
/// </summary>
public interface IRandomGenerator
{
    /// <summary>
    /// Gets the seed used to initialize the random generator.
    /// </summary>
    int Seed { get; }

    /// <summary>
    /// Returns a random integer.
    /// </summary>
    int Next();

    /// <summary>
    /// Returns a random integer between 0 (inclusive) and maxValue (exclusive).
    /// </summary>
    int Next(int maxValue);

    /// <summary>
    /// Returns a random integer between minValue (inclusive) and maxValue (exclusive).
    /// </summary>
    int Next(int minValue, int maxValue);

    /// <summary>
    /// Returns a random double between 0.0 and 1.0.
    /// </summary>
    double NextDouble();

    /// <summary>
    /// Fills the elements of the specified array of bytes with random numbers.
    /// </summary>
    void NextBytes(byte[] buffer);
}