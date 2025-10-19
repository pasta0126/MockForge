using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class PlantProvider(IRandomizer r) : IProvider
{
    public string Name => "Plant";

    public string Any() => r.Pick(PlantDataStore.AllPlants);

    public string Flower() => r.Pick(PlantDataStore.Flowers);
    public string HousePlant() => r.Pick(PlantDataStore.HousePlants);
    public string Tree() => r.Pick(PlantDataStore.Trees);
    public string Herb() => r.Pick(PlantDataStore.Herbs);
    public string Shrub() => r.Pick(PlantDataStore.Shrubs);
}
