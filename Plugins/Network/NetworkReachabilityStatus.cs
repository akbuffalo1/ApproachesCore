#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;

namespace AD.Plugins.Network.Reachability
{
	public enum MvxReachabilityStatus
	{
		Not,
		ViaCarrierDataNetwork,
		ViaWiFiNetwork
	}
}
#endif
