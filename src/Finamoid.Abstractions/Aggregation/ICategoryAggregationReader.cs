namespace Finamoid.Aggregation
{
    public interface ICategoryAggregationReader
    {
        Task<IEnumerable<CategoryAggregation>> ReadAsync(string relativePath);
    }
}
