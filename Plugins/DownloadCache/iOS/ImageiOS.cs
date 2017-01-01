#if _DL_CACHE_ && __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace AD.Plugins.DownloadCache.iOS
{
    public class ImageiOS : Image<UIImage>
    {
        public ImageiOS(UIImage rawImage)
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

            var cg = RawImage.CGImage;
            return (int)(cg.BytesPerRow * cg.Height);
        }
    }
}
#endif