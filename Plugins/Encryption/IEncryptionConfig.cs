#if _ENCRYPTION_ || _TDES_AUTH_TOKEN_
using System;
namespace AD
{
    public interface IEncryptionConfig
    {
        byte[] InitVector { get; }
        byte[] Key { get; }
    }
}
#endif
