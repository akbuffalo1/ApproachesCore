#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using AD.Plugins.Network.Rest;

namespace AD
{
	public interface IJsonRestClient : IRestClient
	{
		Func<IJsonConverter> JsonConverterProvider { get; set; }

		IAbortable MakeRequestFor<T>(RestRequest restRequest, Action<DecodedRestResponse<T>> successAction,
			Action<Exception> errorAction);
	}
}
#endif