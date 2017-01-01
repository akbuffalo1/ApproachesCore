#if _NETWORK_ || _TDES_AUTH_TOKEN_
namespace AD.Plugins.Network.Rest
{
	public static class Verbs
	{
		public const string Get = "GET";
        public const string Patch = "PATCH";
		public const string Put = "PUT";
		public const string Post = "POST";
		public const string Delete = "DELETE";
	}
}
#endif