using System;
using System.IO;
using System.Security.Cryptography;

namespace Mtf.Cryptography.KeyGenerators
{
    public static class AesKeyGenerator
    {
        public static void GenerateKeyFile(string keyFilePath, string ivFilePath, int keySizeBits = 256)
        {
            if (File.Exists(keyFilePath))
            {
                throw new IOException($"Key file already exists: {keyFilePath}");
            }

            if (File.Exists(ivFilePath))
            {
                throw new IOException($"IV file already exists: {ivFilePath}");
            }

            using (var aes = Aes.Create())
            {
                aes.KeySize = keySizeBits;
                aes.GenerateKey();
                aes.GenerateIV();

                File.WriteAllBytes(keyFilePath, aes.Key);
                File.WriteAllBytes(ivFilePath, aes.IV);
            }
        }

        public static Tuple<byte[], byte[]> GenerateKey(int keySizeBits = 256)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = keySizeBits;
                aes.GenerateKey();
                aes.GenerateIV();
                return new Tuple<byte[], byte[]>(aes.Key, aes.IV);
            }
        }
    }
}
