#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Android.Content;

namespace AD.Droid
{
    public class AndroidGlobals : IAndroidGlobals
    {
        private readonly Context _applicationContext;

        public AndroidGlobals(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Context ApplicationContext => _applicationContext;

        public Assembly ExecutableAssembly => this.GetType().Assembly;

        public string ExecutableNamespace => this.GetType().Namespace;
    }
}
#endif