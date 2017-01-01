#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public class FileDownloadedEventArgs : EventArgs
    {
        public FileDownloadedEventArgs(string url, string downloadPath)
        {
            DownloadPath = downloadPath;
            Url = url;
        }

        public string Url { get; private set; }
        public string DownloadPath { get; private set; }
    }
}
#endif