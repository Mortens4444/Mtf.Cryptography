# RsaCipher Class

Implements the `IAsymmetricCipher` interface using RSA encryption with optional OAEP padding.  
Provides both string and byte-level encryption/decryption, with support for importing keys from XML files.

Namespace: `Mtf.Cryptography.AsymmetricCiphers`  
Implements: `IAsymmetricCipher`, `IDisposable`

---

## 🔐 Features

- Uses **RSA with OAEP (SHA-256)** or **PKCS#1 v1.5** padding
- Supports **encryption with public key** and **decryption with private key**
- Can **import keys from XML files**
- Exposes `RSAParameters` containing the public key
- Supports encryption/decryption of both byte arrays and strings (Base64 encoded)

---

## 🧩 Constructors

### `RsaCipher(string keyFilePath, bool includePrivateParameters = false, bool useOaepPadding = true)`

Loads RSA parameters from an XML-formatted key file.

- `keyFilePath`: Path to the XML file
- `includePrivateParameters`: `true` to include private key parameters
- `useOaepPadding`: Use OAEP with SHA-256 padding (`true`) or PKCS#1 v1.5 (`false`)

---

### `RsaCipher(RSAParameters parameters, bool useOaepPadding = true)`

Initializes from an `RSAParameters` struct.

- `parameters`: Must include at least `Modulus` and `Exponent`
- `useOaepPadding`: OAEP (SHA-256) or PKCS#1 padding

---

## 🔑 Public Properties

### `RSAParameters PublicKeyParameters { get; }`

Contains the public portion of the RSA key (`Modulus`, `Exponent`).

---

## 🔒 Methods

### `byte[] Encrypt(byte[] plainBytes)`

Encrypts a byte array using the configured RSA key and padding.

### `string Encrypt(string plainText)`

Encrypts a string using UTF-8 and returns a Base64 encoded result.

---

### `byte[] Decrypt(byte[] cipherBytes)`

Decrypts an RSA-encrypted byte array.  
**Requires that the private key is present**, otherwise throws `InvalidOperationException`.

### `string Decrypt(string cipherText)`

Decrypts a Base64-encoded RSA ciphertext string.  
**Requires private key.**

---

## ♻️ Disposal

### `Dispose()`

Releases underlying RSA resources. Safe to call multiple times.

---

## ⚠️ Exceptions

- `ArgumentNullException` — Input is null
- `InvalidOperationException` — Private key is missing for decryption
- `CryptographicException` — Key import or crypto failure
- `FormatException` — Invalid Base64 input during decryption

---

## 📦 Example Usage

```csharp
var rsa = new RsaCipher("key.xml", includePrivateParameters: true);
var encrypted = rsa.Encrypt("secret message");
var decrypted = rsa.Decrypt(encrypted);
