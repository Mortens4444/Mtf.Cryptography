using System.Security.Cryptography;

namespace Mtf.Cryptography.Interfaces
{
    internal interface IRsaCipher : IAsymmetricCipher
    {
        RSAParameters PublicKeyParameters { get; }
    }
}
