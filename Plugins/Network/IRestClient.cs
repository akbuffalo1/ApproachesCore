#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using AD.Plugins.Network.Rest;
namespace AD
{
	public interface IRestClient
	{
		void ClearSetting(string key);
		void SetSetting(string key, object value);

		IAbortable MakeRequest(RestRequest restRequest, Action<RestResponse> successAction,
			Action<Exception> errorAction);

		IAbortable MakeRequest(RestRequest restRequest, Action<StreamRestResponse> successAction,
			Action<Exception> errorAction);
	}
}
#endif