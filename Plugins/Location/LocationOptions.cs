#if _LOCATION_
using System;

namespace AD.Plugins.Location
{
	public class LocationOptions
	{
		public LocationAccuracy Accuracy { get; set; }
		/// <summary>
		/// Use TimeSpan.Zero for most frequent updates
		/// </summary>
		public TimeSpan TimeBetweenUpdates { get; set; }
		/// <summary>
		/// Use 0 threshold for most frequent updates
		/// </summary>
		public int MovementThresholdInM { get; set; }
		/// <summary>
		/// Use LocationTrackingMode.Background to enable location tracking in background.
		/// </summary>
		public LocationTrackingMode TrackingMode { get; set; }
	}
}
#endif