#if _NETWORK_ || _TDES_AUTH_TOKEN_
namespace AD
{
	public interface INetworkReachability
	{
		bool IsHostReachable(string host);
		bool IsConnected { get; }
	}
}
#endif