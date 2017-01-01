#if _DL_CACHE_ && __ANDROID__
using Android.Graphics;
using Android.Support.V4.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache.Droid
{
    public class ImageDroid : Image<Bitmap>
    {
        public ImageDroid(Bitmap rawImage)
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

            return BitmapCompat.GetAllocationByteCount(RawImage);
        }
    }
}
#endif