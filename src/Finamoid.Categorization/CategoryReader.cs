using Finamoid.Storage;
using Newtonsoft.Json;

namespace Finamoid.Categorization
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
    }
}
