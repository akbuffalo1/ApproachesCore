#if __IOS__
using System;
using UIKit;

namespace AD
{
	public class AppDelegateBase<TSetup> : UIApplicationDelegate where TSetup : AD.ApplicationSetupBase, new()
	{
		public override void FinishedLaunching (UIApplication application)
		{
			ApplicationBase<TSetup>.CheckInitialized();

#if _TDES_AUTH_TOKEN_
            var networkReachability = Resolver.Resolve<INetworkReachability>();

			if (networkReachability.IsConnected)
			{
				var authService = Resolver.Resolve<ITDesAuthService>();
                var authStore = Resolver.Resolve<ITDesAuthStore>();

                bool isFirstTimeRegistration = string.IsNullOrEmpty(authStore.GetAuthData()?.DeviceKey);

                authService.UpdateDevice(
                    (AD.Plugins.TripleDesAuthToken.AuthData authData) => { OnUpdateDeviceSucceed(isFirstTimeRegistration, authData); },
                    (error) => { OnUpdateDeviceFailed(error); });
			}
#endif
        }

#if _TDES_AUTH_TOKEN_
		protected virtual void OnUpdateDeviceSucceed(bool isFirstTimeRegistration, Plugins.TripleDesAuthToken.AuthData authData) { }
		protected virtual void OnUpdateDeviceFailed(Exception ex) { }
#endif
	}
}
#endif

