namespace Finamoid.Utils
{
    public static class PeriodHelper
    {
        public static DateTime GetPeriodStartDate(DateTime dateTime, PeriodType periodType)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new NotSupportedException($"{nameof(GetPeriodStartDate)} only supports UTC dates.");
            }

            return periodType switch
            {
                PeriodType.Week => dateTime.AddDays(-((int)dateTime.DayOfWeek + 6) % 7).Date,
                PeriodType.Month => new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                PeriodType.Year => new DateTime(dateTime.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                _ => throw new NotSupportedException($"{nameof(PeriodType)} {periodType} is not supported.")
            };
        }

        public static DateTime GetPeriodEndDate(DateTime dateTime, PeriodType periodType)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new NotSupportedException($"{nameof(GetPeriodEndDate)} only supports UTC dates.");
            }

            return periodType switch
            {
                PeriodType.Week => dateTime.AddDays(-(int)dateTime.DayOfWeek % 7).Date,
                PeriodType.Month => new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    DateTime.DaysInMonth(dateTime.Year, dateTime.Month),
                    0,
                    0,
                    0,
                    DateTimeKind.Utc),
                PeriodType.Year => new DateTime(dateTime.Year, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                _ => throw new InvalidOperationException($"{nameof(PeriodType)} must be defined.")
            };
        }
    }
}
