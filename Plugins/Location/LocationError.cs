#if _LOCATION_
namespace AD.Plugins.Location
{
	public class LocationError
	{
		public LocationError(LocationErrorCode code)
		{
			Code = code;
		}

		public LocationErrorCode Code { get; private set; }
	}
}
#endif