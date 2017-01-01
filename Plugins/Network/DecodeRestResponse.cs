#if _NETWORK_ || _TDES_AUTH_TOKEN_
namespace AD.Plugins.Network.Rest
{
	public class DecodedRestResponse<T>
		: RestResponse
	{
		public T Result { get; set; }
	}
}
#endif