namespace MockForge.Models
{
    public class Person
    {
        public string? NobleTitle { get; set; }
        public string? Title { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }

        public string FullName => string.Join(" ", new[] { NobleTitle, Title, FirstName, MiddleName, LastName }
            .Where(s => !string.IsNullOrEmpty(s)));

        public required string Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public int Age => CalculateAge(Birthday);

        public required string City { get; set; }

        public required string Company { get; set; }
        public required string Department { get; set; }

        private static int CalculateAge(DateOnly birthday)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age)) age--;
            return age;
        }
    }
}
