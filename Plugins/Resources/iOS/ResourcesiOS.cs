#if __IOS__ && _RESOURCES_
using System;
using System.Collections.Generic;
using System.Text;
using Foundation;

namespace AD.Plugins.Resources.iOS
{
    internal class ResourcesiOS: IResources
    {
        public string GetStringTranslationForKey(string key)
        {
            return NSBundle.MainBundle.LocalizedString(key, "");
        }
    }
}

#endif