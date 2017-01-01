#if _LOCATION_
using System;
using AD.Plugins.Location;

namespace AD
{
	public interface ILocationWatcher
	{
		void Start(
			LocationOptions options,
			Action<GeoLocation> success,
			Action<LocationError> error);
		void Stop();
		bool Started { get; }
		GeoLocation CurrentLocation { get; }
		GeoLocation LastSeenLocation { get; }
	}
}
#endif