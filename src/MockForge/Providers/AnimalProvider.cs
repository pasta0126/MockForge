using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class AnimalProvider(IRandomizer r, RarityProvider rarityProvider) : IProvider
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

    public Animal CreateAnimal(bool isMythical = false)
    {
        return isMythical ? CreateMythicalAnimal() : CreateRealAnimal();
    }

    public Animal CreateRealAnimal()
    {
        var rarityResult = rarityProvider.GetRarity(luckModifier: 0.0);
        
        var minPrice = 1000 * rarityResult.RarityLevel + rarityResult.BasePrice;
        var maxPrice = 5000 * rarityResult.RarityLevel + rarityResult.BasePrice;
        
        var animal = new Animal
        {
            Breed = Real(),
            Rarity = rarityResult.Rarity,
            Price = r.Next(minPrice, maxPrice)
        };

        AssignRandomProperties(animal, r.Next(1, 3));
        return animal;
    }

    public Animal CreateMythicalAnimal()
    {
        var rarityResult = rarityProvider.GetRarity(luckModifier: 1.5, minRarity: ItemRarity.Rare);
        
        var minPrice = 10000 * rarityResult.RarityLevel + rarityResult.BasePrice;
        var maxPrice = 50000 * rarityResult.RarityLevel + rarityResult.BasePrice;
        
        var animal = new Animal
        {
            Breed = Mythical(),
            Rarity = rarityResult.Rarity,
            Price = r.Next(minPrice, maxPrice)
        };

        AssignRandomProperties(animal, r.Next(2, 4));
        return animal;
    }

    private void AssignRandomProperties(Animal animal, int propertyCount)
    {
        var allProperties = new Action<int>[]
        {
            value => animal.Intelligence = value,
            value => animal.Vitality = value,
            value => animal.Wisdom = value,
            value => animal.Charisma = value,
            value => animal.Confidence = value,
            value => animal.Empathy = value,
            value => animal.Exoticness = value,
            value => animal.Influence = value
        };

        var shuffledProperties = r.Shuffle(allProperties);
        
        for (int i = 0; i < propertyCount && i < shuffledProperties.Length; i++)
        {
            shuffledProperties[i](r.Next(1, 6));
        }
    }
}
