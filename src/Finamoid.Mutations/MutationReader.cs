﻿using Finamoid.Mutations;
using Finamoid.Storage;
using Finamoid.Utils;
using Newtonsoft.Json;

namespace Finamoid.Import.Readers
{
    internal class MutationReader : IMutationReader
    {
        private readonly IStorageHandler _storageHandler;

        public MutationReader(IStorageHandlerFactory storageHandlerFactory)
        {
            _storageHandler = storageHandlerFactory.Get(StorageType.Mutations);
        }

        public async Task<IEnumerable<Mutation>> ReadAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var result = new List<Mutation>();

            // Get files that fall in the required range
            var matchedFiles = _storageHandler.EnumerateFiles(
                string.Empty,
                $"*{Constants.DataFileExtension}",
                SearchOption.AllDirectories)
                .Where(f => FileHelper.FileNameIsInPeriod(f, startDate, endDate));

            foreach (var fileName in matchedFiles)
            {
                result.AddRange(await ReadAsync(fileName));
            }

            // Some mutations in the matched files might still fall outside the required range, filter these out
            return result.Where(c =>
                (startDate == null || c.DateTime.Date >= startDate.Value.Date) &&
                (endDate == null || c.DateTime.Date <= endDate.Value.Date));
        }

        public async Task<IEnumerable<Mutation>> ReadAsync(string relativePath)
        {
            var data = await _storageHandler.ReadAllTextAsync(relativePath);

            return JsonConvert.DeserializeObject<IEnumerable<Mutation>>(data) ?? Enumerable.Empty<Mutation>();
        }
    }
}
