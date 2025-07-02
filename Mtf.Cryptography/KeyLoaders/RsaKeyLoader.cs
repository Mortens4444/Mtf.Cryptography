using System;
using System.IO;
using System.Security.Cryptography;

namespace Mtf.Cryptography.KeyLoaders
{
    public static class RsaKeyLoader
    {
        /// <summary>
        /// Loads RSAParameters from a file (supports XML).
        /// </summary>
        /// <param name="filePath">Path to the key file.</param>
        /// <param name="includePrivateParameters">Whether to include private parameters to key or not.</param>
        /// <returns>RSAParameters loaded from the file.</returns>
        public static RSAParameters LoadRsaParametersFromXml(string filePath, bool includePrivateParameters)
        {
            var xml = File.ReadAllText(filePath);
            if (!xml.TrimStart().StartsWith("<", StringComparison.InvariantCulture))
            {
                throw new InvalidOperationException("Unsupported key format. Expected XML.");
            }

            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(xml);
                return rsa.ExportParameters(includePrivateParameters);
            }
        }
    }
}
