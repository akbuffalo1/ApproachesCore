#if _RESOURCES_

namespace AD.Plugins.Resources
{
    public interface IResources
    {
        string GetStringTranslationForKey(string key);
    }
}

#endif