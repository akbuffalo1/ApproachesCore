#if __IOS__ && _LOCATION_
using System;
using System.Text.RegularExpressions;
using Foundation;
using UIKit;

namespace AD.Plugins.LocationMap.iOS
{
	public static class IoSPlatformStrings
	{
		public static string GoogleMapAppUrl = "comgooglemaps://?q=";
		public static string GoogleMapLinkUrl = "http://maps.google.com/maps?q=";
		public static string CannotLoadAppMap = "Google map application is not loaded.";
		public static string CannotLoadLinkMap = "Google map internet url is not loaded.";
	}

	public class LocationMap : AD.Platform.iOS.IosTask, ILocationMap
	{
#region ILocationMap implementation
		ILogger logger;
		public void LoadLocationMap(string address)
		{
			var addr = Regex.Replace(address, @"\s+", "+");

			//set url for application
			var urlstr = IoSPlatformStrings.GoogleMapAppUrl + addr;

			//check if application can be opened
			if (!TryOpenMap(urlstr))
			{
				//if application is not loaded try call online google maps
				logger.Debug("Location Map", IoSPlatformStrings.CannotLoadAppMap);

				urlstr = IoSPlatformStrings.GoogleMapLinkUrl + addr;
				if (!TryOpenMap(urlstr))
					logger.Debug("Location Map", IoSPlatformStrings.CannotLoadLinkMap);
			}
		}


		private bool TryOpenMap(string urlstr)
		{
			var url = NSUrl.FromString(urlstr);
			var can = UIApplication.SharedApplication.CanOpenUrl(url);
			if (can)
				UIApplication.SharedApplication.OpenUrl(url);

			return can;
		}
#endregion ILocationMap

#region Constructor
		public LocationMap()
		{
			logger = AD.Resolver.Resolve<ILogger>();
		}
#endregion
	}
}

#endif