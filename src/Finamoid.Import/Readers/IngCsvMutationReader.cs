using CsvHelper.Configuration;
using Finamoid.Abstractions;
using System.Globalization;

namespace Finamoid.Import.Readers
{
    public class IngCsvMutationReader : MutationReader
    {
        protected virtual CsvConfiguration CsvConfiguration { get; } = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ","
        };

        public override Task<IEnumerable<Mutation>> ReadFromFileAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
