#if _DL_CACHE_ && __ANDROID__
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache.Droid
{
    public class DownloadCacheConfigDefaultDroid : IDownloadCacheConfig
    {
        public string CacheName { get; set; }
        public string CacheFolderPath { get; set; }
        public int MaxFiles { get; set; }
        public TimeSpan MaxFileAge { get; set; }
        public int MaxInMemoryFiles { get; set; }
        public int MaxInMemoryBytes { get; set; }
        public int MaxConcurrentDownloads { get; set; }
        public bool DisposeOnRemoveFromCache { get; set; }

        public DownloadCacheConfigDefaultDroid()
        {
            CacheName = "_ADPictures";
            CacheFolderPath = "_Caches/AD.Pictures/";
            MaxFiles = 500;
            MaxFileAge = TimeSpan.FromDays(7);
            MaxInMemoryBytes = 4000000; // 4 MB
            MaxInMemoryFiles = 30;
            MaxConcurrentDownloads = 10;
            DisposeOnRemoveFromCache = true;
        }
    }
}
#endif