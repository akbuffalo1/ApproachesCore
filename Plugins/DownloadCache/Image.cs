#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public abstract class Image<T>
    {
        protected Image(T rawImage)
        {
            RawImage = rawImage;
        }

        public T RawImage { get; private set; }

        public abstract int GetSizeInBytes();
    }
}
#endif