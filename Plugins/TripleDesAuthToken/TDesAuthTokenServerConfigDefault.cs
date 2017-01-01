#if _TDES_AUTH_TOKEN_

using System;

namespace AD.Plugins.TripleDesAuthToken
{
	public class TDesAuthTokenServerConfigBase : ITDesAuthTokenServerConfig
	{
		public string DeviceAuthRequestPath { get; set; }
		public string UserAuthRequestPath { get; set; }

    }

	public class TDesAuthTokenServerConfigDefault : TDesAuthTokenServerConfigBase
	{
		public TDesAuthTokenServerConfigDefault ()
		{
			DeviceAuthRequestPath = "/api/v1/devices/";
			UserAuthRequestPath = "/api/v1/authuser/";
        }
	}
}

#endif