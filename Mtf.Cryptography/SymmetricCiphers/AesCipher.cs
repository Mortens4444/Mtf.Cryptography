using Mtf.Cryptography.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mtf.Cryptography.SymmetricCiphers
{
    public class AesCipher : ICipher, IDisposable
    {
        private readonly Aes aes;

        public AesCipher(byte[] key, byte[] iv)
        {
            aes = Aes.Create();
            aes.Key = key ?? throw new ArgumentNullException(nameof(key));
            aes.IV = iv ?? throw new ArgumentNullException(nameof(iv));
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
        }

        public string Encrypt(string plainText)
        {
            if (String.IsNullOrEmpty(plainText)) return plainText;
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = Encrypt(plainBytes);
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText)) return cipherText;
            var cipherBytes = Convert.FromBase64String(cipherText);
            var decrypted = Decrypt(cipherBytes);
            return Encoding.UTF8.GetString(decrypted);
        }

        public byte[] Encrypt(byte[] plainBytes)
        {
            using (var encryptor = aes.CreateEncryptor())
            {
                return PerformCryptography(plainBytes, encryptor);
            }
        }

        public byte[] Decrypt(byte[] cipherBytes)
        {
            using (var decryptor = aes.CreateDecryptor())
            {
                return PerformCryptography(cipherBytes, decryptor);
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform transform)
        {
            if (data == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            aes?.Dispose();
        }
    }
}
