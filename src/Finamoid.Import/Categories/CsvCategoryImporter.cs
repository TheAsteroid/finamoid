using CsvHelper;
using CsvHelper.Configuration;
using Finamoid.Categorization;
using System.Globalization;

namespace Finamoid.Import.Categories
{
    internal class CsvCategoryImporter : ICategoryImporter
    {

        private readonly CsvConfiguration _csvConfiguration = new(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = false
        };

        public async Task<IEnumerable<Category>> ReadAsync(string fullPath)
        {
            var result = new List<Category>();
            using var streamReader = new StreamReader(fullPath);
            using var csvReader = new CsvReader(streamReader, _csvConfiguration);

            Category? currentCategory = null;
            var currentFilters = new List<CategoryFilter>();
            while (await csvReader.ReadAsync())
            {
                csvReader.TryGetField(0, out string? categoryCode);
                if (!string.IsNullOrEmpty(categoryCode))
                {
                    csvReader.TryGetField(1, out string? categoryName);
                    csvReader.TryGetField(2, out string? balanceTypeString);
                    csvReader.TryGetField(3, out string? wildCardString);
                    balanceTypeString ??= string.Empty;
                    wildCardString ??= string.Empty;

                    var balanceType =
                        balanceTypeString.Equals("in", StringComparison.InvariantCultureIgnoreCase) ? BalanceType.In :
                        balanceTypeString.Equals("out", StringComparison.InvariantCultureIgnoreCase) ? BalanceType.Out :
                        BalanceType.Undefined;
                    var isWildCard = wildCardString.Equals("yes", StringComparison.InvariantCultureIgnoreCase);

                    currentFilters = new List<CategoryFilter>();
                    currentCategory = new Category(categoryCode, categoryName ?? categoryCode, null, balanceType, isWildCard, currentFilters);
                    result.Add(currentCategory);

                    continue;
                }

                csvReader.TryGetField(1, out string? partialFilter);
                if (string.IsNullOrEmpty(partialFilter))
                {
                    continue;
                }

                if (currentCategory == null)
                {
                    throw new InvalidDataException("Filter row detected but no category set yet.");
                }

                currentFilters.Add(new CategoryFilter(partialFilter, null, null));
            }

            if (result.Where(c => c.BalanceType == BalanceType.In && c.IsWildcard).Count() != 1 ||
                result.Where(c => c.BalanceType == BalanceType.Out && c.IsWildcard).Count() != 1)
            {
                throw new InvalidDataException("Please provide exactly one wildcard category for both balance types (set yes in column 4).");
            }

            return result;
        }
    }
}
