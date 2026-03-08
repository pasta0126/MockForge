using MockForge.Core.Abstractions;
using MockForge.Models;

namespace MockForge.Providers;

public sealed class RarityProvider(IRandomizer r) : IProvider
{
    public string Name => "Rarity";

    private static readonly (ItemRarity Rarity, int Difficulty, int BasePrice)[] RarityData =
    [
        (ItemRarity.Common, 5000, 1),
        (ItemRarity.Uncommon, 3000, 8),
        (ItemRarity.Rare, 1500, 100),
        (ItemRarity.Epic, 400, 800),
        (ItemRarity.Exotic, 50, 1_000),
        (ItemRarity.Relic, 20, 8_000),
        (ItemRarity.Cursed, 15, 10_000),
        (ItemRarity.Legendary, 10, 80_000),
        (ItemRarity.Mythical, 4, 100_000),
        (ItemRarity.Divine, 1, 800_000)
    ];

    public RarityResult GetRarity(double luckModifier = 0.0)
    {
        var totalDifficulty = RarityData.Sum(x => x.Difficulty);
        var randomValue = r.Next(1, totalDifficulty + 1);
        
        if (luckModifier > 0)
        {
            var adjustedValue = randomValue * Math.Pow(1 - (randomValue / (double)totalDifficulty), luckModifier);
            randomValue = (int)Math.Max(1, adjustedValue);
        }
        
        var cumulativeWeight = 0;
        foreach (var (rarity, difficulty, basePrice) in RarityData)
        {
            cumulativeWeight += difficulty;
            if (randomValue <= cumulativeWeight)
            {
                return new RarityResult
                {
                    Rarity = rarity.ToString(),
                    BasePrice = basePrice
                };
            }
        }

        return new RarityResult
        {
            Rarity = ItemRarity.Common.ToString(),
            BasePrice = 1
        };
    }

    public string GetRarityName(double luckModifier = 0.0) => GetRarity(luckModifier).Rarity;
    
    public int GetRarityPrice(double luckModifier = 0.0) => GetRarity(luckModifier).BasePrice;
}
