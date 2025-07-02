using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mtf.Cryptography.Converters
{
    public static class RsaParametersConverter
    {
        public static RSAParameters ToRSAParameters(byte[] publicKeyData)
        {
            using (var ms = new MemoryStream(publicKeyData))
            using (var reader = new BinaryReader(ms))
            {
                var modulusLength = reader.ReadInt32();
                var modulus = reader.ReadBytes(modulusLength);
                var exponentLength = reader.ReadInt32();
                var exponent = reader.ReadBytes(exponentLength);
                return new RSAParameters { Modulus = modulus, Exponent = exponent };
            }
        }

        public static byte[] ToByteArray(RSAParameters parameters)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(parameters.Modulus?.Length ?? 0);
                if (parameters.Modulus != null)
                {
                    writer.Write(parameters.Modulus);
                }

                writer.Write(parameters.Exponent?.Length ?? 0);
                if (parameters.Exponent != null)
                {
                    writer.Write(parameters.Exponent);
                }

                return ms.ToArray();
            }
        }
    }
}
