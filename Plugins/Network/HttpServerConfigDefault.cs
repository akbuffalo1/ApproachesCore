#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using AD.Plugins.Network.Rest;

namespace AD.Plugins.Network
{
	public abstract class ServerConfigBase : IHttpServerConfig
	{
		public ServerConfigBase() {}

		public virtual String Server { get; set;}
		public virtual int Port { get; set; }
		public virtual HttpProtocol Protocol { get; set; }

		public String BaseAddress {
			get {
				String protocol = Enum.GetName(typeof(HttpProtocol), this.Protocol).ToLower();
				return string.Format("{0}://{1}:{2}", protocol, this.Server, this.Port);
			}
		}
	}

	public class HttpServerConfigDefault : ServerConfigBase
	{
		//public HttpServerConfigDefault() {}

		public HttpServerConfigDefault() {
			Protocol = HttpProtocol.Http;
			Server = "localhost";
			Port = 8000;
		}
	}
}
#endif

