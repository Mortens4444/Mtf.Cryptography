using Mtf.Cryptography.SymmetricCiphers;
using System.Security.Cryptography;

namespace Mtf.Cryptography.Tests.SymmetricCiphers
{
    [TestFixture]
    public class AesCipherTests
    {
        private static AesCipher CreateCipher(out byte[] key, out byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();
                key = aes.Key;
                iv = aes.IV;
                return new AesCipher(key, iv);
            }
        }

        [Test]
        [TestCase("Hello world!")]
        [TestCase("Árvíztűrő tükörfúrógép")]
        [TestCase("漢字テスト")] // Unicode
        [TestCase("")]
        [TestCase(null)]
        public void EncryptDecrypt_String_ShouldReturnOriginal(string? input)
        {
            var cipher = CreateCipher(out _, out _);
            var encrypted = cipher.Encrypt(input);
            var decrypted = cipher.Decrypt(encrypted);

            Assert.That(decrypted, Is.EqualTo(input));
        }

        [Test]
        [TestCase(new byte[] { 1, 2, 3, 4, 5 })]
        [TestCase(new byte[] { })]
        public void EncryptDecrypt_Bytes_ShouldReturnOriginal(byte[] input)
        {
            var cipher = CreateCipher(out _, out _);
            var encrypted = cipher.Encrypt(input);
            var decrypted = cipher.Decrypt(encrypted);

            Assert.That(decrypted, Is.EqualTo(input));
        }

        [Test]
        public void EncryptDecrypt_Bytes_Null_ShouldReturnNull()
        {
            var cipher = CreateCipher(out _, out _);
            byte[] input = null;
            var encrypted = cipher.Encrypt(input);
            var decrypted = cipher.Decrypt(encrypted);

            Assert.That(encrypted, Is.Null);
            Assert.That(decrypted, Is.Null);
        }

        [Test]
        public void Constructor_NullKey_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new AesCipher(null, new byte[16]));
        }

        [Test]
        public void Constructor_NullIV_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new AesCipher(new byte[32], null));
        }

        [Test]
        public void Dispose_ShouldNotThrow()
        {
            var cipher = CreateCipher(out _, out _);
            Assert.DoesNotThrow(() => cipher.Dispose());
        }
    }
}
