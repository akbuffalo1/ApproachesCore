#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public static class FileStoreHelper
    {
        public static IFileStore SafeGetFileStore()
        {
            var fileStore = Resolver.Resolve<IFileStore>();
            if (fileStore != null)
                return fileStore;

            throw new Exception("You must call SetupFile on the File plugin before using the DownloadCache");
        }
    }
}
#endif