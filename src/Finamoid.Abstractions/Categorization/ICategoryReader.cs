namespace Finamoid.Categorization
{
    public interface ICategoryReader
    {
        Task<IEnumerable<Category>> ReadAsync(string relativePath);
    }
}
