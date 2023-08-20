using Finamoid.Abstractions.Categorization;
using Finamoid.Abstractions.FileHandling;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Categorization
{
    public class CategorizedMutationReader : ICategorizedMutationReader
    {
        private readonly IFileReader _fileReader;

        public CategorizedMutationReader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public async Task<IEnumerable<CategorizedMutation>> ReadFromDirectoryAsync(
            string directory,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var result = new List<CategorizedMutation>();

            // Get files that fall in the required range
            var matchedFiles = Directory.EnumerateFiles(
                directory,
                "*",
                SearchOption.AllDirectories)
                .Where(f => FileHelper.FileNameIsInPeriod(f, startDate, endDate));

            foreach (var fileName in matchedFiles)
            {
                result.AddRange(await ReadFromFileAsync(fileName));
            }

            // Some mutations in the matched files might still fall outside the required range, filter these out
            return result.Where(c =>
                (startDate == null || c.DateTime.Date >= startDate.Value.Date) &&
                (endDate == null || c.DateTime.Date <= endDate.Value.Date));
        }

        public async Task<IEnumerable<CategorizedMutation>> ReadFromFileAsync(string path)
        {
            var data = await _fileReader.ReadAsync(path);

            return
                JsonConvert.DeserializeObject<IEnumerable<CategorizedMutation>>(data) ??
                Enumerable.Empty<CategorizedMutation>();
        }
    }
}
