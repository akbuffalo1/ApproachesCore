#if _LOCATION_
namespace AD.Plugins.Location
{
	public class Coordinates
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double? Accuracy { get; set; }

		public double? Altitude { get; set; }
		public double? AltitudeAccuracy { get; set; }

		public double? Heading { get; set; }
		public double? HeadingAccuracy { get; set; }

		public double? Speed { get; set; }
	}
}
#endif