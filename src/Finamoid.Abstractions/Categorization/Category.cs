﻿namespace Finamoid.Abstractions.Categorization
{
    public record Category(
        string Code,
        string Name,
        string MainCategory,
        BalanceType BalanceType,
        IEnumerable<CategoryFilter> Filters);
}
