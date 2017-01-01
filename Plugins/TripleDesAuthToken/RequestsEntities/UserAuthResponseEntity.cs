using Newtonsoft.Json;

namespace AD.Plugins.TripleDesAuthToken.RequestsEntities
{
	public class UserAuthResponseEntity : ResponseEntityBase
	{
		[JsonProperty("auth_provider")]
		public string AuthProvider { get; set; }
	}
}