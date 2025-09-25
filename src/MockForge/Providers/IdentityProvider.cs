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

        public string GenerateRobotName(string seed, int number, bool keepSeparator = false)
            => GetCustomName(seed, number, keepSeparator);

        public string GenerateRobotName(bool keepSeparator = false)
        {
            var seed = $"{Company()}";
            var number = numberProvider.RandomNumber<int>(0, 1_000_000);
            return GetCustomName(seed, number, keepSeparator);
        }

        private static string GetCustomName(string input, int number, bool keepSeparator)
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
        public string Department() => r.Pick(IdentityDataStore.DepartmentData);
        public string Company() => r.Pick(IdentityDataStore.CompanyData);

        public Person Person(bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
        {
            var species = Species();
            var gender = species == "Human" ? WeightedGender() : Gender();

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
                Company = Company(),
                Department = Department(),
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
                Company = Company(),
                Department = Department(),
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
                Company = Company(),
                Department = Department(),
            };
        }
    }
}
