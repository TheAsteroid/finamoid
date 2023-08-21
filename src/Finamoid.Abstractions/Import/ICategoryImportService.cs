namespace Finamoid.Import
{
    public interface ICategoryImportService
    {
        Task ImportAndStoreAsync(string fullPath);
    }
}
