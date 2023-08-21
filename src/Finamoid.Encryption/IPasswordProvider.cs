namespace Finamoid.Encryption
{
    public interface IPasswordProvider
    {
        byte[] GetPasswordKey();

        void SetPassword(string password, byte[] salt);
    }
}
