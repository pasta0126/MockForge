# MockForge

A lightweight .NET8 library to generate fake data (mock) with zero configuration.

## Providers

- `AnswerProvider`
  - `Magic8Ball()`
  - `YesNo()`
  - `TrueFalse()`

- `IdentityProvider`
  - `Gender()`
  - `WeightedGender()`
  - `Species()`
  - `GetRobotName(string input, int number, bool keepSeparator)`
  - `MaleFirstName()`, `FemaleFirstName()`, `LastName()`
  - `MaleTitle()`, `FemaleTitle()`
  - `MaleNobleTitle()`, `FemaleNobleTitle()`
  - `City()`
  - `Person(...)` and helpers:
    - `MalePerson(...)`
    - `FemalePerson(...)`
    - `AndroidPerson(...)`
    - `ExtraterrestrialPerson(...)`
    - `OtherGenderPerson(...)`

- `ColorProvider`
  - `GetHexColor(string input)`
  - `GetRgbColor(string input)`
  - `GetRgbaColor(string input, double alpha = 1.0)`
  - `GetColorName()`

- `DateProvider`
  - `FutureDateTime(int maxYearsInFuture = 10)`
  - `PastDateTime(int maxYearsInPast = 50)`
  - `FutureDate(int maxYearsInFuture = 10)`
  - `PastDate(int maxYearsInPast = 50)`
  - `RandomTime()`
  - `RandomDateTime(int maxYearsPast = 50, int maxYearsFuture = 10)`
  - `RandomDate(int maxYearsPast = 50, int maxYearsFuture = 10)`
  - `RandomDateTimeByAgeRange(int minAge = 18, int? maxAge = 120)`
  - `RandomDateByAgeRange(int minAge = 18, int? maxAge = 120)`

- `NumberProvider`
  - `RandomDecimal(decimal? min = 0, decimal? max = 1, int? decimals = 2)`
  - `RandomNumber<T>(T? min = default, T? max = default) where T : struct, IComparable<T>, IConvertible`
  - `RandomPaddedNumber(int length, int maxInclusive)`

- `CardProvider`
  - `PokerCardRank()`, `PokerCardSuit()`, `PokerCard()`
  - `SpanishCardRank()`, `SpanishCardSuit()`, `SpanishCard()`
  - `UnoColor()`, `UnoNumberCard()`, `UnoActionCard()`, `UnoWildCard()`, `UnoCard()`
  - `TarotMajorArcana()`, `TarotArcanoMayor()`

- `ProfessionProvider`
  - `Company()`, `CompanyDepartment()`
  - `StateEmployeeDepartment()`, `StateEmployeeRank()`
  - `PoliceDepartment()`, `PoliceRank()`
  - `Profession()` — returns a `Profession` model
  - `PoliceEmployee()`, `PublicStateEmployee()`

- `FoodProvider`
  - `Any()`, `Fruit()`, `Vegetable()`, `Grain()`, `Protein()`, `Dairy()`, `Legume()`, `NutOrSeed()`

- `AnimalProvider`
  - `Any()`, `Real()`, `Mythical()`

- `PlantProvider`
  - `Any()`, `Flower()`, `HousePlant()`, `Tree()`, `Herb()`, `Shrub()`

- `HousingProvider`
  - `Generate(int minFloor, int maxFloor)` — returns `ApartmentDefinition`

- `ImageProvider`
  - `GenerateRandomBitmap(int width, int height, int tileSize = 32)`
  - `GeneratePngRGBNative(int width, int height, int tileSize = 32, int delta = 30)`
  - `GeneratePngHSVNative(int width, int height, int tileSize = 32, float maxHueStep = 15f)`
  - `GenerateAvatarPng(string seed, int logicalSize = 8, int scale = 8)`

- `ProgressionProvider`
  - `Fibonacci(int n)`, `Jacobsthal(int n)`, `Lucas(int n)`, `Pell(int n)`, `HofstadterQ(int n)`, `Exotic(int n)`
  - `LogisticMap(int n, double r = 4.0, double x0 = 0.2)`
  - Random variants:
    - `RandomFibonacci(...)`
    - `RandomJacobsthal(...)`
    - `RandomLucas(...)`
    - `RandomPell(...)`
    - `RandomHofstadterQ(...)`
    - `RandomLogisticMap(...)`
    - `RandomExotic(...)`
    - `RandomProgression(...)`
  - `GetProgressionName(int progressionType)`
  - `ClearCache()`, `ClearCache(string progressionName)`

- `EnumProvider`
  - `GetRandomValue<T>() where T : struct, Enum`
  - `IsValid<T>(string? value) where T : struct, Enum`

- `ArrayProvider`
  - `Pick<T>(T[]? array)`

## Quick start

### Example without DI

```csharp
using System;
using MockForge;
using MockForge.Providers;

var forge = MockForgeFactory.Create();
var answer = forge.Get<AnswerProvider>().Magic8Ball();
Console.WriteLine($"Magic8 Ball: {answer}");
```

### Example with DI

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using MockForge;
using MockForge.Providers;

var services = new ServiceCollection();
// Register the provider using the factory. Alternatively, register the full "forge" instance.
services.AddSingleton(sp => MockForgeFactory.Create().Get<AnswerProvider>());

var provider = services.BuildServiceProvider().GetRequiredService<AnswerProvider>();
Console.WriteLine(provider.Magic8Ball());
```
