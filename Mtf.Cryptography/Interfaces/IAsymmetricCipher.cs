namespace Mtf.Cryptography.Interfaces
{
    public interface IAsymmetricCipher : ICipher
    {
        string Header { get; }

        byte[] PublicKey { get; }
    }
}
