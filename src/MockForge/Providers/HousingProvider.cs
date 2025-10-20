using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class HousingProvider(IRandomizer r, FoodProvider food, PlantProvider plant) : IProvider
{
    public string Name => "Housing";

    public ApartmentDefinition Generate(int minFloor, int maxFloor)
    {
        // normalize
        if (minFloor > maxFloor) (minFloor, maxFloor) = (maxFloor, minFloor);

        var floor = r.Next(minFloor, maxFloor + 1);

        var uid = Guid.NewGuid().ToString();

        // zone: Alta -> fruits (FoodProvider.Fruit), Economica -> flowers (PlantProvider.Flower)
        var zone = floor switch
        {
            <= 30 => "C",
            <= 60 => "B",
            <= 90 => "A",
            <= 110 => "L",
            _ => "AT"
        };

        var zoneName = floor <= 60 ? "Economica" : "Alta"; // user wanted: zona alta are fruits, zona economica uses plant provider for flowers

        var neighborhood = zoneName == "Alta" ? food.Fruit() : plant.Flower();

        var floorsPerLanding = zone switch
        {
            "L" => 4,
            "AT" => 1,
            _ => 2
        };

        var hasParking = zone is "L" or "AT";

        var bedrooms = zone switch
        {
            "C" => 0,
            "B" => r.Next(1, 3),
            "A" => r.Next(2, 4),
            "L" => r.Next(3, 6),
            _ => r.Next(2, 5)
        };

        var bathrooms = Math.Max(1, bedrooms / 2);

        var size = zone switch
        {
            "C" => r.Next(10, 30),
            "B" => r.Next(30, 60),
            "A" => r.Next(60, 90),
            "L" => r.Next(90, 110),
            _ => r.Next(110, 120)
        };

        // select short phrase pool and count
        var (pool, count) = zone switch
        {
            "C" => (HousingDataStore.ApartmentCShortPhrases, 4),
            "B" => (HousingDataStore.ApartmentBShortPhrases, 5),
            "A" => (HousingDataStore.ApartmentAShortPhrases, 6),
            "L" => (HousingDataStore.ApartmentLShortPhrases, 8),
            _ => (HousingDataStore.ApartmentATShortPhrases, 10)
        };

        var selected = new HashSet<string>();
        var phrases = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var p = r.Pick<string>(pool);
            if (selected.Add(p)) phrases.Add(p);
            else i--; // retry to ensure unique phrases
        }

        // core description parts
        var details = new List<string>
        {
            string.Join(", ", phrases),
            zone switch
            {
                "C" => "Category C: minimal ventilation, no natural light, often subterranean.",
                "B" => "Category B: small, limited light and ventilation.",
                "A" => "Category A: medium size, some scenic views, moderate ventilation.",
                "L" => "Category L: large and spacious, premium features.",
                _ => "AT: top-tier penthouse-like residence, exclusive amenities."
            },
            $"Neighborhood: {neighborhood} (Zone: {zoneName})",
            $"Floors per landing: {floorsPerLanding}, Parking: {hasParking}",
            $"Bedrooms: {bedrooms}, Bathrooms: {bathrooms}, Size: {size}sqm"
        };

        var description = string.Join(" ", details);

        var def = new ApartmentDefinition
        {
            Uid = uid,
            Neighborhood = neighborhood,
            Zone = zoneName,
            Category = zone,
            FloorsPerLanding = floorsPerLanding,
            HasParking = hasParking,
            Bedrooms = bedrooms,
            Bathrooms = bathrooms,
            Size = size,
            Description = description
        };

        return def;
    }
}
