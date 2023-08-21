using System.Security.Cryptography;
using System.Text;

namespace Finamoid.Encryption
{
    public class PasswordProvider : IPasswordProvider
    {
        private byte[]? _passwordKey;

        public byte[] GetPasswordKey()
        {
            return _passwordKey ?? throw new InvalidOperationException("Password not set.");
        }

        public void SetPassword(string password, byte[] salt)
        {
            _passwordKey = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                1000,
                HashAlgorithmName.SHA512,
                32);
        }
    }
}
