#if _TDES_AUTH_TOKEN_

namespace AD
{
	public interface ITDesAuthTokenServerConfig
	{
		string DeviceAuthRequestPath { get; set; }
		string UserAuthRequestPath { get; set; }
	}
}

#endif
