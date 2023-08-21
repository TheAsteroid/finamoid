using Finamoid.Storage;
using Finamoid.Utils;
using Microsoft.Extensions.Options;

namespace Finamoid.Import
{
    internal class MutationImportService : IMutationImportService
    {
        private readonly IBankStatementFormatDetector _bankStatementFormatDetector;
        private readonly IRawMutationReaderFactory _rawMutationReaderFactory;
        private readonly IMutationWriter _mutationWriter;
        private readonly StorageOptions _storageOptions;

        public MutationImportService(
            IBankStatementFormatDetector bankStatementFormatDetector,
            IRawMutationReaderFactory rawMutationReaderFactory,
            IMutationWriter mutationWriter,
            IOptions<StorageOptions> storageOptions)
        {
            _bankStatementFormatDetector = bankStatementFormatDetector;
            _rawMutationReaderFactory = rawMutationReaderFactory;
            _mutationWriter = mutationWriter;
            _storageOptions = storageOptions.Value;
        }

        public async Task<int> ImportAsync(string path)
        {
            var fileCount = 0;

            async Task<IEnumerable<Mutation>> getMutations(string path)
            {
                fileCount++;

                var bankStatementType = await _bankStatementFormatDetector.DetectAsync(path);

                if (bankStatementType == BankStatementType.Undefined)
                {
                    throw new NotSupportedException($"Could not determine {nameof(BankStatementType)} of file {path}.");
                }

                return await _rawMutationReaderFactory.Get(bankStatementType).ReadAsync(path);
            }

            var mutations = new List<Mutation>();

            if (File.Exists(path))
            {
                mutations.AddRange(await getMutations(path));
            }
            else if (Directory.Exists(path))
            {
                foreach (var filePath in Directory.EnumerateFiles(path, $"*", SearchOption.AllDirectories))
                {
                    mutations.AddRange(await getMutations(filePath));
                }
            }
            else
            {
                throw new FileNotFoundException("Given path does not point to a file or directory.", path);
            }

            await _mutationWriter.WriteAsync(mutations, _storageOptions.PeriodType);

            return fileCount;
        }
    }
}
