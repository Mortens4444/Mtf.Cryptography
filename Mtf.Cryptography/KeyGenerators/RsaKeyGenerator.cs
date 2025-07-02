using System;
using System.IO;
using System.Security.Cryptography;

namespace Mtf.Cryptography.KeyGenerators
{
    public static class RsaKeyGenerator
    {
        public static void GenerateKeyFile(string keyFilePath, int keySize = 2048, bool includePrivateParameters = false)
        {
            using (GenerateKeyAndFile(keyFilePath, keySize, includePrivateParameters)) { }
        }

        public static void GenerateKeyFile(RSACng rsaCng, string keyFilePath, bool includePrivateParameters = false)
        {
            if (rsaCng == null)
            {
                throw new ArgumentNullException(nameof(rsaCng));
            }
            var xml = rsaCng.ToXmlString(includePrivateParameters);
            File.WriteAllText(keyFilePath, xml);
        }

        public static void GenerateKeyFiles(string privateAndPublicKeyFilePath, string publicKeyFilePath, int keySize = 2048, bool includePrivateParameters = false)
        {
            using (var rsaCng = GenerateKeyAndFile(privateAndPublicKeyFilePath, keySize, true))
            {
                GenerateKeyFile(rsaCng, publicKeyFilePath, includePrivateParameters);
            }
        }

        public static RSACng GenerateKey(int keySize = 2048)
        {
            return new RSACng { KeySize = keySize };
        }

        public static RSACng GenerateKeyAndFile(string keyFilePath, int keySize = 2048, bool includePrivateParameters = false)
        {
            if (File.Exists(keyFilePath))
            {
                throw new InvalidOperationException(String.Concat("File already exists: ", keyFilePath));
            }

            var rsaCng = GenerateKey(keySize);
            GenerateKeyFile(rsaCng, keyFilePath, includePrivateParameters);
            return rsaCng;
        }
    }
}
