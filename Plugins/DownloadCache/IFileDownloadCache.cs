#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public interface IFileDownloadCache
    {
        void RequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error);
        void RequestLocalFilePathWithExtension(string httpSource, Action<string> success, Action<Exception> error);
        void ClearAll();
        void Clear(string httpSource);
    }
}
#endif