using Newtonsoft.Json;

namespace AD.Plugins.TripleDesAuthToken.RequestsEntities
{
	class DeviceAuthResponseEntity
	{
		[JsonProperty("device_key")]
		public string DeviceKey { get; set; }
	}
}