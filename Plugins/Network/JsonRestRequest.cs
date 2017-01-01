#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace AD.Plugins.Network.Rest
{
	public class JsonRestRequest<T> : TextBasedRestRequest
		where T : class
	{
		public override HttpContent Content => Body != null ? new StringContent(JsonConverterProvider?.Invoke().SerializeObject(Body), Encoding.UTF8, ContentType.Json) : null;

		public T Body { get; set; }
		public Func<IJsonConverter> JsonConverterProvider { get; set; }

		public JsonRestRequest(string url, string verb = Verbs.Post, string accept = ContentType.Json, string tag = null)
			: base(url, verb, accept, tag)
		{
			InitializeCommon();
		}

		public JsonRestRequest(Uri url, string verb = Verbs.Post, string accept = ContentType.Json, string tag = null)
			: base(url, verb, accept, tag)
		{
			InitializeCommon();
		}

		private void InitializeCommon()
		{
			ContentTypeString = ContentType.Json;
			JsonConverterProvider = () => AD.Resolver.Resolve<IJsonConverter>();
		}
	}
}
#endif