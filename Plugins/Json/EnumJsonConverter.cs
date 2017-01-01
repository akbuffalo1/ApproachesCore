#if _JSON_ || _TDES_AUTH_TOKEN_
using System;
using System.Reflection;
using Newtonsoft.Json;

namespace AD.Plugins.Json
{
	public class EnumJsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.GetTypeInfo().IsEnum;
		}

		public override void WriteJson(JsonWriter writer, object
			value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			var theString = reader.Value.ToString();
			return Enum.Parse(objectType, theString, false);
			//return Enum.ToObject(objectType, int.Parse());
		}
	}
}
#endif