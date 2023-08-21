using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Aggregation
{
    public class CategoryAggregationWriter : ICategoryAggregationWriter
    {
        private readonly IStorageHandler _storageHandler;
        private readonly ICategoryAggregationReader _categoryAggregationReader;

        public CategoryAggregationWriter(IStorageHandlerFactory storageHandlerFactory, ICategoryAggregationReader categoryAggregationReader)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Aggregations);
            _categoryAggregationReader = categoryAggregationReader;
        }

        public async Task WriteAsync(string path, IEnumerable<CategoryAggregation> categoryAggregations)
        {
            if (_storageHandler.FileExists(path))
            {
                // Merge existing aggregations into new aggregations.
                var categoryCodesToWrite = new HashSet<string>(categoryAggregations.Select(c => c.CategoryCode));

                var existingCategories = await _categoryAggregationReader.ReadAsync(path);

                var aggregationsToWrite = categoryAggregations.ToList();

                // Give new aggregations precedence over old aggregations
                aggregationsToWrite.AddRange(existingCategories.Where(e => !categoryCodesToWrite.Contains(e.CategoryCode)));

                categoryAggregations = aggregationsToWrite;
            }

            await _storageHandler.WriteAsync(
                path, 
                JsonConvert.SerializeObject(
                    categoryAggregations, 
                    Constants.IndentJson ? Formatting.Indented : Formatting.None));
        }
    }
}
