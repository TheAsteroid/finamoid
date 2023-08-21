using System.Collections.ObjectModel;

namespace Finamoid.Import.Mutations
{
    internal class MutationImporterFactory : IMutationImporterFactory
    {
        private readonly ReadOnlyDictionary<BankStatementType, Lazy<IMutationImporter>> _mutationReaders;

        public MutationImporterFactory()
        {
            _mutationReaders = new ReadOnlyDictionary<BankStatementType, Lazy<IMutationImporter>>(
                new Dictionary<BankStatementType, Lazy<IMutationImporter>>
                {
                    { BankStatementType.AsnCsv, new(() => new AsnCsvMutationImporter()) },
                    { BankStatementType.Camt053, new(()=> new Camt053MutationImporter()) },
                    { BankStatementType.IngCsv, new (() => new IngCsvMutationImporter()) },
                    { BankStatementType.IngSsv, new(() => new IngSsvMutationImporter()) }
                });
        }

        public IMutationImporter Get(BankStatementType bankStatementType)
        {
            if (!_mutationReaders.ContainsKey(bankStatementType))
            {
                throw new NotSupportedException($"Could not find MutationReader for {nameof(BankStatementType)} {bankStatementType}");
            }

            return _mutationReaders[bankStatementType].Value;
        }
    }
}
