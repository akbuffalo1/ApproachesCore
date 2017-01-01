#if _LOCATION_ && __ANDROID__
using System;
using System.Threading;
using Android.Content;
using Android.Locations;
using Android.OS;
using AD.Exceptions;
using AD.Droid.Platform;

namespace AD.Plugins.Location.Droid
{
	public sealed class AndroidLocationWatcher
		: AD.Plugins.Location.LocationWatcher
	, ILocationReceiver
	{
		private Context _context;
		private LocationManager _locationManager;
		private readonly LocationListener _locationListener;
		private string _bestProvider;

		private ILogger _logger;

		private const string TAG = "AD.Location.AndroidLocationWatcher";

		public AndroidLocationWatcher(ILogger logger)
		{
			_logger = logger;
			EnsureStopped();
			_locationListener = new LocationListener(this);
		}

		private Context Context
		{
			get
			{
				if (_context == null)
				{
					_context = Android.App.Application.Context; //Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;
				}
				return _context;
			}
		}

		protected override void PlatformSpecificStart(LocationOptions options)
		{
			if (_locationManager != null)
				throw new ADException("You cannot start the Location service more than once");

			_locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
			if (_locationManager == null)
			{
				_logger.Warn(TAG, "Location Service Manager unavailable - returned null");
				SendError(LocationErrorCode.ServiceUnavailable);
				return;
			}
			var criteria = new Criteria()
			{
				Accuracy = options.Accuracy == LocationAccuracy.Fine ? Accuracy.Fine : Accuracy.Coarse,
			};
			_bestProvider = _locationManager.GetBestProvider(criteria, true);
			if (_bestProvider == null)
			{
				_logger.Warn(TAG, "Location Service Provider unavailable - returned null");
				SendError(LocationErrorCode.ServiceUnavailable);
				return;
			}

			_locationManager.RequestLocationUpdates(
				_bestProvider, 
				(long)options.TimeBetweenUpdates.TotalMilliseconds,
				options.MovementThresholdInM, 
				_locationListener);
		}

		protected override void PlatformSpecificStop()
		{
			EnsureStopped();
		}

		private void EnsureStopped()
		{
			if (_locationManager != null)
			{
				_locationManager.RemoveUpdates(_locationListener);
				_locationManager = null;
				_bestProvider = null;
			}
		}

		private static GeoLocation CreateLocation(global::Android.Locations.Location androidLocation)
		{
			var position = new GeoLocation { Timestamp = androidLocation.Time.FromMillisecondsUnixTimeToUtc() };
			var coords = position.Coordinates;

			if (androidLocation.HasAltitude)
				coords.Altitude = androidLocation.Altitude;

			if (androidLocation.HasBearing)
				coords.Heading = androidLocation.Bearing;

			coords.Latitude = androidLocation.Latitude;
			coords.Longitude = androidLocation.Longitude;
			if (androidLocation.HasSpeed)
				coords.Speed = androidLocation.Speed;
			if (androidLocation.HasAccuracy)
			{
				coords.Accuracy = androidLocation.Accuracy;
			}

			return position;
		}

		public override GeoLocation CurrentLocation 
		{ 
			get
			{
				if (_locationManager == null || _bestProvider == null)
					throw new ADException("Location Manager not started");

				var androidLocation = _locationManager.GetLastKnownLocation(_bestProvider);
				if (androidLocation == null)
					return null;

				return CreateLocation(androidLocation);
			}
		}

		#region Implementation of ILocationListener

		public void OnLocationChanged(global::Android.Locations.Location androidLocation)
		{
			if (androidLocation == null)
			{
				_logger.Warn(TAG, "Android: Null location seen");
				return;
			}

			if (androidLocation.Latitude == double.MaxValue
				|| androidLocation.Longitude == double.MaxValue)
			{
				_logger.Warn(TAG, "Android: Invalid location seen");
				return;
			}

			GeoLocation location;
			try
			{
				location = CreateLocation(androidLocation);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception exception)
			{
				_logger.Exception(TAG, exception, "Android: Exception seen in converting location " + exception.ToLongString());
				return;
			}

			SendLocation(location);
		}

		public void OnProviderDisabled(string provider)
		{
			SendError(LocationErrorCode.PositionUnavailable);
		}

		public void OnProviderEnabled(string provider)
		{
			// nothing to do 
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			switch (status)
			{
			case Availability.Available:
				break;
			case Availability.OutOfService:
			case Availability.TemporarilyUnavailable:
				SendError(LocationErrorCode.PositionUnavailable);
				break;
			default:
				throw new ArgumentOutOfRangeException("status");
			}
		}

		#endregion
	}
}
#endif