using System.Security.Cryptography;

namespace Mtf.Cryptography.Extensions
{
    public static class RSAParametersExtensions
    {
        public static RSAParameters ToPublicKey(this RSAParameters rsaParameters)
        {
            return new RSAParameters
            {
                Modulus = rsaParameters.Modulus,
                Exponent = rsaParameters.Exponent
            };
        }
    }
}
