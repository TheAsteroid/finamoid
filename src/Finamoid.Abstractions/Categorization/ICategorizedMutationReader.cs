﻿namespace Finamoid.Categorization.CategorizedMutations
{
    public interface ICategorizedMutationReader
    {
        Task<IEnumerable<CategorizedMutation>> ReadAsync(DateTime? startDate = null, DateTime? endDate = null);

        Task<IEnumerable<CategorizedMutation>> ReadAsync(string relativePath);
    }
}
