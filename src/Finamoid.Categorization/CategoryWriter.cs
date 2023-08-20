using Finamoid.Abstractions.Categorization;
using Finamoid.Abstractions.FileHandling;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Categorization
{
    public class CategoryWriter : ICategoryWriter
    {
        private readonly IFileWriter _fileWriter;

        public CategoryWriter(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        public Task WriteAsync(string path, IEnumerable<Category> categories)
        {
            var data = JsonConvert.SerializeObject(categories, Constants.IndentJson ? Formatting.Indented : Formatting.None);

            return _fileWriter.WriteAsync(path, data);
        }
    }
}
