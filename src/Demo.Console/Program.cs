using MockForge;
using MockForge.Models;
using MockForge.Providers;
using System.Text.Json;

var forge = MockForgeFactory.Create();

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

Console.WriteLine("Apartamento:");

var housingProvider = forge.Get<HousingProvider>();
ApartmentDefinition apartment = housingProvider.Generate(1, 120);

string apartmentJson = JsonSerializer.Serialize(apartment, jsonOptions);
Console.WriteLine(apartmentJson);
