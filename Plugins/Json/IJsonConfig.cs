#if _JSON_ || _TDES_AUTH_TOKEN_
using System;

namespace AD
{
	public interface IJsonConfig
	{
		bool RegisterAsTextSerializer { get; }
	}
}

#endif