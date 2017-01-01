#if _TDES_AUTH_TOKEN_

namespace AD.Plugins.TripleDesAuthToken
{
	public abstract class TDesAuthConfigBase : ITDesAuthConfig
	{
		public virtual int AppId { get; set; }
		public virtual string ConfigFile { get; set; }
	}

	public class TDesAuthConfigDefault : TDesAuthConfigBase
	{
		public TDesAuthConfigDefault ()
		{
			AppId = 1;
			ConfigFile = "tdesconfig";
		}
	}
}

#endif