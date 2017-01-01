﻿#if __ANDROID__ && (_DEVICE_INFO_ || _TDES_AUTH_TOKEN_)
using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Telephony;
using Android.Provider;
using B = Android.OS.Build;

namespace AD.Plugins.DeviceInfo.Droid
{
    public class DeviceInfoDroid : IDeviceInfo
    {
        private readonly Lazy<string> deviceId;
        private readonly Lazy<int> screenHeight;
        private readonly Lazy<int> screenWidth;
        private readonly Lazy<string> appVersion;


        public DeviceInfoDroid()
        {
            this.appVersion = new Lazy<string>(() =>
                Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName
            );
            this.screenHeight = new Lazy<int>(() => {
                var d = Resources.System.DisplayMetrics;
                return (int)(d.HeightPixels / d.Density);
            });
            this.screenWidth = new Lazy<int>(() => {
                var d = Resources.System.DisplayMetrics;
                return (int)(d.WidthPixels / d.Density);
            });
            this.deviceId = new Lazy<string>(() => {
                var tel = (TelephonyManager)Application.Context.ApplicationContext.GetSystemService(Context.TelephonyService);
                // Returns DeviceId from TelephonyManager for phones or AndroidId from Settings.Secure for tablets
                return tel.DeviceId ?? Settings.Secure.GetString(Application.Context.ApplicationContext.ContentResolver, Settings.Secure.AndroidId);
            });
        }


        public string AppVersion
        {
            get { return this.appVersion.Value; }
        }


        public int ScreenHeight
        {
            get { return this.screenHeight.Value; }
        }


        public int ScreenWidth
        {
            get { return this.screenWidth.Value; }
        }


        public string DeviceId
        {
            get { return this.deviceId.Value; }
        }


        public string Manufacturer
        {
            get { return B.Manufacturer; }
        }


        public string Model
        {
            get { return B.Model; }
        }


        private string os;
        public string OperatingSystem
        {
            get
            {
                this.os = this.os ?? String.Format("{0} - SDK: {1}", B.VERSION.Release, B.VERSION.SdkInt);
                return this.os;
            }
        }


        public bool IsFrontCameraAvailable
        {
            get { return Application.Context.ApplicationContext.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront); }
        }


        public bool IsRearCameraAvailable
        {
            get { return Application.Context.ApplicationContext.PackageManager.HasSystemFeature(PackageManager.FeatureCamera); }
        }


        public bool IsSimulator
        {
            get { return B.Product.Equals("google_sdk"); }
        }

		public string SystemVersion
        {
			get { return B.VERSION.SdkInt + " ("+ B.VERSION.Release+")"; }
        }
		
    }
}
#endif