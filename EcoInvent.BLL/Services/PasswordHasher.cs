using System;
using System.Security.Cryptography;

namespace EcoInvent.BLL.Services
{
    public static class PasswordHasher
    {
        public static string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                100_000,
                HashAlgorithmName.SHA256,
                32);

            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string salt, string storedHash)
        {
            string computedHash = HashPassword(password, salt);
            return computedHash == storedHash;
        }
    }
}