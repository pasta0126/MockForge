using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.StaticData;
using MockForge.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace MockForge.Providers
{
    public sealed class IdentityProvider(
        IRandomizer r,
        DateProvider dateProvider,
        ProfessionProvider professionProvider,
        NumberProvider numberProvider) : IProvider
    {
        public string Name => "Identity";

        public string Gender() => r.Pick(IdentityDataStore.GenderData);

        public string WeightedGender()
        {
            var roll = r.NextDouble();
            if (roll < 0.46) // Male 46%,
            {
                return "Male";
            }
            if (roll < 0.92) // Female 46%,
            {
                return "Female";
            }

            // 1% each for the 8 remaining
            var others = IdentityDataStore.GenderData.Where(g => g != "Male" && g != "Female").ToArray();
            return r.Pick(others);
        }

        public string Species() => r.Pick(IdentityDataStore.SpeciesData);

        public string GetRobotName(string input, int number, bool keepSeparator)
        {
            byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(input));
            string md5Full = BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            string prefix = md5Full[..4];

            string valueB60 = Converters.ToBase60(number, keepSeparator);

            return $"{prefix}-{valueB60}";
        }

        public string MaleFirstName() => r.Pick(IdentityDataStore.MaleNameData);
        public string FemaleFirstName() => r.Pick(IdentityDataStore.FemaleNameData);

        public string LastName() => r.Pick(IdentityDataStore.LastNameData);

        public string MaleTitle() => r.Pick(IdentityDataStore.MaleTitleData);
        public string FemaleTitle() => r.Pick(IdentityDataStore.FemaleTitleData);

        public string MaleNobleTitle() => r.Pick(IdentityDataStore.MaleNobleTitleData);
        public string FemaleNobleTitle() => r.Pick(IdentityDataStore.FemaleNobleTitleData);

        public string City() => r.Pick(IdentityDataStore.NeoCityData);

        public Person Person(bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
        {
            var species = Species();
            var gender = species is "Human" or "Cyborg" or "Synthetic" ? WeightedGender() : Gender();

            if (species == "Android")
            {
                var input = Path.GetRandomFileName().Replace(".", "");
                return AndroidPerson(gender, species, input, maxAge);
            }

            if (species == "Extraterrestrial")
            {
                return ExtraterrestrialPerson(gender, species, withNobelTitle, maxAge);
            }

            if (gender == "Male")
            {
                return MalePerson(gender, species, withNobelTitle, withTitle, withMiddelName, maxAge);
            }

            if (gender == "Female")
            {
                return FemalePerson(gender, species, withNobelTitle, withTitle, withMiddelName, maxAge);
            }

            return OtherGenderPerson(gender, species, withNobelTitle, withTitle, withMiddelName, maxAge);
        }

        public Person OtherGenderPerson(string gender, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
            => OtherGenderPerson(gender, Species(), withNobelTitle, withTitle, withMiddelName, maxAge);

        public Person MalePerson(string gender, string species, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
        {
            var calculatedAge = maxAge ?? numberProvider.RandomNumber<int>(18, 120);
            var birthday = dateProvider.PastDate(calculatedAge);
            var title = withTitle ? MaleTitle() : string.Empty;
            var middleName = withMiddelName ? MaleFirstName() : string.Empty;
            var nobleTitle = withNobelTitle ? MaleNobleTitle() : string.Empty;

            return new()
            {
                NobleTitle = nobleTitle,
                Title = title,
                FirstName = MaleFirstName(),
                MiddleName = middleName,
                LastName = LastName(),
                Gender = gender,
                Species = species,
                Birthday = birthday,
                City = City(),
            };
        }

        public Person FemalePerson(string gender, string species, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
        {
            var calculatedAge = maxAge ?? numberProvider.RandomNumber<int>(18, 120);
            var birthday = dateProvider.PastDate(calculatedAge);
            var title = withTitle ? FemaleTitle() : string.Empty;
            var middleName = withMiddelName ? FemaleFirstName() : string.Empty;
            var nobleTitle = withNobelTitle ? FemaleNobleTitle() : string.Empty;

            return new()
            {
                NobleTitle = nobleTitle,
                Title = title,
                FirstName = FemaleFirstName(),
                MiddleName = middleName,
                LastName = LastName(),
                Gender = gender,
                Species = species,
                Birthday = birthday,
                City = City(),
            };
        }

        public Person AndroidPerson(string gender, string species, string input, int? maxAge = null)
        {
            var calculatedAge = maxAge ?? numberProvider.RandomNumber<int>(18, 120);
            var birthday = dateProvider.PastDate(calculatedAge);
            var num = numberProvider.RandomNumber<int>(0, 3600);

            return new()
            {
                NobleTitle = string.Empty,
                Title = string.Empty,
                FirstName = GetRobotName(input, num, false),
                MiddleName = string.Empty,
                LastName = string.Empty,
                Gender = gender,
                Species = species,
                Birthday = birthday,
                City = City(),
            };
        }

        public Person ExtraterrestrialPerson(string gender, string species, bool withNobelTitle, int? maxAge = null)
        {
            var calculatedAge = maxAge ?? numberProvider.RandomNumber<int>(18, 120);
            var birthday = dateProvider.PastDate(calculatedAge);

            var combinedNobleTitles = IdentityDataStore.MaleNobleTitleData.Concat(IdentityDataStore.FemaleNobleTitleData).ToArray();
            var combinedFirstNames = IdentityDataStore.MaleNameData.Concat(IdentityDataStore.FemaleNameData).ToArray();

            var nobleTitle = withNobelTitle ? r.Pick(combinedNobleTitles) : string.Empty;
            var firstName = r.Pick(combinedFirstNames);

            return new()
            {
                NobleTitle = nobleTitle,
                Title = string.Empty,
                FirstName = firstName,
                MiddleName = string.Empty,
                LastName = string.Empty,
                Gender = gender,
                Species = species,
                Birthday = birthday,
                City = City(),
            };
        }

        public Person OtherGenderPerson(string gender, string species, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
        {
            var calculatedAge = maxAge ?? numberProvider.RandomNumber<int>(18, 120);
            var birthday = dateProvider.PastDate(calculatedAge);

            var combinedTitles = IdentityDataStore.MaleTitleData.Concat(IdentityDataStore.FemaleTitleData).ToArray();
            var combinedNobleTitles = IdentityDataStore.MaleNobleTitleData.Concat(IdentityDataStore.FemaleNobleTitleData).ToArray();
            var combinedFirstNames = IdentityDataStore.MaleNameData.Concat(IdentityDataStore.FemaleNameData).ToArray();

            var title = withTitle ? r.Pick(combinedTitles) : string.Empty;
            var middleName = withMiddelName ? r.Pick(combinedFirstNames) : string.Empty;
            var nobleTitle = withNobelTitle ? r.Pick(combinedNobleTitles) : string.Empty;

            return new()
            {
                NobleTitle = nobleTitle,
                Title = title,
                FirstName = r.Pick(combinedFirstNames),
                MiddleName = middleName,
                LastName = LastName(),
                Gender = gender,
                Species = species,
                Birthday = birthday,
                City = City(),
            };
        }
    }
}
