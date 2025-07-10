using Mtf.Cryptography.AsymmetricCiphers;
using Mtf.Cryptography.Extensions;
using Mtf.Cryptography.Interfaces;
using Mtf.Cryptography.KeyGenerators;
using System.Security.Cryptography;

namespace Mtf.Cryptography.Tests.AsymmetricCiphers
{
    [TestFixture]
    public class RsaCipherTests
    {
        private RSAParameters rsaParameters;
        private RsaCipher rsaCipher;

        [SetUp]
        public void SetUp()
        {
            using (var rsa = RSA.Create())
            {
                rsaParameters = rsa.ExportParameters(true);
            }

            rsaCipher = new RsaCipher(rsaParameters);
        }

        [Test]
        public void Encrypt_Decrypt_ValidInput_ShouldReturnSameResultWithFileKeys()
        {
            if (!File.Exists("key.xml"))
            {
                RsaKeyGenerator.GenerateKeyFiles("key.xml", "public.xml");
            }
            var cipher = new RsaCipher("key.xml");
            var originalText = "hello";
            var encrypted = rsaCipher.Encrypt(originalText);
            var decrypted = rsaCipher.Decrypt(encrypted);
            Assert.That(decrypted, Is.EqualTo(originalText));
        }

        [Test]
        public void Encrypt_Decrypt_ValidInput_ShouldReturnSameResult()
        {
            var originalText = "This is a test message!";
            var encryptedText = rsaCipher.Encrypt(originalText);
            var decryptedText = rsaCipher.Decrypt(encryptedText);

            Assert.That(decryptedText, Is.EqualTo(originalText));
        }

        [Test]
        public void Encrypt_NullInput_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => rsaCipher.Encrypt((string)null));
            Assert.Throws<ArgumentNullException>(() => rsaCipher.Encrypt((byte[])null));
        }

        [Test]
        public void Decrypt_NullInput_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => rsaCipher.Decrypt((string)null));
            Assert.Throws<ArgumentNullException>(() => rsaCipher.Decrypt((byte[])null));
        }

        [Test]
        public void Decrypt_InvalidBase64Input_ShouldThrowFormatException()
        {
            var invalidBase64 = "invalidBase64";
            Assert.Throws<FormatException>(() => rsaCipher.Decrypt(invalidBase64));
        }

        [Test]
        public void Decrypt_WithoutPrivateKey_ShouldThrowInvalidOperationException()
        {
            var rsaParametersWithoutPrivateKey = rsaParameters.ToPublicKey();

            var rsaCipherWithoutPrivateKey = new RsaCipher(rsaParametersWithoutPrivateKey);
            var encryptedText = rsaCipher.Encrypt("Test");

            Assert.Throws<InvalidOperationException>(() => rsaCipherWithoutPrivateKey.Decrypt(encryptedText));
        }

        [Test]
        public void EncryptDecrypt_WithOaepPadding_ShouldReturnSameResult()
        {
            var originalText = "This is a test message!";
            var rsaCipherWithOaep = new RsaCipher(rsaParameters, useOaepPadding: true);

            var encryptedText = rsaCipherWithOaep.Encrypt(originalText);
            var decryptedText = rsaCipherWithOaep.Decrypt(encryptedText);

            Assert.That(decryptedText, Is.EqualTo(originalText));
        }

        [Test]
        public void EncryptDecrypt_WithPkcs1Padding_ShouldReturnSameResult()
        {
            var originalText = "This is a test message!";
            var rsaCipherWithPkcs1 = new RsaCipher(rsaParameters, useOaepPadding: false);

            var encryptedText = rsaCipherWithPkcs1.Encrypt(originalText);
            var decryptedText = rsaCipherWithPkcs1.Decrypt(encryptedText);

            Assert.That(decryptedText, Is.EqualTo(originalText));
        }


        [Test]
        public void Encrypt_Decrypt()
        {
            var originalText = "Test";
            var rsaCipher = new RsaCipher(rsaParameters);
            var cryptor = new RsaCipher(rsaCipher.PublicKeyParameters);
            var encryptedText = cryptor.Encrypt(originalText);
            var decryptedText = rsaCipher.Decrypt(encryptedText);
            Assert.That(decryptedText, Is.EqualTo(originalText));
        }

        [Test]
        public void Encrypt_Decrypt_FromFileKey()
        {
            var originalText = "Test";
            if (!File.Exists("key.xml"))
            {
                RsaKeyGenerator.GenerateKeyFiles("key.xml", "public.xml");
            }
            var rsaCipher = new RsaCipher("key.xml", true);
            var cryptor = new RsaCipher("key.xml", true);
            var encryptedText = cryptor.Encrypt(originalText);
            var decryptedText = rsaCipher.Decrypt(encryptedText);
            Assert.That(decryptedText, Is.EqualTo(originalText));

            var encryptedText2 = rsaCipher.Encrypt(originalText);
            var decryptedText2 = cryptor.Decrypt(encryptedText);
            Assert.That(decryptedText2, Is.EqualTo(originalText));
        }

        [Test]
        public void Encrypt_Decrypt_Reflection_FromFileKey()
        {
            var originalText = "Test";
            if (!File.Exists("key.xml"))
            {
                RsaKeyGenerator.GenerateKeyFiles("key.xml", "public.xml");
            }
            var rsaCipher = CreateCiphers(typeof(RsaCipher), new object[] { "key.xml", true, true })[0];
            var cryptor = CreateCiphers(typeof(RsaCipher), new object[] { "key.xml", true, true })[0];
            var encryptedText = cryptor.Encrypt(originalText);
            var decryptedText = rsaCipher.Decrypt(encryptedText);
            Assert.That(decryptedText, Is.EqualTo(originalText));

            var encryptedText2 = rsaCipher.Encrypt(originalText);
            var decryptedText2 = cryptor.Decrypt(encryptedText);
            Assert.That(decryptedText2, Is.EqualTo(originalText));
        }

        [Test]
        public void Encrypt_Decrypt_Reflection_FromFileKey_SameCipher()
        {
            var originalText = "Test";
            if (!File.Exists("key.xml"))
            {
                RsaKeyGenerator.GenerateKeyFiles("key.xml", "public.xml");
            }
            var rsaCipher = CreateCiphers(typeof(RsaCipher), new object[] { "key.xml", true, true })[0];
            var encryptedText = rsaCipher.Encrypt(originalText);
            var decryptedText = rsaCipher.Decrypt(encryptedText);
            Assert.That(decryptedText, Is.EqualTo(originalText));
        }

        private static ICipher[] CreateCiphers(Type type, object[] args)
        {
            if (type == null)
            {
                return Array.Empty<ICipher>();
            }

            if (args == null)
            {
                return new[] { (ICipher)Activator.CreateInstance(type) };
            }

            return new[] { (ICipher)Activator.CreateInstance(type, args) };
        }

        [TearDown]
        public void TearDown()
        {
            rsaCipher.Dispose();
        }
    }
}
