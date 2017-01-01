#if _TDES_AUTH_TOKEN_

namespace AD
{
	public interface ITDesAuthConfig
	{
		int AppId { get; set; }
		string ConfigFile { get; set; }
	}
}

#endif
