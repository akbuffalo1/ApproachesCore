using Newtonsoft.Json;

namespace AD.Plugins.TripleDesAuthToken.RequestsEntities
{
	public class DeviceAuthRequestEntity
	{
		[JsonProperty("device")]
		public string Device;

		[JsonProperty("os")]
		public string OS;

		[JsonProperty("osversion")]
		public string OSVersion;
	}
}