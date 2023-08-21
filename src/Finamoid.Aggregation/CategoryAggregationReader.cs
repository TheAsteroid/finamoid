using Finamoid.Storage;
using Newtonsoft.Json;

namespace Finamoid.Aggregation
{
    public class CategoryAggregationReader : ICategoryAggregationReader
    {
        private readonly IStorageHandler _storageHandler;

        public CategoryAggregationReader(IStorageHandlerFactory storageHandlerFactory)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Aggregations);
        }

        public async Task<IEnumerable<CategoryAggregation>> ReadAsync(string relativePath)
        {
            var data = await _storageHandler.ReadAllTextAsync(relativePath);

            return JsonConvert.DeserializeObject<IEnumerable<CategoryAggregation>>(data) ??
                throw new InvalidDataException($"Category aggregations could not be loaded from file {relativePath}.");
        }
    }
}
