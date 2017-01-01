#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using System.IO;

namespace AD.Plugins.Network.Rest
{
	public class JsonRestClient : RestClient , IJsonRestClient
	{
		public Func<IJsonConverter> JsonConverterProvider { get; set; }

		public IAbortable MakeRequestFor<T>(RestRequest restRequest, Action<DecodedRestResponse<T>> successAction, Action<Exception> errorAction)
		{
			return MakeRequest(restRequest, (StreamRestResponse streamResponse) =>
				{
					using (var textReader = new StreamReader(streamResponse.Stream))
					{
						var text = textReader.ReadToEnd();
						var result = JsonConverterProvider().DeserializeObject<T>(text);
						var decodedResponse = new DecodedRestResponse<T>
						{
							CookieCollection = streamResponse.CookieCollection,
							Result = result,
							StatusCode = streamResponse.StatusCode,
							Tag = streamResponse.Tag
						};
						successAction(decodedResponse);
					}
				}, errorAction);
		}

		public JsonRestClient()
		{
			JsonConverterProvider = () => AD.Resolver.Resolve<IJsonConverter>();
		}
	}
}
#endif