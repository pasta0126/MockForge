using MockForge.Core.Abstractions;
using MockForge.Models;
using MockForge.StaticData;
using System.Linq;

namespace MockForge.Providers
{
    public sealed class ProfessionProvider(IRandomizer r) : IProvider
    {
        public string Name => "Profession";

        public string Company() => r.Pick(ProfessionDataStore.CompanyData);
        public string CompanyDepartment() => r.Pick(ProfessionDataStore.CompanyDepartmentData);

        public string StateEmployeeDepartment() => r.Pick(ProfessionDataStore.StateEmployeeDepartmentData);
        public string StateEmployeeRank() => r.Pick(ProfessionDataStore.StateEmployeeRankData);

        public string PoliceDepartment() => r.Pick(ProfessionDataStore.PoliceDepartmentData);
        public string PoliceRank() => r.Pick(ProfessionDataStore.PoliceRankData);

        public Profession Profession() => new()
        {
            Company = Company(),
            Department = CompanyDepartment()
        };

        public Profession PoliceEmployee() => new()
        {
            Company = "Police",
            Department = PoliceDepartment(),
            Rank = PoliceRank()
        };

        public Profession PublicStateEmployee()
        {
            var department = ProfessionDataStore.StateEmployeeDepartmentData
                .Where(d => d != "Police")
                .ToArray();

            return new Profession
            {
                Company = "State Government",
                Department = r.Pick(department),
                Rank = StateEmployeeRank()
            };
        }
    }
}
