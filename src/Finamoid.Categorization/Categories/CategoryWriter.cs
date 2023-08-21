using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Categorization.Categories
{
    public class CategoryWriter : ICategoryWriter
    {
        private readonly IStorageHandler _storageHandler;

        public CategoryWriter(IStorageHandlerFactory storageHandlerFactory)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Categories);
        }

        public Task WriteAsync(IEnumerable<Category> categories)
        {
            var data = JsonConvert.SerializeObject(categories, Constants.IndentJson ? Formatting.Indented : Formatting.None);

            return _storageHandler.WriteAsync($"{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss}{Constants.DataFileExtension}", data);
        }
    }
}
