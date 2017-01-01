#if _JSON_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AD.Plugins.Json
{
	public class JsonConverter : IJsonConverter
	{
		private static readonly JsonSerializerSettings Settings;

		static JsonConverter()
		{
			Settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
				Converters = new List<Newtonsoft.Json.JsonConverter>
				{
					new EnumJsonConverter(),
				},
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
			};
		}

		public T DeserializeObject<T>(string inputText)
		{
            return JsonConvert.DeserializeObject<T>(inputText, Settings);
		}

		public string SerializeObject(object toSerialise)
		{
			return JsonConvert.SerializeObject(toSerialise, Formatting.None, Settings);
		}

		public object DeserializeObject(Type type, string inputText)
		{
			return JsonConvert.DeserializeObject(inputText, type, Settings);
		}
	}
}
#endif