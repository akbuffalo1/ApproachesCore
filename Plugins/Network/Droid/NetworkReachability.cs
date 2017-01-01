#if (_NETWORK_ || _TDES_AUTH_TOKEN_) && __ANDROID__
using System;
using Android.Content;
using Android.Net;
using AD.Exceptions;
using AD.Plugins.Network.Reachability;
using Java.Net;

namespace AD.Plugins.Network.Droid
{
	public class NetworkReachability : INetworkReachability
	{
		private const int ReachableTimeoutInMilliseconds = 5000;

		private ConnectivityManager _connectivityManager;

		private const string TAG = "AD.Network.NetworkReachability";

		protected ConnectivityManager ConnectivityManager
		{
			get
			{
				_connectivityManager = _connectivityManager ?? (ConnectivityManager)(Android.App.Application.Context.GetSystemService(Context.ConnectivityService));
				return _connectivityManager;
			}
		}

		public bool IsConnected
		{
			get
			{
				try
				{
					var activeConnection = ConnectivityManager.ActiveNetworkInfo;

					return ((activeConnection != null) && activeConnection.IsConnected);
				}
				catch (Exception ex)
				{
					AD.Resolver.Resolve<AD.ILogger>().Exception(
						TAG, ex,
						"Unable to get connected state - do you have ACCESS_NETWORK_STATE permission - error: {0}", ex.ToLongString());
					return false;
				}
			}
		}

		public bool IsHostReachable(string host)
		{
			bool reachable = false;

			if (IsConnected)
			{
				if (!string.IsNullOrEmpty(host))
				{
					// to avoid ICMP/ping issues we don't test ping, but just return true if we have a network here
					// see discussion in https://github.com/MvvmCross/MvvmCross/pull/408
					reachable = true;
				}
			}

			return reachable;
		}

		public bool IsHostPingReachable(string host)
		{
			bool reachable = false;

			if (IsConnected)
			{
				if (!string.IsNullOrEmpty(host))
				{
					try
					{
						reachable = InetAddress.GetByName(host).IsReachable(ReachableTimeoutInMilliseconds);
					}
					catch (UnknownHostException)
					{
						reachable = false;
					}
				}
			}

			return reachable;
		}
	}
}

/*
protected MvxReachabilityStatus GetReachabilityType()
{
    if (IsConnected)
    {
        var wifiState = ConnectivityManager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
        if (wifiState == NetworkInfo.State.Connected)
        {
            // We are connected via WiFi
            return MvxReachabilityStatus.ViaWiFiNetwork;
        }

        var mobile = ConnectivityManager.GetNetworkInfo(ConnectivityType.Mobile).GetState();
        if (mobile == NetworkInfo.State.Connected)
        {
            // We are connected via carrier data
            return MvxReachabilityStatus.ViaCarrierDataNetwork;
        }
    }

    return MvxReachabilityStatus.Not;
}
*/
#endif
