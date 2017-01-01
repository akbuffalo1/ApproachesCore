#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public static class ObjectExtensions
    {
        public static void DisposeIfDisposable(this object thing)
        {
            var disposable = thing as IDisposable;
            disposable?.Dispose();
        }
    }
}
#endif