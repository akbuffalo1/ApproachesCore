using System;
using System.Diagnostics;

namespace AD
{
	public abstract class ApplicationSetupBase
	{
		public ApplicationSetupBase() { }

		protected ILogger Logger { get; set; }
		private const string TAG = "AD.ApplicationSetupBase";

        public virtual void Setup(IDependencyContainer ioc)
        {
            SetupLogger(ioc);
            SetupDialogProvider(ioc);

#if __ANDROID__
            SetupCurrentActivity(ioc);
#endif

#if _JSON_ || _TDES_AUTH_TOKEN_
            SetupJson(ioc);
#endif
#if _PERMISSIONS_ || (__ANDROID__ && _CONTACTS_)
            SetupPermissions(ioc);
#endif
#if _FILE_ || _TDES_AUTH_TOKEN_
            SetupFile(ioc);
#endif
#if _NETWORK_ || _TDES_AUTH_TOKEN_
            SetupNetwork(ioc);
#endif
#if _LOCATION_
			SetupLocation(ioc);
#endif
#if _ENCRYPTION_ || _TDES_AUTH_TOKEN_
            SetupEncryption(ioc);
#endif
#if _TDES_AUTH_TOKEN_
            SetupTDesAuthTokenAppConfig(ioc);
#endif
#if _DL_CACHE_
            SetupDownloadCache(ioc);
#endif
#if _DEVICE_INFO_ || _TDES_AUTH_TOKEN_
            SetupDeviceInfo(ioc);
#endif
#if _CALENDARS_
			SetupCalendars(ioc);
#endif
#if _CONTACTS_
			SetupContacts(ioc);
#endif
#if _RESOURCES_
			SetupResources(ioc);
#endif
#if _PHONE_CALL_
            SetupPhone(ioc);
#endif
#if _MAIL_
			SetupMail(ioc);
#endif
#if _SMS_
			SetupSMS(ioc);
#endif
#if _LOCATION_
			SetupLocationMap(ioc);
#endif

#if _OAUTH_
            SetupOAuth(ioc);
#endif
        }

#if __ANDROID__
		public virtual void SetupLogger(IDependencyContainer ioc)
		{
			Logger = new AD.Droid.Logger();
			ioc.Register<ILogger>(Logger);
		}
#endif

#if __ANDROID__
		public virtual void SetupCurrentActivity(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ICurrentActivity");
			ioc.RegisterAsSingleton<AD.Plugins.CurrentActivity.ICurrentActivity, AD.Plugins.CurrentActivity.CurrentActivity>();
		}
#endif

#if __IOS__
		public virtual void SetupLogger(IDependencyContainer ioc)
		{
			Logger = new AD.GenericLogger();
			ioc.Register<ILogger>(Logger);
			Logger.Warn(TAG, "Using generic logger");
		}
#endif

#if __IOS__
		public virtual void SetupDialogProvider(IDependencyContainer ioc)
		{
			ioc.Register<IDialogProvider, AD.iOS.DialogProvider>();
			Logger.Debug(TAG, "Register IDialogProvider");
		}
#elif __ANDROID__
		public virtual void SetupDialogProvider(IDependencyContainer ioc)
		{
			ioc.Register<IDialogProvider, AD.Droid.DialogProvider>();
			Logger.Debug(TAG, "Register IDialogProvider");
		}
#endif
#if WINDOWS_PHONE
		public virtual void SetupLogger(IDependencyContainer ioc)
		{
			Logger = new AD.GenericLogger();
			ioc.Register<ILogger> (Logger);
			Logger.Warn(TAG, "Using generic logger");
		}
#endif

#if _JSON_ || _TDES_AUTH_TOKEN_
		protected void SetupJson (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register IJsonConverter");
			ioc.Register<IJsonConverter, AD.Plugins.Json.JsonConverter> ();
            ioc.Register<AD.Plugins.Json.IJsonFileReader, AD.Plugins.Json.JsonFileReader>();
			var config = Resolver.Resolve<IJsonConfig> ();
			if (config == null) {
				config = new AD.Plugins.Json.JsonConfigDefault ();
				Logger.Debug (TAG, "Using configuration in **JsonConfigDefault**");
			} else {
				Logger.Debug (TAG, "Using configuration in {0}", config.GetType ().ToString ());
			}
			if (config.RegisterAsTextSerializer) {
				Logger.Debug (TAG, "- RegisterAsTextSerializer: {0}", config.RegisterAsTextSerializer);
				ioc.Register<ITextSerializer, AD.Plugins.Json.JsonConverter> ();
			}
		}
#endif

#if __ANDROID__ && (_PERMISSIONS_ || _CONTACTS_)
		protected void SetupPermissions(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IPermissions");
			ioc.Register<AD.Plugins.Permissions.IPermissions, AD.Plugins.Permissions.Droid.PermissionsDroid>();
		}
#endif

#if __IOS__ && _PERMISSIONS_
		protected void SetupPermissions (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register IPermissions");
			ioc.Register<AD.Plugins.Permissions.IPermissions, AD.Plugins.Permissions.iOS.PermissionsiOS>();
		}
#endif

#if __ANDROID__ && (_FILE_ || _TDES_AUTH_TOKEN_)
		protected void SetupFile(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IFileStore");
			ioc.Register<IFileStore, AD.Plugins.File.Droid.FileStoreDroid>();
		}
#endif

#if __IOS__ && (_FILE_ || _TDES_AUTH_TOKEN_)
		protected void SetupFile (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register IFileStore");
			ioc.Register<IFileStore, AD.Plugins.File.iOS.FileStoreiOS> ();
		}
#endif

#if WINDOWS_PHONE && (_FILE_ || _TDES_AUTH_TOKEN_)
		protected void SetupFile(IDependencyContainer ioc)
		{
		Logger.Warn(TAG, "**FILE plugin not yet available for Windows Phone**");
		}
#endif

#if __ANDROID__ && (_NETWORK_ || _TDES_AUTH_TOKEN_)
		protected void SetupNetwork(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register INetworkReachability");
			ioc.Register<INetworkReachability, AD.Plugins.Network.Droid.NetworkReachability>();
			Logger.Debug(TAG, "Register IRestClient");
			ioc.Register<IRestClient, AD.Plugins.Network.Rest.JsonRestClient>();
			Logger.Debug(TAG, "Register IJsonRestClient");
			ioc.Register<IJsonRestClient, AD.Plugins.Network.Rest.JsonRestClient>();
			Logger.Debug(TAG, "Register IHttpServerConfig");
			var config = Resolver.Resolve<IHttpServerConfig>();
			if (config == null)
			{
				ioc.Register<IHttpServerConfig, AD.Plugins.Network.HttpServerConfigDefault>();
			}
#if DEBUG
			config = Resolver.Resolve<IHttpServerConfig>();
			Logger.Debug(TAG, "- Server: {0}", config.Server);
			Logger.Debug(TAG, "- Port: {0}", config.Port);
			Logger.Debug(TAG, "- Protocol: {0}", config.Protocol);
#endif
			Logger.Debug(TAG, "Register IApiClient");
#if _TDES_AUTH_TOKEN_
			ioc.Register<IApiClient, AD.Plugins.TripleDesAuthToken.TDesAuthApiClient>();
#else
			ioc.Register<IApiClient, AD.Plugins.Network.Rest.ApiClient> ();
#endif
			ioc.Register<IAuthTokenApiClient, AD.Plugins.Network.AuthTokenApiClient>();
		}
#endif

#if __IOS__ && (_NETWORK_ || _TDES_AUTH_TOKEN_)
		protected void SetupNetwork (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register INetworkReachability");
			ioc.Register<INetworkReachability, AD.Plugins.Network.iOS.NetworkReachabilityiOS> ();
			Logger.Debug (TAG, "Register IRestClient");
			ioc.Register<IRestClient, AD.Plugins.Network.Rest.JsonRestClient> ();
			Logger.Debug (TAG, "Register IJsonRestClient");
			ioc.Register<IJsonRestClient, AD.Plugins.Network.Rest.JsonRestClient> ();
			Logger.Debug (TAG, "Register IHttpServerConfig");
			var config = Resolver.Resolve<IHttpServerConfig> ();
			if (config == null) {
				ioc.Register<IHttpServerConfig, AD.Plugins.Network.HttpServerConfigDefault> ();
			}
#if DEBUG
			config = Resolver.Resolve<IHttpServerConfig> ();
			Logger.Debug (TAG, "- Server: {0}", config.Server);
			Logger.Debug (TAG, "- Port: {0}", config.Port);
			Logger.Debug (TAG, "- Protocol: {0}", config.Protocol);
#endif
			Logger.Debug (TAG, "Register IApiClient");
#if _TDES_AUTH_TOKEN_
			ioc.Register<IApiClient, AD.Plugins.TripleDesAuthToken.TDesAuthApiClient>();
#else
			ioc.Register<IApiClient, AD.Plugins.Network.Rest.ApiClient> ();
			
			Logger.Debug (TAG, "Register IAuthTokenApiClient");
			ioc.Register<IAuthTokenApiClient, AD.Plugins.Network.AuthTokenApiClient>();
#endif
		}
#endif

#if WINDOWS_PHONE && (_NETWORK_ || _TDES_AUTH_TOKEN_)
		protected void SetupNetwork(IDependencyContainer ioc)
		{
		Logger.Warn(TAG, "**NETWORK plugin not yet available for Windows Phone**");
		}
#endif

#if __ANDROID__ && _LOCATION_
		protected void SetupLocation(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ILocationWatcher");
			ioc.Register<ILocationWatcher, AD.Plugins.Location.Droid.AndroidLocationWatcher>();
		}
#endif

#if __IOS__ && _LOCATION_
		protected void SetupLocation(IDependencyContainer ioc)
		{
			Logger.Warn(TAG, "**LOCATION plugin not yet available for iOS**");
		}
#endif

#if WINDOWS_PHONE && _LOCATION_
		protected void SetupLocation(IDependencyContainer ioc)
		{
		Logger.Warn(TAG, "**LOCATION plugin not yet available for Windows Phone**");
		}
#endif

#if _ENCRYPTION_ || _TDES_AUTH_TOKEN_
		protected void SetupEncryption (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register IEncryptionService");
			ioc.Register<AD.Plugins.Encryption.IEncryptionService, AD.Plugins.Encryption.EncryptionService> ();
		}
#endif

#if _TDES_AUTH_TOKEN_
		protected void SetupTDesAuthTokenAppConfig (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register TripleDesAuthToken");

			//ioc.RegisterAsSingleton<ITDesAuthStore, AD.Plugins.TripleDesAuthToken.TDesAuthStore>();

			if (Resolver.Resolve<ITDesAuthConfig>() == null)
			{
				ioc.Register<ITDesAuthConfig, AD.Plugins.TripleDesAuthToken.TDesAuthConfigDefault>();
			}

			if (Resolver.Resolve<ITDesAuthTokenServerConfig>() == null)
			{
				ioc.Register<ITDesAuthTokenServerConfig, AD.Plugins.TripleDesAuthToken.TDesAuthTokenServerConfigDefault>();
			}

			ioc.Register<ITDesAuthService, AD.Plugins.TripleDesAuthToken.TDesAuthService>();
		}
#endif

#if __ANDROID__ && _DL_CACHE_
		protected void SetupDownloadCache(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IDownloadCacheConfig check");
			if(Resolver.Resolve<AD.Plugins.DownloadCache.IDownloadCacheConfig>() == null)
			{
				ioc.Register<AD.Plugins.DownloadCache.IDownloadCacheConfig, AD.Plugins.DownloadCache.Droid.DownloadCacheConfigDefaultDroid>();
			}

			Logger.Debug(TAG, "Register MainThreadDispatcher");
			ioc.Register<AD.IMainThreadDispatcher, AD.Plugins.DownloadCache.Droid.MainThreadDispatcherDroid>();

			Logger.Debug(TAG, "Register IHttpFileDownloader");
			ioc.Register<AD.Plugins.DownloadCache.IHttpFileDownloader, AD.Plugins.DownloadCache.HttpFileDownloader>();
			Logger.Debug(TAG, "Register IFileDownloadCache");
			ioc.Register<AD.Plugins.DownloadCache.IFileDownloadCache, AD.Plugins.DownloadCache.FileDownloadCache>();
			Logger.Debug(TAG, "Register IImageCache");
			ioc.Register<AD.Plugins.DownloadCache.IImageCache<Android.Graphics.Bitmap>, AD.Plugins.DownloadCache.ImageCache<Android.Graphics.Bitmap>>();
			Logger.Debug(TAG, "Register IImageHelper");
			ioc.Register<AD.Plugins.DownloadCache.IImageHelper<Android.Graphics.Bitmap>, AD.Plugins.DownloadCache.DynamicImageHelper<Android.Graphics.Bitmap>>();
			Logger.Debug(TAG, "Register ILocalFileImageLoader");
			ioc.Register<AD.Plugins.DownloadCache.ILocalFileImageLoader<Android.Graphics.Bitmap>, AD.Plugins.DownloadCache.Droid.LocalFileImageLoaderDroid>();
		}
#endif

#if __IOS__ && _DL_CACHE_
		protected void SetupDownloadCache(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IDownloadCacheConfig check");
			if(Resolver.Resolve<AD.Plugins.DownloadCache.IDownloadCacheConfig>() == null)
			{
				ioc.Register<AD.Plugins.DownloadCache.IDownloadCacheConfig, AD.Plugins.DownloadCache.iOS.DownloadCacheConfigDefaultiOS>();
			}
			
			Logger.Debug (TAG, "Register MainThreadDispatcher");
			ioc.Register<AD.IMainThreadDispatcher, AD.Plugins.DownloadCache.iOS.UIMainThreadDispatcher> ();
			
			Logger.Debug(TAG, "Register IHttpFileDownloader");
			ioc.Register<AD.Plugins.DownloadCache.IHttpFileDownloader, AD.Plugins.DownloadCache.HttpFileDownloader>();
			Logger.Debug(TAG, "Register IFileDownloadCache");
			ioc.Register<AD.Plugins.DownloadCache.IFileDownloadCache, AD.Plugins.DownloadCache.FileDownloadCache>();
			Logger.Debug(TAG, "Register IImageCache");
			ioc.Register<AD.Plugins.DownloadCache.IImageCache<UIKit.UIImage>, AD.Plugins.DownloadCache.ImageCache<UIKit.UIImage>>();
			Logger.Debug(TAG, "Register IImageHelper");
			ioc.Register<AD.Plugins.DownloadCache.IImageHelper<UIKit.UIImage>, AD.Plugins.DownloadCache.DynamicImageHelper<UIKit.UIImage>>();
			Logger.Debug(TAG, "Register ILocalFileImageLoader");
			ioc.Register<AD.Plugins.DownloadCache.ILocalFileImageLoader<UIKit.UIImage>, AD.Plugins.DownloadCache.iOS.LocalFileImageLoaderiOS>();
		}
#endif

#if __ANDROID__ && (_DEVICE_INFO_ || _TDES_AUTH_TOKEN_)
		protected void SetupDeviceInfo(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IDeviceInfo");
			ioc.Register<AD.Plugins.DeviceInfo.IDeviceInfo, AD.Plugins.DeviceInfo.Droid.DeviceInfoDroid>();
		}
#endif

#if __IOS__ && (_DEVICE_INFO_ || _TDES_AUTH_TOKEN_)
		protected void SetupDeviceInfo (IDependencyContainer ioc)
		{
			Logger.Debug (TAG, "Register IDeviceInfo");
			ioc.Register<AD.Plugins.DeviceInfo.IDeviceInfo, AD.Plugins.DeviceInfo.iOS.DeviceInfoiOS> ();
		}
#endif

#if __ANDROID__ && _CALENDARS_
		protected void SetupCalendars(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ICalendars");
			ioc.Register<AD.Plugins.Calendars.ICalendars, AD.Plugins.Calendars.Droid.CalendarsDroid>();
		}
#endif

#if __IOS__ && _CALENDARS_
		protected void SetupCalendars(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ICalendars");
			ioc.Register<AD.Plugins.Calendars.ICalendars, AD.Plugins.Calendars.iOS.CalendarsiOS>();
		}
#endif

#if __ANDROID__ && _CONTACTS_
		protected void SetupContacts(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IContacts");

			ioc.Register<AD.Plugins.Contacts.IContacts, AD.Plugins.Contacts.Droid.ContactsDroid>();
		}
#endif

#if __IOS__ && _CONTACTS_
		protected void SetupContacts(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IContacts");
			ioc.Register<AD.Plugins.Contacts.IContacts, AD.Plugins.Contacts.iOS.ContactsiOS>();
		}
#endif

#if __ANDROID__ && _RESOURCES_
		protected void SetupResources(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IResources");
			ioc.Register<AD.Plugins.Resources.IResources, AD.Plugins.Resources.Droid.ResourcesDroid>();
		}
#endif

#if __IOS__ && _RESOURCES_
		protected void SetupResources(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IResources");
			ioc.Register<AD.Plugins.Resources.IResources, AD.Plugins.Resources.iOS.ResourcesiOS>();
		}
#endif

#if __IOS__ && _PHONE_CALL_
		protected void SetupPhone(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IPhoneCallTask");
			ioc.Register<IPhoneCallTask, AD.Plugins.PhoneCall.iOS.PhoneCallTask>();
		}
#endif

#if __ANDROID__ && _PHONE_CALL_
		protected void SetupPhone(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IPhoneCallTask");
			ioc.Register<IPhoneCallTask, AD.Plugins.PhoneCall.Droid.PhoneCallTaskDroid>();
		}
#endif
#if __IOS__ && _MAIL_
		protected void SetupMail(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IComposeEmailTask");
			ioc.Register<IComposeEmailTaskEx, AD.Plugins.Email.iOS.ComposeEmailTask>();
		}
#endif
#if __ANDROID__ && _MAIL_
		protected void SetupMail(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register IComposeEmailTask");
			ioc.Register<IComposeEmailTaskEx, AD.Plugins.Email.Droid.ComposeEmailTask>();
		}
#endif
#if __IOS__ && _SMS_
		protected void SetupSMS(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ISmsTask");
			ioc.Register<ISmsTask, AD.Plugins.Sms.iOS.SmsTask>();
		}
#endif
#if __ANDROID__ && _SMS_
		protected void SetupSMS(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ISmsTask");
			ioc.Register<ISmsTask, AD.Plugins.Sms.Droid.SmsTaskDroid>();
		}
#endif
#if __IOS__ && _LOCATION_
		protected void SetupLocationMap(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ILocationMap");
			ioc.Register<ILocationMap, AD.Plugins.LocationMap.iOS.LocationMap>();
		}
#endif
#if __ANDROID__ && _LOCATION_
		protected void SetupLocationMap(IDependencyContainer ioc)
		{
			Logger.Debug(TAG, "Register ILocationMap");
			ioc.Register<ILocationMap, AD.Plugins.LocationMap.Droid.LocationMapDroid>();
		}
#endif
#if _OAUTH_ 
    protected void SetupOAuth(IDependencyContainer ioc)
        {
            Logger.Debug(TAG, "Register OAuthEntities");
            ioc.Register<AD.Plugins.OAuth.IOAuthServiceProvider, AD.Plugins.OAuth.OAuthServiceProvider>();
            ioc.Register<AD.Plugins.OAuth.IOAuthAccountHelper, AD.Plugins.OAuth.Droid.OAuthAccountHelper>();
        }
#endif
	}
}