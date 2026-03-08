using MockForge.Core.Abstractions;
using MockForge.Models;
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

    public Animal CreateAnimal(bool isMythical = false)
    {
        return isMythical ? CreateMythicalAnimal() : CreateRealAnimal();
    }

    public Animal CreateRealAnimal()
    {
        var animal = new Animal
        {
            Name = Real(),
            Price = RealPrice()
        };

        AssignRandomProperties(animal, r.Next(1, 3));
        return animal;
    }

    public Animal CreateMythicalAnimal()
    {
        var animal = new Animal
        {
            Name = Mythical(),
            Price = MythicalPrice()
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
