using CsvHelper.Configuration;
using Finamoid.Mutations;
using System.Globalization;

namespace Finamoid.Import.Mutations
{
    internal class IngCsvMutationImporter : MutationImporter
    {
        protected virtual CsvConfiguration CsvConfiguration { get; } = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ","
        };

        public override Task<IEnumerable<Mutation>> ReadAsync(string fullPath)
        {
            throw new NotImplementedException();
        }
    }
}
