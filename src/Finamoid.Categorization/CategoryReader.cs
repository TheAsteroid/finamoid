using Finamoid.Abstractions.Categorization;
using Finamoid.Abstractions.FileHandling;
using Newtonsoft.Json;

namespace Finamoid.Categorization
{
    public class CategoryReader : ICategoryReader
    {
        private readonly IFileReader _fileReader;

        public CategoryReader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public async Task<IEnumerable<Category>> ReadAsync(string path)
        {
            var data = await _fileReader.ReadAsync(path);

            return JsonConvert.DeserializeObject<IEnumerable<Category>>(data) ??
                throw new InvalidDataException($"Categories could not be loaded from file {path}.");
        }
    }
}
