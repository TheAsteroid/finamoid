namespace Finamoid.Categorization.Categories
{
    internal interface ICategoryReader
    {
        Task<IEnumerable<Category>> ReadAsync(string relativePath);

        Task<IEnumerable<Category>> ReadLatestAsync();
    }
}
