using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NoTrick.Web {
    public static class PasswordHasher {
        
        public static Tuple<string, string> HashPassword(string password, string? salt = null) {
            if (salt == null) {
                var rng = RandomNumberGenerator.Create();
                var saltByte = new byte[64];
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);
            }
            var passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(Encoding.UTF8.GetBytes(password));
            passwordWithSaltBytes.AddRange(Convert.FromBase64String(salt));
            var hash = SHA256.Create().ComputeHash(passwordWithSaltBytes.ToArray());
            return new Tuple<string, string>(Convert.ToBase64String(hash), salt);
        } 
    }
}