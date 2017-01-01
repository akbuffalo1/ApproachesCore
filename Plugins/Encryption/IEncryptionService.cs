#if _ENCRYPTION_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.Encryption
{
    public interface IEncryptionService
    {
        void Init(byte[] iv, byte[] key);
        string EncryptData(string plaintext);
        string DecryptData(string encryptedtext);
    }
}
#endif