namespace Finamoid.Abstractions
{
    public record Mutation(
        string Id,
        DateTime DateTime,
        CurrencyAmount Amount,
        string? Name = null,
        string? Description = null,
        string? Reference = null,
        string? AccountNumber = null,
        MutationType? MutationType = null);
}
