using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers
{
    public sealed class IdentityProvider(IRandomizer r) : IProvider
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

        //public string FullName(string? gender = null) => $"{FirstName(gender)} {LastName()}";
        //public string FullNameWithTitle(string? gender = null) => $"{Title(gender)} {FullName(gender)}";
        //public string FullNameWithNobleTitle(string? gender = null) => $"{NobleTitle(gender)} {FullName(gender)}";
    }
}
