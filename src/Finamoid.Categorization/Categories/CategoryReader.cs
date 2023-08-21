using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Categorization.Categories
{
    public class CategoryReader : ICategoryReader
    {
        private readonly IStorageHandler _storageHandler;

        public CategoryReader(IStorageHandlerFactory storageHandlerFactory)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Categories);
        }

        public async Task<IEnumerable<Category>> ReadAsync(string path)
        {
            var data = await _storageHandler.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject<IEnumerable<Category>>(data) ??
                throw new InvalidDataException($"Categories could not be loaded from file {path}.");
        }

        public Task<IEnumerable<Category>> ReadLatestAsync()
        {
            var latestFile = _storageHandler
                .EnumerateFiles(string.Empty, $"*{Constants.DataFileExtension}", SearchOption.AllDirectories)
                .OrderBy(s => s)
                .LastOrDefault();

            return latestFile == null ? throw new FileNotFoundException("No categories file found.") : ReadAsync(latestFile);
        }
    }
}
