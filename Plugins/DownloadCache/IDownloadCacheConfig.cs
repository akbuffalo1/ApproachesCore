#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public interface IDownloadCacheConfig
    {
        string CacheName { get; set; }
        string CacheFolderPath { get; set; }
        int MaxFiles { get; set; }
        TimeSpan MaxFileAge { get; set; }
        int MaxInMemoryFiles { get; set; }
        int MaxInMemoryBytes { get; set; }
        int MaxConcurrentDownloads { get; set; }
        bool DisposeOnRemoveFromCache { get; set; }
    }
}
#endif