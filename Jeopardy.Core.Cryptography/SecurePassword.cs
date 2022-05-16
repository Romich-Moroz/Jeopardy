using System.Security.Cryptography;

namespace Jeopardy.Core.Cryptography
{
    public class SecurePassword
    {
        private const int SaltSize = 128;
        private const int HashSize = 63;
        private const int Iterations = 100000;

        public byte[] Salt { get; private set; }
        public string PasswordHash { get; private set; }

        public SecurePassword(string password, byte[]? salt = null)
        {
            Salt = salt ?? RandomNumberGenerator.GetBytes(SaltSize);
            var hash = new Rfc2898DeriveBytes(password, Salt, Iterations).GetBytes(HashSize);
            PasswordHash = Convert.ToBase64String(hash);
        }

        public bool Verify(string passwordHash) => PasswordHash == passwordHash;
    }
}
