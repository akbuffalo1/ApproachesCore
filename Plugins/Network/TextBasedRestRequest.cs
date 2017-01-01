#if _NETWORK_ || _TDES_AUTH_TOKEN_
using System;
using System.IO;
using System.Text;

namespace AD.Plugins.Network.Rest
{
	public abstract class TextBasedRestRequest : RestRequest
	{
		protected TextBasedRestRequest(string url, string verb = Verbs.Get, string accept = ContentType.Json, string tag = null)
			: base(url, verb, accept, tag)
		{
		}

		protected TextBasedRestRequest(Uri uri, string verb = Verbs.Get, string accept = ContentType.Json, string tag = null)
			: base(uri, verb, accept, tag)
		{
		}

		protected static void WriteTextToStream(Stream stream, string text)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
		}
	}
}
#endif
