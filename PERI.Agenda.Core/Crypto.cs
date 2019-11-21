using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PERI.Agenda.Core
{
    public static class Crypto
    {
        /// <summary>
        /// Hash an ordinary string via salt
        /// </summary>
        /// <param name="text"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Hash(string text, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: text,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 80000,
                numBytesRequested: 256 / 8));
        }

        /// <summary>
        /// Generates random salt
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSalt()
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
    }
}
