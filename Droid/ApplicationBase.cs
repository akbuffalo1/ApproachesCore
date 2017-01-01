#if __ANDROID__
using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;

namespace AD.Droid
{
    public class ApplicationBase<TSetup> : Application, Application.IActivityLifecycleCallbacks where TSetup : ApplicationSetupBase, new()
    {
        public ApplicationBase(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            AD.ApplicationBase<TSetup>.CheckInitialized();

#if _TDES_AUTH_TOKEN_
            _updateDevice();
#endif
        }



#if _TDES_AUTH_TOKEN_
        protected virtual void OnUpdateDeviceSucceed(bool isFirstTimeRegistration, Plugins.TripleDesAuthToken.AuthData authData) { }
        protected virtual void OnUpdateDeviceFailed(Exception ex) { }
        private async void _updateDevice()
        {
            await Task.Run(() =>
            {
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
            });
        }
#endif

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
#if _CURRENT_ACTIVITY_
            Resolver.Resolve<AD.Plugins.CurrentActivity.ICurrentActivity>().Activity = activity;
#endif
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
#if _CURRENT_ACTIVITY_
            Resolver.Resolve<AD.Plugins.CurrentActivity.ICurrentActivity>().Activity = activity;
#endif
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
#if _CURRENT_ACTIVITY_
            Resolver.Resolve<AD.Plugins.CurrentActivity.ICurrentActivity>().Activity = activity;
#endif
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}
#endif