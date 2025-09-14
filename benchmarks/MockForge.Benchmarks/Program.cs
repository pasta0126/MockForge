using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MockForge.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class MockForgeBenchmarks
{
    private IMockForge _mockForge = null!;

    [GlobalSetup]
    public void Setup()
    {
        _mockForge = new MockForgeCore("en", 12345);
    }

    [Benchmark]
    public async Task<string> GenerateFirstName()
    {
        return await _mockForge.Name.FirstNameAsync();
    }

    [Benchmark]
    public async Task<string> GenerateFullName()
    {
        return await _mockForge.Name.FullNameAsync();
    }

    [Benchmark]
    public async Task<string> GenerateAddress()
    {
        return await _mockForge.Address.FullAddressAsync();
    }

    [Benchmark]
    public async Task<string> GenerateEmail()
    {
        return await _mockForge.Internet.EmailAsync();
    }

    [Benchmark]
    public async Task<string> GeneratePhoneNumber()
    {
        return await _mockForge.Number.PhoneNumberAsync();
    }

    [Benchmark]
    public async Task<string> GenerateLoremSentence()
    {
        return await _mockForge.Lorem.SentenceAsync();
    }

    [Benchmark]
    public async Task<string> GenerateLoremParagraph()
    {
        return await _mockForge.Lorem.ParagraphAsync();
    }

    [Benchmark]
    public int GenerateRandomNumber()
    {
        return _mockForge.Number.Integer(1, 100);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<MockForgeBenchmarks>();
        Console.WriteLine(summary);
    }
}
