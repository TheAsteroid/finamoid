namespace Finamoid.Abstractions.Categorization
{
    public record CategoryFilter(string? PartialMatch, string? ExactMatch, string? AccountNumber);
}
