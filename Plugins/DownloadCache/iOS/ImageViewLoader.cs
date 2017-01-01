#if _DL_CACHE_ && __IOS__
namespace AD.Plugins.DownloadCache.iOS
{
	using System;

    using Foundation;
    using UIKit;
	
    public class ImageViewLoader
        : BaseImageViewLoader<UIImage>
    {
        public ImageViewLoader(Func<UIImageView> imageViewAccess, Action afterImageChangeAction = null)
            : base(image =>
            {
                OnUiThread(() => OnImage(imageViewAccess(), image));
                if (afterImageChangeAction != null)
                    OnUiThread(afterImageChangeAction);
            })
        {
        }

        private static void OnImage(UIImageView imageView, UIImage image)
        {
            if (imageView == null ) return;
            imageView.Image = image;
        }
        
        private static void OnUiThread(Action action)
        {
            new NSObject().InvokeOnMainThread(action);
        }
    }
}
#endif