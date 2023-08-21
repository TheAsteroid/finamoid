using CsvHelper.Configuration;
using Finamoid.Mutations;
using System.Globalization;

namespace Finamoid.Import.Mutations
{
    internal class IngSsvMutationImporter : IngCsvMutationImporter
    {
        protected override CsvConfiguration CsvConfiguration { get; } = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        public override Task<IEnumerable<Mutation>> ReadAsync(string fullPath)
        {
            throw new NotImplementedException();
        }
    }
}
