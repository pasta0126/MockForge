using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class DateProvider(IRandomizer r) : IProvider
    {
        public string Name => "Date";

        public DateTime FutureDateTime(int maxYearsInFuture = 10)
        {
            var maxDays = maxYearsInFuture * 365;
            var daysToAdd = r.Next(1, maxDays + 1);
            return DateTime.Now.AddDays(daysToAdd);
        }

        public DateTime PastDateTime(int maxYearsInPast = 50)
        {
            var maxDays = maxYearsInPast * 365;
            var daysToSubtract = r.Next(1, maxDays + 1);
            return DateTime.Now.AddDays(-daysToSubtract);
        }

        public DateOnly FutureDate(int maxYearsInFuture = 10)
        {
            var maxDays = maxYearsInFuture * 365;
            var daysToAdd = r.Next(1, maxDays + 1);
            return DateOnly.FromDateTime(DateTime.Now.AddDays(daysToAdd));
        }

        public DateOnly PastDate(int maxYearsInPast = 50)
        {
            var maxDays = maxYearsInPast * 365;
            var daysToSubtract = r.Next(1, maxDays + 1);
            return DateOnly.FromDateTime(DateTime.Now.AddDays(-daysToSubtract));
        }

        public TimeOnly RandomTime()
        {
            var hours = r.Next(0, 24);
            var minutes = r.Next(0, 60);
            var seconds = r.Next(0, 60);
            return new TimeOnly(hours, minutes, seconds);
        }

        public DateTime RandomDateTime(int maxYearsPast = 50, int maxYearsFuture = 10)
        {
            var randomDays = r.Next(-maxYearsPast * 365, maxYearsFuture * 365 + 1);
            return DateTime.Now.AddDays(randomDays);
        }

        public DateOnly RandomDate(int maxYearsPast = 50, int maxYearsFuture = 10)
        {
            var randomDays = r.Next(-maxYearsPast * 365, maxYearsFuture * 365 + 1);
            return DateOnly.FromDateTime(DateTime.Now.AddDays(randomDays));
        }

        public DateTime RandomDateTimeByAgeRange(int minAge = 18, int? maxAge = 120)
        {
            var effectiveMaxAge = maxAge ?? 120;
            if (minAge > effectiveMaxAge)
                (minAge, effectiveMaxAge) = (effectiveMaxAge, minAge);

            var minDate = DateTime.Now.AddYears(-effectiveMaxAge);
            var maxDate = DateTime.Now.AddYears(-minAge);
            
            var totalDays = (int)(maxDate - minDate).TotalDays;
            var randomDays = r.Next(0, totalDays + 1);
            
            return minDate.AddDays(randomDays);
        }

        public DateOnly RandomDateByAgeRange(int minAge = 18, int? maxAge = 120)
        {
            return DateOnly.FromDateTime(RandomDateTimeByAgeRange(minAge, maxAge));
        }
    }
}