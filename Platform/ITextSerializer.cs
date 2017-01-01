using System;

namespace AD
{
	public interface ITextSerializer
	{
		T DeserializeObject<T>(string inputText);
		string SerializeObject(object toSerialise);
		object DeserializeObject(Type type, string inputText);
	}
}