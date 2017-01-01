#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using AD.Plugins.Network;
using AD.Plugins.Network.Rest;

namespace AD
{
	public interface IHttpServerConfig 
	{
		String Server { get;}
		int Port { get;}
		HttpProtocol Protocol { get;}
		String BaseAddress { get; }
	}
}
#endif
