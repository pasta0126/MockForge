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
        var hasParking = zone is "L" or "AT";

        var bedrooms = zone switch
        {
            "C" => r.Next(0, 1),
            "B" => r.Next(0, 3),
            "A" => r.Next(2, 4),
            "L" => r.Next(3, 6),
            "AT" => r.Next(4, 8),
            _ => r.Next(2, 5)
        };

        var bathrooms = zone switch
        {
            "C" => 0,
            "B" => 1,
            "A" => r.Next(1, 2),
            "L" => 2,
            "AT" => r.Next(2, 4),
            _ => 1
        };

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
            "AT" => (HousingDataStore.ApartmentLShortPhrases, 10),
            _ => (HousingDataStore.ApartmentATShortPhrases, 3)
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
            zone switch
            {
                "C" => "Minimal Ventilation, No Natural Light, Maybe Subterranean",
                "B" => "Small, Limited Light And Ventilation",
                "A" => "Medium Size, Some Scenic Views, Moderate Ventilation",
                "L" => "Large And Spacious, Premium Features",
                "AT" => "Top-Tier Penthouse-Like Residence, Exclusive Amenities",
                _ => "Standard Apartment With Average Features"
            },

            string.Join(". ", phrases.Select(p => char.ToUpper(p[0]) + p[1..]))
        };

        var description = string.Join(". ", details) + ".";

        var (rentalPrice, purchasePrice) = CalculatePrices(floor, size, zone, bathrooms, bedrooms);

        var def = new ApartmentDefinition
        {
            Uid = uid,
            Neighborhood = neighborhood,
            Zone = zoneName,
            Category = zone,
            HasParking = hasParking,
            Bedrooms = bedrooms,
            Bathrooms = bathrooms,
            SqmSize = size,
            Description = description,
            RentalPrice = rentalPrice,
            PurchasePrice = purchasePrice
        };

        return def;
    }

    private static (int rentalPrice, int purchasePrice) CalculatePrices(int floor, int size, string zone, int bathrooms, int bedrooms)
    {
        var basePricePerSqm = zone switch
        {
            "C" => 500,
            "B" => 1200,
            "A" => 2500,
            "L" => 4500,
            "AT" => 8000,
            _ => 1000
        };

        var floorMultiplier = 1.0 + (floor * 0.01);

        var bathroomBonus = bathrooms * 5000;

        var bedroomBonus = bedrooms * 8000;

        var purchasePrice = (int)((basePricePerSqm * size * floorMultiplier) + bathroomBonus + bedroomBonus);

        var rentalPrice = (int)(purchasePrice * 0.005);

        return (rentalPrice, purchasePrice);
    }

    public ApartmentDefinition GenerateUnderprivilegedApartment() => Generate(1, 60);
    public ApartmentDefinition GenerateAffluentApartment() => Generate(61, 120);

    public ApartmentDefinition GenerateCategoryAApartment() => Generate(1, 30);
    public ApartmentDefinition GenerateCategoryBApartment() => Generate(31, 60);
    public ApartmentDefinition GenerateCategoryCApartment() => Generate(61, 90);
    public ApartmentDefinition GenerateCategoryLApartment() => Generate(91, 110);
    public ApartmentDefinition GenerateCategoryATApartment() => Generate(111, 120);
}