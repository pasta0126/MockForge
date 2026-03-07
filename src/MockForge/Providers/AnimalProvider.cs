using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class AnimalProvider(IRandomizer r) : IProvider
{
    public string Name => "Animal";

    public string Any() => r.Pick(AnimalDataStore.Animals);
    public string Real() => r.Pick(AnimalDataStore.RealAnimals);
    public string Mythical() => r.Pick(AnimalDataStore.MythicalAnimals);

    public int PurchasePrice(bool isMythical = false)
    {
        return isMythical ? MythicalPrice() : RealPrice();
    }

    public int RealPrice() => r.Next(1000, 50000);
    
    public int MythicalPrice() => r.Next(10000, 500000);
}
