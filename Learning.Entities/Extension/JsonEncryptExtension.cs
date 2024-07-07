using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Learning.Entities.Extension
{
    public static class JsonEncryptExtension
    {
        public static string EncryptJson(this object jsonText)
        {
            // Generate a random encryption key and initialization vector (IV)
            byte[] encryptionKey = new byte[32]; // 256 bits
            byte[] iv = new byte[16]; // 128 bits
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(encryptionKey);
                rng.GetBytes(iv);
            }

            // Create an AES encryptor
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write the JSON string to the crypto stream
                            swEncrypt.Write(jsonText);
                        }
                    }

                    // Get the encrypted bytes
                    byte[] encryptedData = msEncrypt.ToArray();

                    // Convert the IV and encrypted data to a single byte array
                    byte[] result = new byte[iv.Length + encryptedData.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedData, 0, result, iv.Length, encryptedData.Length);

                    // The 'result' array now contains the IV followed by the encrypted JSON
                    // You can store or transmit this 'result' array as needed
                    return Convert.ToBase64String(encryptedData);
                }
            }
        }

    }
}
