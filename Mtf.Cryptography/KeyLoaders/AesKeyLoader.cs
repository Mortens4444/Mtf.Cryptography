using System;
using System.IO;

namespace Mtf.Cryptography.KeyLoaders
{
    public static class AesKeyLoader
    {
        public static Tuple<byte[], byte[]> LoadKeyAndIV(string keyFilePath, string ivFilePath)
        {
            if (!File.Exists(keyFilePath)) throw new FileNotFoundException("Key file not found.", keyFilePath);
            if (!File.Exists(ivFilePath)) throw new FileNotFoundException("IV file not found.", ivFilePath);

            var key = File.ReadAllBytes(keyFilePath);
            var iv = File.ReadAllBytes(ivFilePath);

            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new InvalidOperationException("Invalid AES key length.");

            if (iv.Length != 16)
                throw new InvalidOperationException("Invalid AES IV length (must be 16 bytes).");

            return new Tuple<byte[], byte[]>(key, iv);
        }
    }
}
