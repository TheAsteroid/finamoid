using Finamoid.Mutations;
using Finamoid.Storage;
using Microsoft.Extensions.Options;

namespace Finamoid.Import.Mutations
{
    internal class MutationImportService : IMutationImportService
    {
        private readonly IBankStatementFormatDetector _bankStatementFormatDetector;
        private readonly IMutationImporterFactory _mutationImporterFactory;
        private readonly IMutationWriter _mutationWriter;
        private readonly StorageOptions _storageOptions;

        public MutationImportService(
            IBankStatementFormatDetector bankStatementFormatDetector,
            IMutationImporterFactory mutationImporterFactory,
            IMutationWriter mutationWriter,
            IOptions<StorageOptions> storageOptions)
        {
            _bankStatementFormatDetector = bankStatementFormatDetector;
            _mutationImporterFactory = mutationImporterFactory;
            _mutationWriter = mutationWriter;
            _storageOptions = storageOptions.Value;
        }

        public async Task<int> ImportAndStoreAsync(string fullPath)
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

                return await _mutationImporterFactory.Get(bankStatementType).ReadAsync(path);
            }

            var mutations = new List<Mutation>();

            if (File.Exists(fullPath))
            {
                mutations.AddRange(await getMutations(fullPath));
            }
            else if (Directory.Exists(fullPath))
            {
                foreach (var filePath in Directory.EnumerateFiles(fullPath, $"*", SearchOption.AllDirectories))
                {
                    mutations.AddRange(await getMutations(filePath));
                }
            }
            else
            {
                throw new FileNotFoundException("Given path does not point to a file or directory.", fullPath);
            }

            await _mutationWriter.WriteAsync(mutations, _storageOptions.PeriodType);

            return fileCount;
        }
    }
}
