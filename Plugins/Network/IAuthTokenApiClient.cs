#if _NETWORK_ || _TDES_AUTH_TOKEN_
using AD.Plugins.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD
{
    public interface IAuthTokenApiClient : IApiClient
    {
        string AuthToken { get; }
        void RequestAuthToken<T>(string path, T requestBody, Action<string> receivedTokenHandler, Action<Exception> errorHandler) where T : BaseLoginRequest;
        void ClearToken();
    }
}
#endif
