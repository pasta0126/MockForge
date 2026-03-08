namespace MockForge.Models;

public enum ItemRarity
{
    Common = 0,     // Difficulty: 5000 | BasePrice:       1
    Uncommon = 1,   // Difficulty: 3000 | BasePrice:       8
    Rare = 2,       // Difficulty: 1500 | BasePrice:     100
    Epic = 3,       // Difficulty:  400 | BasePrice:     800
    Exotic = 4,     // Difficulty:   50 | BasePrice:   1_000
    Relic = 5,      // Difficulty:   20 | BasePrice:   8_000
    Cursed = 6,     // Difficulty:   15 | BasePrice:  10_000
    Legendary = 7,  // Difficulty:   10 | BasePrice:  80_000
    Mythical = 8,   // Difficulty:    4 | BasePrice: 100_000
    Divine = 9      // Difficulty:    1 | BasePrice: 800_000
}
