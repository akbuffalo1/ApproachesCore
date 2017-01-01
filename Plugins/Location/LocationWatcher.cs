#if _LOCATION_
using System;

namespace AD.Plugins.Location
{
	public abstract class LocationWatcher
		: ILocationWatcher
	{
		private Action<GeoLocation> _locationCallback;
		private Action<LocationError> _errorCallback;

		public void Start(LocationOptions options, Action<GeoLocation> success, Action<LocationError> error)
		{
			lock (this)
			{
				_locationCallback = success;
				_errorCallback = error;

				PlatformSpecificStart(options);

				Started = true;
			}
		}

		public void Stop()
		{
			lock (this)
			{
				_locationCallback = position => { };
				_errorCallback = error => { };

				PlatformSpecificStop();

				Started = false;
			}
		}

		public bool Started { get; set; }

		public abstract GeoLocation CurrentLocation { get; }
		public GeoLocation LastSeenLocation { get; protected set; }

		protected abstract void PlatformSpecificStart(LocationOptions options);
		protected abstract void PlatformSpecificStop();

		protected virtual void SendLocation(GeoLocation location)
		{
			LastSeenLocation = location;
			var callback = _locationCallback;
			if (callback != null)
				callback(location);
		}

		protected void SendError(LocationErrorCode code)
		{
			SendError(new LocationError(code));
		}

		protected void SendError(LocationError error)
		{
			var errorCallback = _errorCallback;
			if (errorCallback != null)
				errorCallback(error);
		}
	}
}
#endif