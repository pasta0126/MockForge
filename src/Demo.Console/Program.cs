using MockForge;
using MockForge.Models;
using MockForge.Providers;
using System.Text.Json;

var forge = MockForgeFactory.Create();

var answerProvider = forge.Get<AnswerProvider>();
var magicBall = answerProvider.Magic8Ball();
Console.WriteLine($"Magic 8 Ball: {magicBall}");


Console.WriteLine();

var identityProvider = forge.Get<IdentityProvider>();
Person person = identityProvider.Person(withNobelTitle: false, withTitle: false, withMiddelName: false);

Console.WriteLine("Persona:");

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
string personJson = JsonSerializer.Serialize(person, jsonOptions);
Console.WriteLine(personJson);
