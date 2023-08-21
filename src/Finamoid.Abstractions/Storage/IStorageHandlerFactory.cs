namespace Finamoid.Storage
{
    public interface IStorageHandlerFactory
    {
        IStorageHandler Get(StorageType storageType);
    }
}
