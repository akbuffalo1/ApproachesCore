#if _DL_CACHE_ && __IOS__
namespace AD.Views.iOS
{
    using System;

    using AD.Platform;
    using AD.Platform.Core;
	using AD.Plugins.DownloadCache;
	
    using UIKit;

    public class ADImageViewWrapper
        : IDisposable
    {
        private readonly Func<UIImageView> _imageView;
        private readonly IImageHelper<UIImage> _imageHelper;

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

        public ADImageViewWrapper(Func<UIImageView> imageView)
        {
            this._imageView = imageView;
			this._imageHelper = new DynamicImageHelper<UIImage> (); // AD.Resolver.Resolve<IImageHelper<UIImage>>();
            this._imageHelper.ImageChanged += this.ImageHelperOnImageChanged;
        }

        ~ADImageViewWrapper()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ImageHelperOnImageChanged(object sender, ValueEventArgs<UIImage> mvxValueEventArgs)
        {
			var imageView = this._imageView ();
			if (imageView != null && mvxValueEventArgs.Value != null) {
				new Foundation.NSObject ()
					.InvokeOnMainThread (() => imageView.Image = mvxValueEventArgs.Value);
			}
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper.Dispose();
            }
        }
    }
}
#endif