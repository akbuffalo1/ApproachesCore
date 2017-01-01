#if _LOCATION_ && __ANDROID__
using Android.Locations;
using Android.OS;

namespace AD.Plugins.Location.Droid
{
	public interface ILocationReceiver
	{
		void OnLocationChanged(global::Android.Locations.Location location);
		void OnProviderDisabled(string provider);
		void OnProviderEnabled(string provider);
		void OnStatusChanged(string provider, Availability status, Bundle extras);
	}
}
#endif