#if _LOCATION_
using System;

namespace AD.Plugins.Location
{
	public class GeoLocation
	{
		public GeoLocation()
		{
			Coordinates = new Coordinates();
		}

		public Coordinates Coordinates { get; set; }
		public DateTimeOffset Timestamp { get; set; }
	}
}
#endif