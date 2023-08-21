using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Aggregation.CategoryAggregations
{
    internal class CategoryAggregationWriter : ICategoryAggregationWriter
    {
        private readonly IStorageHandler _storageHandler;
        private readonly ICategoryAggregationReader _categoryAggregationReader;

        public CategoryAggregationWriter(IStorageHandlerFactory storageHandlerFactory, ICategoryAggregationReader categoryAggregationReader)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Aggregations);
            _categoryAggregationReader = categoryAggregationReader;
        }

        public Task WriteToLatestAsync(IEnumerable<CategoryAggregation> categoryAggregations)
        {
            var latestFile = _storageHandler
                .EnumerateFiles(string.Empty, $"*{Constants.DataFileExtension}", SearchOption.AllDirectories)
                .OrderBy(s => s)
                .LastOrDefault();

            latestFile ??= GetDateFileName();
            
            return WriteAsync(latestFile, categoryAggregations);
        }

        public Task WriteToNewAsync(IEnumerable<CategoryAggregation> categoryAggregations)
        {
            return WriteAsync(GetDateFileName(), categoryAggregations);
        }

        private async Task WriteAsync(string path, IEnumerable<CategoryAggregation> categoryAggregations)
        {
            if (_storageHandler.FileExists(path))
            {
                // Merge existing aggregations into new aggregations.
                // This does assume they are full periods and the PeriodType with which they were stored was the same.
                var aggregationIdentifiersToWrite = new HashSet<(string, DateTime)>(categoryAggregations.Select(c => (c.CategoryCode, c.StartDate)));

                var existingAggregations = await _categoryAggregationReader.ReadAsync(path);

                var aggregationsToWrite = categoryAggregations.ToList();

                // Give new aggregations precedence over old aggregations
                aggregationsToWrite.AddRange(existingAggregations.Where(e => !aggregationIdentifiersToWrite.Contains((e.CategoryCode, e.StartDate))));

                categoryAggregations = aggregationsToWrite;
            }

            await _storageHandler.WriteAsync(
                path,
                JsonConvert.SerializeObject(
                    categoryAggregations.OrderBy(c => c.StartDate),
                    Constants.IndentJson ? Formatting.Indented : Formatting.None));
        }

        private static string GetDateFileName()
        {
            return $"{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss}{Constants.DataFileExtension}";
        }
    }
}
