namespace Finamoid.Abstractions.Categorization
{
    public interface ICategoryReader
    {
        Task<IEnumerable<Category>> ReadAsync(string path);
    }
}
