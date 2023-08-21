namespace Finamoid.Categorization
{
    public interface ICategoryWriter
    {
        Task WriteAsync(string relativePath, IEnumerable<Category> categories);
    }
}
