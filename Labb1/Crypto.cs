using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lektion2Grupp
{
    public class Crypto
    {
        private static readonly string password = "secret-password";

        public static Tuple<string, int> Encrypt(string text)
        {
            Random random = new Random();
            byte[] salt = new byte[random.Next(8, 33)];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var passwordBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                using (Aes encryptor = Aes.Create())
                {
                    encryptor.Key = passwordBytes.GetBytes(32);
                    encryptor.IV = passwordBytes.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(salt, 0, salt.Length);

                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] textBytes = Encoding.UTF8.GetBytes(text);
                            cs.Write(textBytes, 0, textBytes.Length);
                            cs.FlushFinalBlock();
                        }

                        return Tuple.Create(Convert.ToBase64String(ms.ToArray()), salt.Length);
                    }
                }
            }
        }

        public static string Decrypt(string encrypted, int saltLength)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);

            byte[] salt = new byte[saltLength];
            Array.Copy(encryptedBytes, 0, salt, 0, salt.Length);

            byte[] cipherBytes = new byte[encryptedBytes.Length - salt.Length];
            Array.Copy(encryptedBytes, salt.Length, cipherBytes, 0, cipherBytes.Length);

            using (var passwordBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                using (Aes encryptor = Aes.Create())
                {
                    encryptor.Key = passwordBytes.GetBytes(32);
                    encryptor.IV = passwordBytes.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.FlushFinalBlock();
                        }

                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}
