#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AD.Plugins.DownloadCache
{
    public interface ILocalFileImageLoader<T>
    {
        Task<Image<T>> Load(string localPath, bool shouldCache, int width, int height);
    }
}
#endif