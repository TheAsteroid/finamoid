using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Finamoid.Categorization
{
    public class CategorizedMutationWriter : ICategorizedMutationWriter
    {
        private readonly IStorageHandler _storageHandler;
        private readonly ICategorizedMutationReader _categorizedMutationReader;

        public CategorizedMutationWriter(
            IStorageHandlerFactory storageHandlerFactory,
            ICategorizedMutationReader categorizedMutationReader)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.CategorizedMutations);
            _categorizedMutationReader = categorizedMutationReader;
        }

        public async Task WriteAsync(IEnumerable<CategorizedMutation> categorizedMutations, PeriodType periodType)
        {
            var mutationsPerFile = new ConcurrentDictionary<string, List<CategorizedMutation>>();

            foreach (var dateGroup in categorizedMutations.GroupBy(m => m.DateTime))
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

            foreach (var file in mutationsPerFile)
            {
                var fileName = $"{file.Key}{Constants.DataFileExtension}";

                var mutationsToWrite = file.Value;
                if (_storageHandler.FileExists(fileName))
                {
                    var existingMutations = await _categorizedMutationReader.ReadAsync(fileName);

                    // Overwrite existing mutations with new mutations,
                    // but make sure to keep existing mutations that are not in the new mutations.
                    mutationsToWrite.AddRange(existingMutations.Where(e => !mutationsToWrite.Any(m => m.Id == e.Id)));
                }

                await _storageHandler.WriteAsync(
                    fileName,
                    JsonConvert.SerializeObject(mutationsToWrite, Constants.IndentJson ? Formatting.Indented : Formatting.None));
            }
        }
    }
}
