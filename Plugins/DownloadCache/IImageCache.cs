#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AD.Plugins.DownloadCache
{
    public interface IImageCache<T>
    {
        Task<T> RequestImage(string url);
    }
}
#endif