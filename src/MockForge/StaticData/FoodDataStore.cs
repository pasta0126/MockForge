namespace MockForge.StaticData;

public static class FoodDataStore
{
    public static readonly string[] Fruits =
    [
        "Apple",
        "Banana",
        "Orange",
        "Strawberry",
        "Grape",
        "Pineapple",
        "Mango",
        "Blueberry",
        "Watermelon",
        "Peach",
        "Pear",
        "Cherry",
        "Kiwi",
        "Lemon",
        "Lime",
        "Papaya",
        "Raspberry",
        "Blackberry",
        "Coconut",
        "Pomegranate",
        "Tomato",
        "Cucumber",
        "Pepper",
        "Eggplant",
        "Zucchini"
    ];

    public static readonly string[] Vegetables =
    [
        "Carrot",
        "Broccoli",
        "Spinach",
        "Lettuce",
        "Onion",
        "Garlic",
        "Potato",
        "SweetPotato",
        "Cauliflower",
        "Cabbage",
        "Kale",
        "Beet"
    ];

    public static readonly string[] Grains =
    [
        "Rice",
        "Wheat",
        "Oats",
        "Barley",
        "Quinoa",
        "Corn",
        "Rye",
        "Millet",
        "Sorghum",
        "Bulgur"
    ];

    public static readonly string[] Proteins =
    [
        "Chicken",
        "Beef",
        "Pork",
        "Turkey",
        "Fish",
        "Tofu",
        "Tempeh",
        "Eggs",
        "Lamb",
        "Shrimp"
    ];

    public static readonly string[] Dairy =
    [
        "Milk",
        "Cheese",
        "Yogurt",
        "Butter",
        "Cream"
    ];

    public static readonly string[] Legumes =
    [
        "Lentils",
        "Chickpeas",
        "BlackBeans",
        "KidneyBeans",
        "Soybeans",
        "PintoBeans",
        "GreenBean",
        "Pea",
        "Peanuts"
    ];

    public static readonly string[] NutsAndSeeds =
    [
        "Almonds",
        "Walnuts",
        "Cashews",
        "Pistachios",
        "SunflowerSeeds",
        "PumpkinSeeds",
        "ChiaSeeds",
        "Flaxseeds",
        "SesameSeeds"
    ];

    public static readonly string[] AllFoods = [.. Fruits, .. Vegetables, .. Grains, .. Proteins, .. Dairy, .. Legumes, .. NutsAndSeeds];
}
