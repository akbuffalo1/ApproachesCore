#if _TDES_AUTH_TOKEN_
using AD.Plugins.Network.Rest;

namespace AD.Plugins.TripleDesAuthToken
{
	public class TDesAuthApiClient : ApiClient
	{
		private readonly ITDesAuthStore _authStore;
		
		public TDesAuthApiClient(
			ILogger logger,
			IHttpServerConfig config,
			IJsonConverter serializer,
			IJsonRestClient restClient,
			INetworkReachability networkReachability,
			IFileStore fileStore,
			ITDesAuthStore authStore
			) : base(logger, config, serializer, restClient, networkReachability, fileStore)
		{
			_authStore = authStore;
		}

		public override RestRequest CreateRequest(string path)
		{
			var request = base.CreateRequest(path);
			request.Headers["x-auth-token"] = _authStore.AuthToken;
			return request;
		}

		public override RestRequest CreateRequest<T>(string path, T body)
		{
			var request = base.CreateRequest<T>(path, body);
			request.Headers["x-auth-token"] = _authStore.AuthToken;
			return request;
		}
	}
}
#endif