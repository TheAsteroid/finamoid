using Finamoid.Abstractions;

namespace Finamoid.Utils
{
    public static class PeriodHelper
    {
        public static DateTime GetPeriodStartDate(DateTime dateTime, PeriodType periodType)
        {
            return periodType switch
            {
                PeriodType.Week => dateTime.AddDays(-((int)dateTime.DayOfWeek + 6) % 7).Date,
                PeriodType.Month => new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                PeriodType.Year => new DateTime(dateTime.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                _ => throw new InvalidOperationException($"{nameof(PeriodType)} must be defined.")
            };
        }
    }
}
