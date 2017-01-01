using System;
using AD.TypeExtensions;
using TinyIoC;
using System.Reflection;
#if __ANDROID__
using Android.App;
#endif

namespace AD
{
    public class ApplicationBase<T>
        where T : ApplicationSetupBase, new()
    {
        //private static volatile ApplicationBase<T> _instance;
        private static volatile bool _isInitialized = false;
        private static object _syncRoot = new object();

        /*public static ApplicationBase<T> Instance {
			get {
				if (_instance == null) {
					lock (_syncRoot) {
						if (_instance == null)
							_instance = new ApplicationBase<T> ();
					}
				}
				return _instance;
			}
		}*/

        public static void CheckInitialized()
        {
            if (!_isInitialized)
            {
                lock (_syncRoot)
                {
                    if (!_isInitialized)
                    {

                        _Initialize();
                    }
                    _isInitialized = true;
                }
            }
        }

        private static void _Initialize()
        {
            var ioc = new AD.IoC.TinyIoC.TinyContainer(TinyIoCContainer.Current);

            Resolver.SetResolver(ioc.GetResolver());
            ioc.Register<IDependencyContainer>(ioc);
            //ioc.Register<IADApplication> (_instance);

            Assembly.GetExecutingAssembly().CreatableTypes()
                .EndingWith("Config")
                .AsInterfaces()
                .RegisterAsMultiInstance();
            Assembly.GetExecutingAssembly().CreatableTypes()
                .EndingWith("Service")
#if __ANDROID__
				.DoesNotInherit(typeof(Service))
#endif
                .AsInterfaces()
                .RegisterAsSingleton();
            Assembly.GetExecutingAssembly().CreatableTypes()
                .EndingWith("Utility")
                .AsInterfaces()
                .RegisterAsMultiInstance();
            
            (new T()).Setup(ioc);


            // actions that can be performed only after default initializations
#if _TDES_AUTH_TOKEN_
            var fileStore = Resolver.Resolve<IFileStore>();
            var serializer = Resolver.Resolve<IJsonConverter>();
            var authStore = new Plugins.TripleDesAuthToken.TDesAuthStore();
            if (!fileStore.Exists(Resolver.Resolve<ITDesAuthConfig>().ConfigFile))
            {
                authStore.SetAuthData(new Plugins.TripleDesAuthToken.AuthData());
            }
            else
            {
                string fileContent;
                fileStore.TryReadTextFile(Resolver.Resolve<ITDesAuthConfig>().ConfigFile, out fileContent);
                authStore.AuthToken = fileContent;
            }
            ioc.Register<ITDesAuthStore>(authStore);

#endif
        }

        //public static void Init(IoCContainer ioc) {}
        /*#if __ANDROID__
		protected abstract virtual void InitDroid(IoCContainer ioc);
		#endif
		#if __IOS__
		protected abstract virtual void InitiOS(IOCContainer ioc);
		#endif*/



    }
}

