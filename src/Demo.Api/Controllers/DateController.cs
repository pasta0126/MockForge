using Microsoft.AspNetCore.Mvc;
using MockForge.Core.Abstractions;
using MockForge.Providers;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DateController(IMockForge mockForge) : ControllerBase
    {
        [HttpGet("future-datetime")]
        public ActionResult<DateTime> GetFutureDateTime([FromQuery] int maxYearsInFuture = 10)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.FutureDateTime(maxYearsInFuture));
        }

        [HttpGet("past-datetime")]
        public ActionResult<DateTime> GetPastDateTime([FromQuery] int maxYearsInPast = 50)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.PastDateTime(maxYearsInPast));
        }

        [HttpGet("future-date")]
        public ActionResult<DateOnly> GetFutureDate([FromQuery] int maxYearsInFuture = 10)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.FutureDate(maxYearsInFuture));
        }

        [HttpGet("past-date")]
        public ActionResult<DateOnly> GetPastDate([FromQuery] int maxYearsInPast = 50)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.PastDate(maxYearsInPast));
        }

        [HttpGet("random-time")]
        public ActionResult<TimeOnly> GetRandomTime()
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.RandomTime());
        }

        [HttpGet("random-datetime")]
        public ActionResult<DateTime> GetRandomDateTime([FromQuery] int maxYearsPast = 50, [FromQuery] int maxYearsFuture = 10)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.RandomDateTime(maxYearsPast, maxYearsFuture));
        }

        [HttpGet("random-date")]
        public ActionResult<DateOnly> GetRandomDate([FromQuery] int maxYearsPast = 50, [FromQuery] int maxYearsFuture = 10)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.RandomDate(maxYearsPast, maxYearsFuture));
        }

        [HttpGet("random-datetime-by-age")]
        public ActionResult<DateTime> GetRandomDateTimeByAgeRange([FromQuery] int minAge = 18, [FromQuery] int? maxAge = 120)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.RandomDateTimeByAgeRange(minAge, maxAge));
        }

        [HttpGet("random-date-by-age")]
        public ActionResult<DateOnly> GetRandomDateByAgeRange([FromQuery] int minAge = 18, [FromQuery] int? maxAge = 120)
        {
            var provider = mockForge.Get<DateProvider>();
            return Ok(provider.RandomDateByAgeRange(minAge, maxAge));
        }
    }
}