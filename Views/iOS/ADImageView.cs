#if _DL_CACHE_ && __IOS__
namespace AD.Views.iOS
{
	using AD.Plugins.DownloadCache.iOS;
	using AD.Plugins.DownloadCache;
	using AD;
	
    using System;
    using System.ComponentModel;
    using CoreGraphics;

    using Foundation;

    using UIKit;

    [Register("ADImageView"), DesignTimeVisible(true)]
    public class ADImageView
        : UIImageView
    {
        private AD.Plugins.DownloadCache.iOS.ImageViewLoader _imageHelper;

        public string ImageUrl
        {
            get { return this._imageHelper.ImageUrl; }
            set { this._imageHelper.ImageUrl = value; }
        }

        public string DefaultImagePath
        {
            get { return this._imageHelper.DefaultImagePath; }
            set { this._imageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return this._imageHelper.ErrorImagePath; }
            set { this._imageHelper.ErrorImagePath = value; }
        }

        public ADImageView(Action afterImageChangeAction = null)
        {
            this.InitializeImageHelper(afterImageChangeAction);
        }
		
        public ADImageView(IntPtr handle)
            : base(handle)
        {
			InitializeImageHelper ();	
        }

        public ADImageView(CGRect frame, Action afterImageChangeAction = null)
            : base(frame)
        {
			InitializeImageHelper (afterImageChangeAction);	
        }

        private void InitializeImageHelper(Action afterImageChangeAction = null)
        {
			this._imageHelper = new ImageViewLoader(() => this, afterImageChangeAction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
#endif
