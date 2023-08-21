using CsvHelper.Configuration;
using Finamoid;
using System.Globalization;

namespace Finamoid.Import.Readers
{
    internal class IngSsvMutationReader : IngCsvMutationReader
    {
        protected override CsvConfiguration CsvConfiguration { get; } = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        public override Task<IEnumerable<Mutation>> ReadAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
