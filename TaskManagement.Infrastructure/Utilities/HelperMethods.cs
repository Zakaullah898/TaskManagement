using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Entities;
namespace TaskManagement.Infrastructure.Utilities
{
    public class HelperMethods : IHelperMethods
    {
        public PasswordHashResult HashPassword(string password)
        {
            // Implement a simple hashing mechanism for demonstration purposes
            // In a real-world application, use a secure hashing algorithm like BCrypt or PBKDF2
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            var result = new PasswordHashResult
            {
                Hash = hash,
                Salt = Convert.ToBase64String(salt)
            };
            return result;
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hash == storedHash;
        }

        //public string GenerateOtp()
        //{
        //    Random rnd = new Random();
        //    return rnd.Next(100000, 999999).ToString(); // 6-digit OTP
        //}
        public string GenerateSecureOtp()
        {
            byte[] bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);

            int value = BitConverter.ToInt32(bytes, 0) & 0x7fffffff;
            return (value % 1000000).ToString("D6"); // 6 digit
        }

        public string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(bytes);
        }
    }
}
