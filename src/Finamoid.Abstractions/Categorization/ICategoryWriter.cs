namespace Finamoid.Categorization
{
    public interface ICategoryWriter
    {
        Task WriteAsync(IEnumerable<Category> categories);
    }
}
