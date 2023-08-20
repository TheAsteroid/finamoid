using Finamoid.Abstractions.Aggregation;
using Finamoid.Abstractions.FileHandling;
using Newtonsoft.Json;

namespace Finamoid.Aggregation
{
    public class CategoryAggregationReader : ICategoryAggregationReader
    {
        private readonly IFileReader _fileReader;

        public CategoryAggregationReader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public async Task<IEnumerable<CategoryAggregation>> ReadAsync(string path)
        {
            var data = await _fileReader.ReadAsync(path);

            return JsonConvert.DeserializeObject<IEnumerable<CategoryAggregation>>(data) ??
                throw new InvalidDataException($"Category aggregations could not be loaded from file {path}.");
        }
    }
}
