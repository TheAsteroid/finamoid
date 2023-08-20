using Finamoid.Abstractions.Aggregation;
using Finamoid.Abstractions.FileHandling;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Aggregation
{
    public class CategoryAggregationWriter : ICategoryAggregationWriter
    {
        private readonly IFileWriter _fileWriter;
        private readonly ICategoryAggregationReader _categoryAggregationReader;

        public CategoryAggregationWriter(IFileWriter fileWriter, ICategoryAggregationReader categoryAggregationReader)
        {
            _fileWriter = fileWriter;
            _categoryAggregationReader = categoryAggregationReader;
        }

        public async Task WriteAsync(string path, IEnumerable<CategoryAggregation> categoryAggregations)
        {
            if (File.Exists(path))
            {
                // Merge existing aggregations into new aggregations.
                var categoryCodesToWrite = new HashSet<string>(categoryAggregations.Select(c => c.CategoryCode));

                var existingCategories = await _categoryAggregationReader.ReadAsync(path);

                var aggregationsToWrite = categoryAggregations.ToList();

                // Give new aggregations precedence over old aggregations
                aggregationsToWrite.AddRange(existingCategories.Where(e => !categoryCodesToWrite.Contains(e.CategoryCode)));

                categoryAggregations = aggregationsToWrite;
            }

            await _fileWriter.WriteAsync(
                path, 
                JsonConvert.SerializeObject(
                    categoryAggregations, 
                    Constants.IndentJson ? Formatting.Indented : Formatting.None));
        }
    }
}
