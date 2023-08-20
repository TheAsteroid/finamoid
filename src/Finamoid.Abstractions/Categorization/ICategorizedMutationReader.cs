namespace Finamoid.Abstractions.Categorization
{
    public interface ICategorizedMutationReader
    {
        Task<IEnumerable<CategorizedMutation>> ReadFromDirectoryAsync(
            string directory,
            DateTime? startDate = null,
            DateTime? endDate = null);

        Task<IEnumerable<CategorizedMutation>> ReadFromFileAsync(string directory);
    }
}
