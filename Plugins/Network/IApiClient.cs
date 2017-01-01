#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using AD.Plugins.Network.Rest;

namespace AD
{
	public interface IApiClient 
	{
		IHttpServerConfig ServerConfig { get; }
		IJsonConverter Serializer { get;}
		IJsonRestClient RestClient { get;}
		INetworkReachability NetworkReachability { get;}
		//Task<HttpResponseMessage> GetAsync(string uri);

		void MakeCacheableRequest<T> (Priority priority,
			string path, 
			string slug, 
			Action<T> successFunc, 
			Action<Exception> errorAct,
			Action<string,T> storeData,
			Func<string, T> retrieveData);

		void MakeFileCacheableRequest<T>(Priority priority, string path, string slug, Action<T> successAction, Action<Exception> errorAction);

		IAbortable MakeRequest(string path, Action<RestResponse> successAction, Action<Exception> errorAction);

		IAbortable MakeRequest(string path, Action<StreamRestResponse> successAction, Action<Exception> errorAction);

		IAbortable MakeRequestFor<TResponse>(string path, Action<DecodedRestResponse<TResponse>> successAction, Action<Exception> errorAction, string verb = Verbs.Post);

        IAbortable MakeRequestFor<TResponse, TRequest>(string path, TRequest requestBody, Action<DecodedRestResponse<TResponse>> successAction, Action<Exception> errorAction, string verb = Verbs.Post) where TRequest : class;

    }
}
#endif

