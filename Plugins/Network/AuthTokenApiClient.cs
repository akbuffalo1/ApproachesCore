#if _NETWORK_ || _TDES_AUTH_TOKEN_
using AD.Plugins.Network.Rest;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.Network
{
    public class AuthTokenApiClient : ApiClient, IAuthTokenApiClient
    {
        private const string TAG = "AD.AuthTokenApiClient";
        private const string FilePath = "\\authtoken.dat";

        private string _authToken;

        public AuthTokenApiClient(
            ILogger logger,
            IHttpServerConfig config,
            IJsonConverter serializer,
            IJsonRestClient restClient,
            INetworkReachability networkReachability,
            IFileStore fileStore
            ) : base(logger, config, serializer, restClient, networkReachability, fileStore)
        {

        }

        public string AuthToken
        {
            get
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    return _authToken;
                }

                string cache;
                if (FileStore.TryReadTextFile(FilePath, out cache))
                {
                    _authToken = (Serializer.DeserializeObject<BaseLoginResponse>(cache)).Token;
                    return _authToken;
                }
                else
                {
                    return null;
                }
            }
        }

        public void ClearToken()
        {
            _authToken = null;
            FileStore.DeleteFile(FilePath);
        }

        public void RequestAuthToken<T>(string path, T requestBody, Action<string> receivedTokenHandler, Action<Exception> errorHandler) where T : BaseLoginRequest
        {
            ClearToken();
            MakeRequestFor<BaseLoginResponse, T>(path ?? "/api-token-auth/", requestBody,
                (response) => 
                {
                    if(!string.IsNullOrEmpty(response.Result.Token))
                    {
                        _authToken = response.Result.Token;
                        FileStore.WriteFile(FilePath, Serializer.SerializeObject(response.Result));
                        receivedTokenHandler(_authToken);
                    }
                }, 
                errorHandler);
        }

        public override RestRequest CreateRequest(string path)
        {
            var request = base.CreateRequest(path);
            SetAuthorizationHeader(request);
            return request;
        }

        public override RestRequest CreateRequest<T>(string path, T body)
        {
            var request = base.CreateRequest<T>(path, body);
            SetAuthorizationHeader(request);
            return request;
        }

        private void SetAuthorizationHeader(RestRequest request)
        {
            if (!string.IsNullOrEmpty(AuthToken))
            {
                request.Headers.Add("Authorization", "Token " + AuthToken);
            }
        }
    }
}
#endif