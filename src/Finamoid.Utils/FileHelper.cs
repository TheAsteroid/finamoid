using System.Globalization;

namespace Finamoid.Utils
{
    public static class FileHelper
    {
        private const string _weekPrefix = "w-";
        private const string _monthPrefix = "m-";

        public static string GetFileNameDatePart(DateTime dateTime, PeriodType periodType)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new NotSupportedException($"{nameof(GetFileNameDatePart)} only supports UTC dates.");
            }

            return periodType switch
            {
                PeriodType.Week => Path.Combine(ISOWeek.GetYear(dateTime).ToString(), $"{_weekPrefix}{ISOWeek.GetWeekOfYear(dateTime):00}"),
                PeriodType.Month => Path.Combine(dateTime.Year.ToString(), $"{_monthPrefix}{dateTime.Month:00}"),
                PeriodType.Year => dateTime.Year.ToString(),
                _ => throw new InvalidOperationException($"{nameof(PeriodType)} must be defined.")
            };
        }

        public static bool FileNameIsInPeriod(string fileName, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate == null)
            {
                return true;
            }

            if (startDate.HasValue && startDate?.Kind != DateTimeKind.Utc || 
                endDate.HasValue && endDate?.Kind != DateTimeKind.Utc)
            {
                throw new NotSupportedException($"{nameof(FileNameIsInPeriod)} only supports UTC dates.");
            }

            DateTime minDate = startDate ?? DateTime.MinValue;
            DateTime maxDate = endDate ?? DateTime.MaxValue;

            var directoryName = new FileInfo(fileName).Directory?.Name;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            // Not within year range? No match
            if (!int.TryParse(directoryName, out var year) || year < minDate.Year || year > maxDate.Year)
            {
                return false;
            }

            // Within year range, but not the start or end year? Match
            if (year > minDate.Year && year < maxDate.Year)
            {
                return true;
            }

            // Within year range, and in the first year or the last year of the range?
            // Check month or week depending on file type.
            if (fileNameWithoutExtension.StartsWith(_weekPrefix) &&
                int.TryParse(fileNameWithoutExtension.Replace(_weekPrefix, string.Empty), out var week))
            {
                return
                    year == minDate.Year && week >= ISOWeek.GetWeekOfYear(minDate) ||
                    year == maxDate.Year && week <= ISOWeek.GetWeekOfYear(maxDate);
            }

            if (fileNameWithoutExtension.StartsWith(_monthPrefix) &&
                int.TryParse(fileNameWithoutExtension.Replace(_monthPrefix, string.Empty), out var month))
            {
                return
                    year == minDate.Year && month >= minDate.Month ||
                    year == maxDate.Year && month <= maxDate.Month;
            }

            return false;
        }
    }
}