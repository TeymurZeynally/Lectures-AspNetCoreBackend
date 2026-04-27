using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Lecture13.Auth.JWT.Full.Api.Account.Services
{
    internal class Argon2PasswordHashService : IPasswordHashService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;

        private const int DegreeOfParallelism = 4;
        private const int MemorySize = 65536;
        private const int Iterations = 3;

        public (byte[] HashBytes, byte[] SaltBytes) HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = HashPassword(password, salt);

            return (hash, salt);
        }

        public bool VerifyPassword(string password, byte[] hashBytes, byte[] saltBytes)
        {
            var actualHash = HashPassword(password, saltBytes);

            return CryptographicOperations.FixedTimeEquals(actualHash, hashBytes);
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                MemorySize = MemorySize,
                Iterations = Iterations
            };

            return argon2.GetBytes(HashSize);
        }
    }
}
