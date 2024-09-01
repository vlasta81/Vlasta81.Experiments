using System.Security.Cryptography;
using System.Text;

namespace ConfigurationStorage
{
    /// <summary>
    /// Provides methods for hybrid encryption and decryption.
    /// </summary>
    internal class HybridEncryptor
    {
        /// <summary>
        /// Generates a 256-bit key from the given name using a combination of ROT13, Base64 encoding, SHA-256, and SHA-512 hashing.
        /// </summary>
        /// <param name="name">The name from which to generate the key.</param>
        /// <returns>A 256-bit key derived from the given name.</returns>
        private byte[] GenerateKeyFromName(string name)
        {
            string rot13Encoded = Rot13(Base64.Encode(name));
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sha256Hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(rot13Encoded));
                string rot13Hashed256 = Rot13(Base64.Encode(sha256Hash));
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] sha512Hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(name.Length.ToString() + name + rot13Hashed256));
                    string rot13Hashed512 = Rot13(Base64.Encode(sha512Hash));
                    byte[] sha512HashArray = Encoding.UTF8.GetBytes(rot13Hashed512 + rot13Hashed256);
                    Array.Resize(ref sha512HashArray, 32);
                    return sha512HashArray;
                }
            }
        }

        /// <summary>
        /// Applies the ROT13 algorithm to the input string.
        /// </summary>
        /// <param name="inputString">The string to be encoded using ROT13.</param>
        /// <returns>The ROT13 encoded string.</returns>
        private string Rot13(string inputString)
        {
            char[] array = inputString.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = (int)array[i];
                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }

        /// <summary>
        /// Encrypts the given plaintext using AES encryption with a key derived from the index name.
        /// </summary>
        /// <param name="indexName">The name used to derive the encryption key.</param>
        /// <param name="plainText">The plaintext to be encrypted.</param>
        /// <returns>The encrypted text, encoded in Base64.</returns>
        public string EncryptString(string indexName, string plainText)
        {

            byte[] key = GenerateKeyFromName(indexName);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();
                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }

        }

        /// <summary>
        /// Encrypts the given plaintext if secured is true, otherwise returns the plaintext.
        /// </summary>
        /// <param name="secured">Indicates whether the encryption should be applied.</param>
        /// <param name="indexName">The name used to derive the encryption key.</param>
        /// <param name="plainText">The plaintext to be encrypted.</param>
        /// <returns>The encrypted text if secured is true, otherwise the plaintext.</returns>
        public string EncryptString(bool secured, string indexName, string plainText)
        {
            if (secured) { return EncryptString(indexName, plainText); }
            return plainText;
        }

        /// <summary>
        /// Decrypts the given ciphertext if secured is true, otherwise returns the ciphertext.
        /// </summary>
        /// <param name="secured">Indicates whether the decryption should be applied.</param>
        /// <param name="indexName">The name used to derive the decryption key.</param>
        /// <param name="cipherText">The ciphertext to be decrypted.</param>
        /// <returns>The decrypted text if secured is true, otherwise the ciphertext.</returns>
        public string DecryptString(bool secured, string indexName, string cipherText)
        {
            if (secured) { return DecryptString(indexName, cipherText); }
            return cipherText;
        }

        /// <summary>
        /// Decrypts the given ciphertext using AES decryption with a key derived from the index name.
        /// </summary>
        /// <param name="indexName">The name used to derive the decryption key.</param>
        /// <param name="cipherText">The ciphertext to be decrypted.</param>
        /// <returns>The decrypted text.</returns>
        public string DecryptString(string indexName, string cipherText)
        {
            byte[] key = GenerateKeyFromName(indexName);
            var fullCipher = Convert.FromBase64String(cipherText);
            using (Aes aesAlg = Aes.Create())
            {
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
                aesAlg.Key = key;
                aesAlg.IV = iv;
                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var msDecrypt = new MemoryStream(cipher))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

    }
}
