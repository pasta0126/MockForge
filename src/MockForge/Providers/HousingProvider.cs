using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.StaticData;

namespace MockForge.Providers;

public sealed class HousingProvider(IRandomizer r, FoodProvider food, PlantProvider plant) : IProvider
{
    public string Name => "Housing";

    public ApartmentDefinition Generate(int minFloor, int maxFloor)
    {
        if (minFloor > maxFloor) (minFloor, maxFloor) = (maxFloor, minFloor);

        var floor = r.Next(minFloor, maxFloor + 1);

        var uid = Guid.NewGuid().ToString();

        var zone = floor switch
        {
            <= 30 => "C",
            <= 60 => "B",
            <= 90 => "A",
            <= 110 => "L",
            _ => "AT"
        };

        var zoneName = floor <= 60 ? "Underprivileged" : "Affluent";

        var neighborhood = zoneName == "Affluent" ? food.Fruit() : plant.Flower();

        var floorsPerLanding = zone switch
        {
            "L" => 4,
            "AT" => 1,
            _ => 2
        };

        var hasParking = zone is "L" or "AT";

        var bedrooms = zone switch
        {
            "C" => r.Next(0, 1),
            "B" => r.Next(0, 3),
            "A" => r.Next(2, 4),
            "L" => r.Next(3, 6),
            _ => r.Next(2, 5)
        };

        var bathrooms = Math.Max(1, bedrooms / 2);

        var size = zone switch
        {
            "C" => r.Next(10, 45),
            "B" => r.Next(30, 70),
            "A" => r.Next(60, 100),
            "L" => r.Next(90, 120),
            _ => r.Next(150, 250)
        };

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
            else i--; 
        }

        var details = new List<string>
        {
            string.Join(", ", phrases),
            zone switch
            {
                "C" => "Category C: Minimal ventilation, no natural light, often subterranean.",
                "B" => "Category B: Small, limited light and ventilation.",
                "A" => "Category A: Medium size, some scenic views, moderate ventilation.",
                "L" => "Category L: Large and spacious, premium features.",
                _ => "AT: Top-tier penthouse-like residence, exclusive amenities."
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
