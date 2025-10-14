namespace MockForge.StaticData;

public static class AnimalDataStore
{
    public static readonly string[] RealAnimals =
    [
        "Dog",
        "Cat",
        "Parrot",
        "Rabbit",
        "Fox",
        "Elephant",
        "Dolphin",
        "Penguin",
        "Shark",
        "Pigeon",
        "Meerkat",
        "Wolf",
        "Bear",
        "Snake",
        "Turtle",
        "Monkey",
        "Octopus",
        "Crab",
        "Kangaroo",
        "Koala",
        "Peacock",
        "Falcon"
    ];

    public static readonly string[] MythicalAnimals =
    [
        "Dragon",
        "Phoenix",
        "Griffin",
        "Unicorn",
        "Pegasus",
        "Hydra",
        "Kraken",
        "Basilisk",
        "Chimera",
        "Mermaid",
        "Centaur",
        "Minotaur",
        "Sphinx",
        "Wyvern",
        "Cyclops",
        "Fairy",
        "Goblin",
        "Werewolf",
        "Vampire",
        "Yeti",
        "Manticore",
        "Hippogriff",
        "Cerberus",
        "Leviathan",
        "Selkie",
        "Nymph",
        "Dryad",
        "Djinn",
        "Sprite",
        "Gorgon",
        "Kelpie",
        "Jackalope",
        "Banshee",
        "Troll",
        "Ogre",
        "Orb",
        "Willow-the-Wisp"
    ];

    public static readonly string[] Animals = [.. RealAnimals, ..MythicalAnimals];
}
