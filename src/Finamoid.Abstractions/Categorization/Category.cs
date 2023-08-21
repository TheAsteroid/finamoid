namespace Finamoid.Categorization
{
    public record Category(
        string Code,
        string Name,
        string? MainCategory,
        BalanceType BalanceType,
        bool IsWildcard,
        IEnumerable<CategoryFilter> Filters);
}
