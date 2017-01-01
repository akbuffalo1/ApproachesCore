using Newtonsoft.Json;

namespace AD.Plugins.TripleDesAuthToken.RequestsEntities
{
	public class ResponseEntityBase
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }
	}
}

