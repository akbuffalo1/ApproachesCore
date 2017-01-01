#if _TDES_AUTH_TOKEN_

using Newtonsoft.Json;

namespace AD.Plugins.TripleDesAuthToken.RequestsEntities
{
	public class AuthDataRequestEntity
	{
		[JsonProperty ("idapp")]
		public int AppId {
			get;
			set;
		} = AD.Resolver.Resolve<ITDesAuthConfig>().AppId;

		[JsonProperty ("username")]
		public string Username { get; set; }

		[JsonProperty ("password")]
		public string Password { get; set; }

		[JsonProperty ("device_key")]
		public string DeviceKey { get; set; }

		[JsonProperty ("auth_provider")]
		public string AuthProvider { get; set; }
	}
}

#endif