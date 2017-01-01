using Newtonsoft.Json;

namespace AD.Plugins.TripleDesAuthToken.RequestsEntities
{
	public class LoginCredentialsEntity
	{
		[JsonProperty("userid")]
		public string Username { get; set; }

		[JsonProperty("password")]
		public string Password { get; set; }
	}
}