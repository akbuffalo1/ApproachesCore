#if _DEVICE_INFO_ || _TDES_AUTH_TOKEN_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DeviceInfo
{
    public interface IDeviceInfo
    {
        int ScreenHeight { get; }
        int ScreenWidth { get; }
        string AppVersion { get; }
        string DeviceId { get; }
        string Manufacturer { get; }
        string Model { get; }
        string OperatingSystem { get; }
		string SystemVersion { get; }
        bool IsFrontCameraAvailable { get; }
        bool IsRearCameraAvailable { get; }
        bool IsSimulator { get; }
    }
}
#endif