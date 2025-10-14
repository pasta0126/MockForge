using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class AnimalProvider(IRandomizer r) : IProvider
{
    public string Name => "Animal";

    public string Any() => r.Pick(AnimalDataStore.Animals);
    public string Real() => r.Pick(AnimalDataStore.RealAnimals);
    public string Mythical() => r.Pick(AnimalDataStore.MythicalAnimals);
}
