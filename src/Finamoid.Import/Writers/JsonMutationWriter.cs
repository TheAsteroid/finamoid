using Finamoid.Abstractions;
using Finamoid.Abstractions.FileHandling;
using Finamoid.Abstractions.Import;
using Finamoid.Import.Readers;
using Finamoid.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Finamoid.Import.Writers
{
    public class JsonMutationWriter : IMutationWriter
    {
        private readonly IFileWriter _fileWriter;
        private readonly IJsonMutationReader _jsonMutationReader;

        public JsonMutationWriter(IFileWriter fileWriter, IJsonMutationReader jsonMutationReader)
        {
            _fileWriter = fileWriter;
            _jsonMutationReader = jsonMutationReader;
        }

        public async Task WriteAsync(string directory, IEnumerable<Mutation> mutations, PeriodType periodType)
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

            foreach (var file in mutationsPerFile)
            {
                var fileName = Path.Combine(directory, $"{file.Key}{Constants.DataFileExtension}");

                var mutationsToWrite = file.Value;
                if (File.Exists(fileName))
                {
                    mutationsToWrite.AddRange(await _jsonMutationReader.ReadFromFileAsync(fileName));
                }

                // Filter out duplicate entries
                mutationsToWrite = mutationsToWrite.DistinctBy(m => m.Id).ToList();

                await _fileWriter.WriteAsync(
                    fileName,
                    JsonConvert.SerializeObject(mutationsToWrite, Constants.IndentJson ? Formatting.Indented : Formatting.None));
            }
        }
    }
}
