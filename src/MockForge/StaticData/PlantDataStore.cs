namespace MockForge.StaticData;

public static class PlantDataStore
{
    public static readonly string[] Flowers =
    [
        "Rose",
        "Tulip",
        "Lily",
        "Daisy",
        "Sunflower",
        "Orchid",
        "Carnation",
        "Marigold",
        "Peony",
        "Chrysanthemum",
        "Iris",
        "Gardenia"
    ];

    public static readonly string[] HousePlants =
    [
        "Monstera",
        "Pothos",
        "Chlorophytum",
        "Sansevieria",  
        "Philodendron",
        "Spathiphyllum", 
        "Aloe",
        "Ficus",
        "Tradescantia",
        "Peperomia"
    ];

    public static readonly string[] Trees =
    [
        "Oak",
        "Maple",
        "Pine",
        "Birch",
        "Palm",
        "Willow",
        "Cherry",
        "Cedar",
        "Spruce",
        "Apple"
    ];

    public static readonly string[] Herbs =
    [
        "Basil",
        "Mint",
        "Parsley",
        "Cilantro",
        "Rosemary",
        "Thyme",
        "Oregano",
        "Sage",
        "Dill"
    ];

    public static readonly string[] Shrubs =
    [
        "Azalea",
        "Rhododendron",
        "Boxwood",
        "Lilac",
        "Forsythia",
        "Holly"
    ];

    public static readonly string[] AllPlants = [.. Flowers, .. HousePlants, .. Trees, .. Herbs, .. Shrubs];
}
