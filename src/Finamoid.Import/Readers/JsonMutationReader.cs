using Finamoid.Abstractions;
using Finamoid.Abstractions.FileHandling;
using Newtonsoft.Json;

namespace Finamoid.Import.Readers
{
    public class JsonMutationReader : MutationReader, IJsonMutationReader
    {
        private readonly IFileReader _fileReader;

        public JsonMutationReader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public override async Task<IEnumerable<Mutation>> ReadFromFileAsync(string path)
        {
            var data = await _fileReader.ReadAsync(path);

            return JsonConvert.DeserializeObject<IEnumerable<Mutation>>(data) ?? Enumerable.Empty<Mutation>();
        }
    }
}
