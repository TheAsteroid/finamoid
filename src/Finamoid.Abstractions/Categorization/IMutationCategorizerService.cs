namespace Finamoid.Categorization
{
    public interface IMutationCategorizerService
    {
        Task CategorizeAndStoreAsync(DateTime? startDate, DateTime? endDate);
    }
}
