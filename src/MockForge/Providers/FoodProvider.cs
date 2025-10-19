using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class FoodProvider(IRandomizer r) : IProvider
{
    public string Name => "Food";

    public string Any() => r.Pick(FoodDataStore.AllFoods);

    public string Fruit() => r.Pick(FoodDataStore.Fruits);
    public string Vegetable() => r.Pick(FoodDataStore.Vegetables);

    public string Grain() => r.Pick(FoodDataStore.Grains);
    public string Protein() => r.Pick(FoodDataStore.Proteins);
    public string Dairy() => r.Pick(FoodDataStore.Dairy);
    public string Legume() => r.Pick(FoodDataStore.Legumes);
    public string NutOrSeed() => r.Pick(FoodDataStore.NutsAndSeeds);
}
