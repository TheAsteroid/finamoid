namespace Finamoid.Abstractions.Encryption
{
    public interface IEncryptionConfiguration
    {
        bool EncryptCategories { get; }

        bool EncryptImportedMutations { get; }

        bool EncryptAggregations { get; }

        string EncryptionKeyDirectory { get; }
    }
}
