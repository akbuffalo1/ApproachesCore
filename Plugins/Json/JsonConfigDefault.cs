#if _JSON_ || _TDES_AUTH_TOKEN_
using System;

namespace AD.Plugins.Json
{
	public class JsonConfigDefault : IJsonConfig
	{
		#region IJsonConfig implementation

		public bool RegisterAsTextSerializer {
			get {
				return true;
			}
		}

		#endregion

		public JsonConfigDefault ()
		{
		}
	}
}
#endif