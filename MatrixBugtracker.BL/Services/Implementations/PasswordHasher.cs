using MatrixBugtracker.Abstractions;
using System.Security.Cryptography;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int Iterations = 10000;
        private const int SaltSize = 16;
        private const int HashSize = 32;


        public string HashPassword(string password)
        {
            using var random = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            random.GetBytes(salt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i]) return false;
            }

            return true;
        }
    }
}
