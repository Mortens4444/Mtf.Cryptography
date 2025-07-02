namespace Mtf.Cryptography.Interfaces
{
    public interface IAsymmetricCipher : ICipher
    {
        byte[] PublicKey { get; }
    }
}
