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
    }
}
