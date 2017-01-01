#if __ANDROID__ && _LOCATION_
using AD.Plugins.CurrentActivity;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.LocationMap.Droid
{
    public class LocationMapDroid : ILocationMap
    {
        public void LoadLocationMap(string address)
        {
            Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse($"geo:0,0?q={address}");
            Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
            mapIntent.SetPackage("com.google.android.apps.maps");
            Resolver.Resolve<ICurrentActivity>().Activity.StartActivity(mapIntent);
        }
    }
}
#endif