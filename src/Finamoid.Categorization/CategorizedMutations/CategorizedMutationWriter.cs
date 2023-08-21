using Finamoid.Storage;
using Finamoid.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Finamoid.Categorization.CategorizedMutations
{
    internal class CategorizedMutationWriter : ICategorizedMutationWriter
    {
        private readonly IStorageHandler _storageHandler;
        private readonly StorageOptions _storageOptions;
        private readonly ICategorizedMutationReader _categorizedMutationReader;

        public CategorizedMutationWriter(
            IOptions<StorageOptions> storageOptions,
            IStorageHandlerFactory storageHandlerFactory,
            ICategorizedMutationReader categorizedMutationReader)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.CategorizedMutations);
            _storageOptions = storageOptions.Value;
            _categorizedMutationReader = categorizedMutationReader;
        }

        public async Task WriteAsync(IEnumerable<CategorizedMutation> categorizedMutations)
        {
            var mutationsPerFile = new ConcurrentDictionary<string, List<CategorizedMutation>>();

            var periodType = _storageOptions.PeriodType;
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
                    JsonConvert.SerializeObject(
                        mutationsToWrite.OrderBy(c => c.DateTime),
                        Constants.IndentJson ? Formatting.Indented : Formatting.None));
            }
        }
    }
}
