#if _DL_CACHE_ && __ANDROID__
using AD.Plugins.DownloadCache;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Views.Android
{
    public class ADImageView : ImageView
    {
        private AD.Plugins.DownloadCache.Droid.ImageViewLoaderDroid _imageHelper;

        public string ImageUrl
        {
            get
            {
                return this.ImageHelper?.ImageUrl;
            }
            set
            {
                if (this.ImageHelper == null) return;
                this.ImageHelper.ImageUrl = value;
            }
        }

        public string DefaultImagePath
        {
            get { return this.ImageHelper.DefaultImagePath; }
            set { this.ImageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return this.ImageHelper.ErrorImagePath; }
            set { this.ImageHelper.ErrorImagePath = value; }
        }

        public override void SetMaxHeight(int maxHeight)
        {
            if (this.ImageHelper != null)
                this.ImageHelper.MaxHeight = maxHeight;

            base.SetMaxHeight(maxHeight);
        }

        public override void SetMaxWidth(int maxWidth)
        {
            if (this.ImageHelper != null)
                this.ImageHelper.MaxWidth = maxWidth;

            base.SetMaxWidth(maxWidth);
        }

        protected AD.Plugins.DownloadCache.Droid.ImageViewLoaderDroid ImageHelper
        {
            get
            {
                if (this._imageHelper == null)
                {
                    this._imageHelper = new AD.Plugins.DownloadCache.Droid.ImageViewLoaderDroid(() => this, null);
                }
                return this._imageHelper;
            }
        }

        public ADImageView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {

        }

        public ADImageView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        { }

        public ADImageView(Context context)
            : this(context, null)
        { }

        protected ADImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
#endif