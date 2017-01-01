#if _DL_CACHE_
namespace AD.Plugins.DownloadCache
{
	using System;
	using AD.Plugins.DownloadCache;
	using AD.WeakSubscription;
	
	public abstract class BaseImageViewLoader<TImage>
		: IDisposable
		where TImage : class
	{
		private readonly IImageHelper<TImage> _imageHelper;
		private readonly Action<TImage> _imageSetAction;
		private IDisposable _subscription;

		protected BaseImageViewLoader(Action<TImage> imageSetAction)
		{
			this._imageSetAction = imageSetAction;
			_imageHelper = new DynamicImageHelper<TImage> (); // Resolver.Resolve<IImageHelper<TImage>> ();
			var eventInfo = this._imageHelper.GetType().GetEvent("ImageChanged");
			this._subscription = eventInfo.WeakSubscribe<TImage>(this._imageHelper, ImageHelperOnImageChanged);
		}

		~BaseImageViewLoader()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Note - this is public because we use it in weak referenced situations
		public virtual void ImageHelperOnImageChanged(object sender, ValueEventArgs<TImage> mvxValueEventArgs)
		{
			this._imageSetAction(mvxValueEventArgs.Value);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._subscription != null)
				{
					this._subscription.Dispose();
					this._subscription = null;
				}
			}
		}

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

		public int MaxHeight
		{
			get { return _imageHelper.MaxHeight; }
			set { _imageHelper.MaxHeight = value; }
		}

		public int MaxWidth
		{
			get { return _imageHelper.MaxWidth; }
			set { _imageHelper.MaxWidth = value; }
		}
	}
}
#endif