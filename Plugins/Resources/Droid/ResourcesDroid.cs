#if __ANDROID__ && _RESOURCES_

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using OSMT.Droid;

namespace AD.Plugins.Resources.Droid
{
    internal class ResourcesDroid : IResources
    {
        private static readonly Dictionary<string, int> KeyToIdMappings;

        static ResourcesDroid()
        {
            KeyToIdMappings = typeof (Resource.String).GetRuntimeFields()
                .ToDictionary(info => info.Name, info => (int) info.GetValue(null));
        }

        public string GetStringTranslationForKey(string key)
        {
            if (KeyToIdMappings.ContainsKey(key))
                return Application.Context.Resources.GetString(KeyToIdMappings[key]);
            return key;
        }
    }
}

#endif