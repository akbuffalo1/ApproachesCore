#if _NETWORK_ || _TDES_AUTH_TOKEN_
namespace AD.Plugins.Network.Rest
{
	public static class ContentType
	{
		public const string Json = "application/json";
		public const string WwwForm = "application/x-www-form-urlencoded";
		public const string MultipartFormWithBoundary = "multipart/form-data; boundary=";
		//public const string Xml = "application/xml";
	}
}
#endif