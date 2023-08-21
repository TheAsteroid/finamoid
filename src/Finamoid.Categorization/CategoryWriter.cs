using Finamoid.Categorization;
using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Categorization
{
    public class CategoryWriter : ICategoryWriter
    {
        private readonly IStorageHandler _storageHandler;

        public CategoryWriter(IStorageHandlerFactory storageHandlerFactory)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Categories);
        }

        public Task WriteAsync(string path, IEnumerable<Category> categories)
        {
            var data = JsonConvert.SerializeObject(categories, Constants.IndentJson ? Formatting.Indented : Formatting.None);

            return _storageHandler.WriteAsync(path, data);
        }
    }
}
