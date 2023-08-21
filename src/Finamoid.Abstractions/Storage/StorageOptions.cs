namespace Finamoid.Storage
{
    public class StorageOptions
    {
        public bool EncryptCategories { get; init; } = true;

        public bool EncryptCategorizedMutations { get; init; } = true;

        public bool EncryptMutations { get; init; } = true;

        public bool EncryptAggregations { get; init; } = true;

        public PeriodType PeriodType { get; init; } = PeriodType.Month;

        public string? RootDirectory { get; init; } =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create),
                "Finamoid");
    }
}
