using Finamoid.Storage;
using Newtonsoft.Json;

namespace Finamoid.Import.Readers
{
    internal class JsonMutationReader : IJsonMutationReader
    {
        private readonly IStorageHandler _storageHandler;

        public JsonMutationReader(IStorageHandlerFactory storageHandlerFactory)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Mutations);
        }

        public async Task<IEnumerable<Mutation>> ReadAsync(string relativePath)
        {
            var data = await _storageHandler.ReadAllTextAsync(relativePath);

            return JsonConvert.DeserializeObject<IEnumerable<Mutation>>(data) ?? Enumerable.Empty<Mutation>();
        }
    }
}
