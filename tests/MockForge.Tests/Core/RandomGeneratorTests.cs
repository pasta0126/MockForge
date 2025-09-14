using MockForge.Core;

namespace MockForge.Tests.Core;

public class RandomGeneratorTests
{
    [Fact]
    public void Constructor_WithSeed_SetsSeedProperty()
    {
        // Arrange
        const int seed = 12345;

        // Act
        var generator = new RandomGenerator(seed);

        // Assert
        Assert.Equal(seed, generator.Seed);
    }

    [Fact]
    public void Constructor_WithSameSeed_ProducesSameSequence()
    {
        // Arrange
        const int seed = 12345;
        var generator1 = new RandomGenerator(seed);
        var generator2 = new RandomGenerator(seed);

        // Act & Assert
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(generator1.Next(), generator2.Next());
        }
    }

    [Fact]
    public void Next_WithMaxValue_ReturnsValueInRange()
    {
        // Arrange
        var generator = new RandomGenerator(12345);
        const int maxValue = 100;

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var value = generator.Next(maxValue);
            Assert.True(value >= 0 && value < maxValue);
        }
    }

    [Fact]
    public void Next_WithMinAndMaxValue_ReturnsValueInRange()
    {
        // Arrange
        var generator = new RandomGenerator(12345);
        const int minValue = 10;
        const int maxValue = 20;

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var value = generator.Next(minValue, maxValue);
            Assert.True(value >= minValue && value < maxValue);
        }
    }

    [Fact]
    public void NextDouble_ReturnsValueBetweenZeroAndOne()
    {
        // Arrange
        var generator = new RandomGenerator(12345);

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var value = generator.NextDouble();
            Assert.True(value >= 0.0 && value < 1.0);
        }
    }

    [Fact]
    public void NextBytes_FillsArrayWithRandomBytes()
    {
        // Arrange
        var generator = new RandomGenerator(12345);
        var buffer = new byte[10];

        // Act
        generator.NextBytes(buffer);

        // Assert
        Assert.NotEqual(new byte[10], buffer); // Should not be all zeros
    }
}