#if _ENCRYPTION_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.Encryption
{
    public class TripleDESUninitializedException : Exception
    {
    }
}
#endif