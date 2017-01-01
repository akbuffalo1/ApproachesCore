#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System.Net;

namespace AD.Plugins.Network.Rest
{
	public class RestResponse
	{
		public string Tag { get; set; }
		public CookieCollection CookieCollection { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
}
#endif