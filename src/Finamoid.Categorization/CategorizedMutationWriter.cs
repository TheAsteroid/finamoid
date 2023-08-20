using Finamoid.Abstractions;
using Finamoid.Abstractions.Categorization;
using Finamoid.Abstractions.FileHandling;
using Finamoid.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Finamoid.Categorization
{
    public class CategorizedMutationWriter : ICategorizedMutationWriter
    {
        private readonly IFileWriter _fileWriter;
        private readonly ICategorizedMutationReader _categorizedMutationReader;

        public CategorizedMutationWriter(IFileWriter fileWriter, ICategorizedMutationReader categorizedMutationReader)
        {
            _fileWriter = fileWriter;
            _categorizedMutationReader = categorizedMutationReader;
        }

        public async Task WriteAsync(string directory, IEnumerable<CategorizedMutation> categorizedMutations, PeriodType periodType)
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
                var fileName = Path.Combine(directory, $"{file.Key}{Constants.DataFileExtension}");

                var mutationsToWrite = file.Value;
                if (File.Exists(fileName))
                {
                    var existingMutations = await _categorizedMutationReader.ReadFromFileAsync(fileName);

                    // Overwrite existing mutations with new mutations,
                    // but make sure to keep existing mutations that are not in the new mutations.
                    mutationsToWrite.AddRange(existingMutations.Where(e => !mutationsToWrite.Any(m => m.Id == e.Id)));
                }

                await _fileWriter.WriteAsync(
                    fileName,
                    JsonConvert.SerializeObject(mutationsToWrite, Constants.IndentJson ? Formatting.Indented : Formatting.None));
            }
        }
    }
}
