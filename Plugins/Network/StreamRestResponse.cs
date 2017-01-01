#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System.IO;

namespace AD.Plugins.Network.Rest
{
	public class StreamRestResponse
		: RestResponse
	{
		public Stream Stream { get; set; }
	}
}
#endif