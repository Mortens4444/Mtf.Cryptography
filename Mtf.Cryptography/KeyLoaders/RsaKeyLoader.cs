using System;
using System.Security.Cryptography;
using System.Xml;

namespace Mtf.Cryptography.KeyLoaders
{
    /// <summary>
    /// A stateless, modern helper to load RSA parameters directly from an XML file.
    /// </summary>
    public static class RsaKeyLoader
    {
        /// <summary>
        /// Loads RSA parameters by manually parsing the standard RSA XML key format.
        /// This avoids issues with legacy cryptographic providers and non-exportable keys.
        /// </summary>
        /// <param name="filePath">The path to the XML key file.</param>
        /// <param name="includePrivateParameters">Whether to load the private key components.</param>
        /// <returns>An RSAParameters struct containing the key data.</returns>
        /// <exception cref="CryptographicException">Thrown if the XML file is not a valid RSA key.</exception>
        public static RSAParameters LoadRsaParametersFromXml(string filePath, bool includePrivateParameters)
        {
            var parameters = new RSAParameters();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            if (xmlDoc.DocumentElement?.Name != "RSAKeyValue")
            {
                throw new CryptographicException("Invalid RSA XML key file format.");
            }

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Modulus":
                        parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                    case "Exponent":
                        parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                    // Only load private parameters if requested
                    case "P":
                        if (includePrivateParameters)
                        {
                            parameters.P = Convert.FromBase64String(node.InnerText);
                        }

                        break;
                    case "Q":
                        if (includePrivateParameters)
                        {
                            parameters.Q = Convert.FromBase64String(node.InnerText);
                        }

                        break;
                    case "DP":
                        if (includePrivateParameters)
                        {
                            parameters.DP = Convert.FromBase64String(node.InnerText);
                        }

                        break;
                    case "DQ":
                        if (includePrivateParameters)
                        {
                            parameters.DQ = Convert.FromBase64String(node.InnerText);
                        }

                        break;
                    case "InverseQ":
                        if (includePrivateParameters)
                        {
                            parameters.InverseQ = Convert.FromBase64String(node.InnerText);
                        }

                        break;
                    case "D":
                        if (includePrivateParameters)
                        {
                            parameters.D = Convert.FromBase64String(node.InnerText);
                        }

                        break;
                }
            }
            return parameters;
        }
    }
}
