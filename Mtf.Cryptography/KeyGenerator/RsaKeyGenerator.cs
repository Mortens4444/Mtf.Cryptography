using System;
using System.IO;
using System.Security.Cryptography;

namespace Mtf.Cryptography.KeyGenerator
{
    public static class RsaKeyGenerator
    {
        public static void GenerateKeyFiles(string privateAndPublicKeyFilePath, string publicKeyFilePath, int keySize = 2048)
        {
            if (File.Exists(privateAndPublicKeyFilePath))
            {
                throw new InvalidOperationException(String.Concat("File already exists: ", privateAndPublicKeyFilePath));
            }
            if (File.Exists(publicKeyFilePath))
            {
                throw new InvalidOperationException(String.Concat("File already exists: ", publicKeyFilePath));
            }

            using (var rsaInstance = new RSACng { KeySize = keySize })
            {
                var xml = rsaInstance.ToXmlString(true);
                File.WriteAllText(privateAndPublicKeyFilePath, xml);

                var publicKeyXml = rsaInstance.ToXmlString(false);
                File.WriteAllText(publicKeyFilePath, publicKeyXml);
            }
        }
    }
}
