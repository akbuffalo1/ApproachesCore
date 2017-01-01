#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public interface IHttpFileDownloader
    {
        void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error);
    }
}
#endif