#if _ENCRYPTION_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AD.Plugins.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private static TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();

        private byte[] _iv;
        private byte[] _key;

        public EncryptionService(IEncryptionConfig cfg)
        {
            _iv = cfg.InitVector;
            _key = cfg.Key;
        }

        public void Init(byte[] iv, byte[] key)
        {
            _iv = iv;
            _key = key;
        }

        public string EncryptData(string plaintext)
        {
            if (_iv == null || _key == null)
            {
                throw new TripleDESUninitializedException();
            }

            TripleDES.Key = _key;
            TripleDES.IV = _iv;
            // Convert the plaintext string to a byte array.       
            byte[] plaintextBytes = Encoding.Unicode.GetBytes(plaintext);
            // Create the stream.         
            MemoryStream ms = new MemoryStream();
            // Create the encoder to write to the stream.         
            CryptoStream encStream = new CryptoStream(ms, TripleDES.CreateEncryptor(), CryptoStreamMode.Write);
            // Use the crypto stream to write the byte array to the stream.         
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();
            // Convert the encrypted stream to a printable string.         
            return Convert.ToBase64String(MemoryStreamToByteArray(ms));
        }

        public string DecryptData(string encryptedtext)
        {
            if (_iv == null || _key == null)
            {
                throw new TripleDESUninitializedException();
            }

            TripleDES.Key = _key;
            TripleDES.IV = _iv;
            // Convert the encrypted text string to a byte array.         
            byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);
            // Create the stream.         
            MemoryStream ms = new MemoryStream();
            // Create the decoder to write to the stream.         
            CryptoStream decStream = new CryptoStream(ms, TripleDES.CreateDecryptor(), CryptoStreamMode.Write);
            // Use the crypto stream to write the byte array to the stream.         
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();
            // Convert the plaintext stream to a string.         
            return Encoding.Unicode.GetString(MemoryStreamToByteArray(ms));
        }

        private byte[] MemoryStreamToByteArray(MemoryStream ms)
        {
            byte[] biteArray = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(biteArray, 0, (int)ms.Length);
            return biteArray;
        }
    }
}
#endif