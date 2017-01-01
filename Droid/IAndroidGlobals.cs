#if __ANDROID__
using Android.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AD.Droid
{
    public interface IAndroidGlobals
    {
        string ExecutableNamespace { get; }
        Assembly ExecutableAssembly { get; }
        Context ApplicationContext { get; }
    }
}
#endif