using Finamoid.Mutations;
using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Finamoid.Import.Writers
{
    internal class MutationWriter : IMutationWriter
    {
        private readonly IStorageHandlerFactory _storageHandlerFactory;
        private readonly IMutationReader _mutationReader;

        public MutationWriter(IStorageHandlerFactory storageHandlerFactory, IMutationReader mutationReader)
        {
            _storageHandlerFactory = storageHandlerFactory;
            _mutationReader = mutationReader;
        }

        public async Task WriteAsync(IEnumerable<Mutation> mutations, PeriodType periodType)
        {
            var mutationsPerFile = new ConcurrentDictionary<string, List<Mutation>>();

            foreach (var dateGroup in mutations.GroupBy(m => m.DateTime))
            {
                var fileNameDatePart = FileHelper.GetFileNameDatePart(dateGroup.Key, periodType);

                mutationsPerFile.AddOrUpdate(
                    fileNameDatePart,
                    (_) => dateGroup.ToList(),
                    (_, existing) =>
                    {
                        existing.AddRange(dateGroup);
                        return existing;
                    });
            }

            var storageHandler = _storageHandlerFactory.Get(StorageType.Mutations);
            foreach (var file in mutationsPerFile)
            {
                var fileName = $"{file.Key}{Constants.DataFileExtension}";

                var mutationsToWrite = file.Value;
                if (storageHandler.FileExists(fileName))
                {
                    mutationsToWrite.AddRange(await _mutationReader.ReadAsync(fileName));
                }

                // Filter out duplicate entries
                mutationsToWrite = mutationsToWrite.DistinctBy(m => m.Id).ToList();

                await storageHandler.WriteAsync(
                    fileName,
                    JsonConvert.SerializeObject(
                        mutationsToWrite.OrderBy(m => m.DateTime), 
                        Constants.IndentJson ? Formatting.Indented : Formatting.None));
            }
        }
    }
}
