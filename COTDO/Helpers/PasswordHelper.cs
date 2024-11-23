using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace COTDO.Helpers
{
    public class PasswordHelper
    {
        private const int SaltSize = 16; // Tamaño en bytes
        private const int HashSize = 32; // Tamaño en bytes
        private const int Iterations = 20000;

        /// <summary>
        /// Genera un hash seguro para la contraseña proporcionada.
        /// </summary>
        /// <param name="password">Contraseña a hashear.</param>
        /// <returns>Cadena segura en formato 'salt:hash'.</returns>
        public string HashPassword(string password)
        {
            // Generar una sal aleatoria
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Generar el hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Combinar sal y hash en una cadena segura
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        /// <summary>
        /// Verifica si una contraseña coincide con el hash almacenado.
        /// </summary>
        /// <param name="password">Contraseña a verificar.</param>
        /// <param name="storedHash">Hash almacenado en formato 'salt:hash'.</param>
        /// <returns>True si coincide, de lo contrario False.</returns>
        public bool VerifyPassword(string password, string storedHash)
        {
            // Separar la sal y el hash
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedPasswordHash = Convert.FromBase64String(parts[1]);

            // Generar el hash con la contraseña proporcionada y la sal almacenada
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] computedHash = pbkdf2.GetBytes(HashSize);

                // Comparar los hashes
                return SlowEquals(computedHash, storedPasswordHash);
            }
        }

        /// <summary>
        /// Compara dos arreglos de bytes de forma segura para evitar ataques de tiempo.
        /// </summary>
        /// <param name="a">Primer arreglo de bytes.</param>
        /// <param name="b">Segundo arreglo de bytes.</param>
        /// <returns>True si son iguales, False en caso contrario.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}