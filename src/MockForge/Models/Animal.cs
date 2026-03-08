namespace MockForge.Models;

public class Animal
{
    public required string Breed { get; set; }
    public required string Rarity { get; set; }
    public int Price { get; set; } = 0;
    public int Intelligence { get; set; } = 0;
    public int Vitality { get; set; } = 0;
    public int Wisdom { get; set; } = 0;
    public int Charisma { get; set; } = 0;
    public int Confidence { get; set; } = 0;
    public int Empathy { get; set; } = 0;
    public int Exoticness { get; set; } = 0;
    public int Influence { get; set; } = 0;
}
