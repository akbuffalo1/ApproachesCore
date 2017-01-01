#if __IOS__ && (_DEVICE_INFO_ || _TDES_AUTH_TOKEN_)
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace AD.Plugins.DeviceInfo.iOS
{
    public class DeviceInfoiOS : IDeviceInfo
    {
        public DeviceInfoiOS()
        {
            this.AppVersion = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
            this.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            this.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            this.DeviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            this.Manufacturer = "Apple";
            this.Model = UIDevice.CurrentDevice.Model;
			this.OperatingSystem = UIDevice.CurrentDevice.SystemName;
			this.SystemVersion = UIDevice.CurrentDevice.SystemVersion;
            this.IsFrontCameraAvailable = UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Front);
            this.IsRearCameraAvailable = UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Rear);
            this.IsSimulator = (Runtime.Arch == Arch.SIMULATOR);
        }


        public string AppVersion { get; private set; }
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }
        public string DeviceId { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string OperatingSystem { get; private set; }
		public string SystemVersion { get; private set; }
        public bool IsFrontCameraAvailable { get; private set; }
        public bool IsRearCameraAvailable { get; private set; }
        public bool IsSimulator { get; private set; }
    }
}
#endif