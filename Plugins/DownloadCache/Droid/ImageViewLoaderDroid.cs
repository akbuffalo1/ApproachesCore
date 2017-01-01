#if _DL_CACHE_ && __ANDROID__
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache.Droid
{
    public class ImageViewLoaderDroid : BaseImageViewLoader<Bitmap>
    {
        public ImageViewLoaderDroid(Func<ImageView> imageViewAccess, Action afterImageChangeAction = null)
            : base(image =>
            {
                OnUiThread(() => OnImage(imageViewAccess(), image));
                if (afterImageChangeAction != null)
                    OnUiThread(afterImageChangeAction);
            })
        {
        }

        private static void OnImage(ImageView imageView, Bitmap image)
        {
            if (imageView == null) return;
            try
            {
                if(image != null)
                    imageView.SetImageBitmap(image);
            }
            catch (Exception ex) {
                Console.WriteLine(string.Format("[ImageViewLoaderDroid] : {0}",ex.Message));
            }
        }

        private static void OnUiThread(Action action)
        {
            using (var h = new Handler(Looper.MainLooper))
            {
                h.Post(() =>
                {
                    action();
                });
            }
        }
    }
}
#endif