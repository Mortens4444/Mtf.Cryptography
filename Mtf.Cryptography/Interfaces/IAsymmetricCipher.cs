using System.Security.Cryptography;

namespace Mtf.Cryptography.Interfaces
{
    public interface IAsymmetricCipher : ICipher
    {
        RSAParameters PublicKeyParameters { get; }
    }
}
