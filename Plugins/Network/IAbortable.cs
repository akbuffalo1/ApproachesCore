#if _NETWORK_ || _TDES_AUTH_TOKEN_
namespace AD.Plugins.Network.Rest
{
	public interface IAbortable
	{
		void Abort();
	}
}
#endif