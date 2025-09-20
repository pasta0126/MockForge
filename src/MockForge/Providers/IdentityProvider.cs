using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.StaticData;

namespace MockForge.Providers
{
    public sealed class IdentityProvider(
        IRandomizer r,
        DateProvider dateProvider,
        NumberProvider numberProvider) : IProvider
    {
        public string Name => "Identity";

        public string Gender() => r.Pick(IdentityDataStore.GenderData);

        public string MaleFirstName() => r.Pick(IdentityDataStore.MaleNameData);
        public string FemaleFirstName() => r.Pick(IdentityDataStore.FemaleNameData);

        public string LastName() => r.Pick(IdentityDataStore.LastNameData);

        public string MaleTitle() => r.Pick(IdentityDataStore.MaleTitleData);
        public string FemaleTitle() => r.Pick(IdentityDataStore.FemaleTitleData);

        public string MaleNobleTitle() => r.Pick(IdentityDataStore.MaleNobleTitleData);
        public string FemaleNobleTitle() => r.Pick(IdentityDataStore.FemaleNobleTitleData);

        public string City() => r.Pick(IdentityDataStore.CityData);
        public string Department() => r.Pick(IdentityDataStore.DepartmentData);
        public string Company() => r.Pick(IdentityDataStore.CompanyData);

        public Person Person(bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
        {
            var gender = Gender();

            if (gender == "Male")
            {
                return MalePerson(gender, withNobelTitle, withTitle, withMiddelName, maxAge);
            }

            if (gender == "Female")
            {
                return FemalePerson(gender, withNobelTitle, withTitle, withMiddelName, maxAge);
            }

            return OtherGenderPerson(gender, withNobelTitle, withTitle, withMiddelName, maxAge);
        }

        public Person MalePerson(string gender, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
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
                Birthday = birthday,
                City = City(),
                Company = Company(),
                Department = Department(),
            };
        }

        public Person FemalePerson(string gender, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
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
                Birthday = birthday,
                City = City(),
                Company = Company(),
                Department = Department(),
            };
        }

        public Person OtherGenderPerson(string gender, bool withNobelTitle, bool withTitle, bool withMiddelName, int? maxAge = null)
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
                Birthday = birthday,
                City = City(),
                Company = Company(),
                Department = Department(),
            };
        }
    }
}
