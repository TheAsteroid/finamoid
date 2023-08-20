namespace Finamoid.Abstractions.Categorization
{
    public interface ICategoryWriter
    {
        Task WriteAsync(string path, IEnumerable<Category> categories);
    }
}
