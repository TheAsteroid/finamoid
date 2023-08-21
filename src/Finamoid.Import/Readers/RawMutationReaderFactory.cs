using Finamoid.Import;
using System.Collections.ObjectModel;

namespace Finamoid.Import.Readers
{
    internal class RawMutationReaderFactory : IRawMutationReaderFactory
    {
        private readonly ReadOnlyDictionary<BankStatementType, Lazy<IRawMutationReader>> _mutationReaders;

        public RawMutationReaderFactory()
        {
            _mutationReaders = new ReadOnlyDictionary<BankStatementType, Lazy<IRawMutationReader>>(
                new Dictionary<BankStatementType, Lazy<IRawMutationReader>>
                {
                    { BankStatementType.AsnCsv, new(() => new AsnCsvMutationReader()) },
                    { BankStatementType.Camt053, new(()=> new Camt053MutationReader()) },
                    { BankStatementType.IngCsv, new (() => new IngCsvMutationReader()) },
                    { BankStatementType.IngSsv, new(() => new IngSsvMutationReader()) }
                });
        }

        public IRawMutationReader Get(BankStatementType bankStatementType)
        {
            if (!_mutationReaders.ContainsKey(bankStatementType))
            {
                throw new NotSupportedException($"Could not find MutationReader for {nameof(BankStatementType)} {bankStatementType}");
            }

            return _mutationReaders[bankStatementType].Value;
        }
    }
}
