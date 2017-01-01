#if _DL_CACHE_
using AD.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AD.Plugins.DownloadCache
{
    public class DynamicImageHelper<T>
        : IImageHelper<T>
        where T : class
    {
        private const string TAG = "AD.Plugins.DownloadCache.DynamicImageHelper";

        #region ImageState enum

        public enum ImageState
        {
            DefaultShown,
            ErrorShown,
            HttpImageShown
        }

        #endregion ImageState enum

        private ImageState _currentImageState = ImageState.DefaultShown;

        private CancellationTokenSource _cancellationSource;

        private string _defaultImagePath;

        private string _errorImagePath;

        private string _imageUrl;

        public string DefaultImagePath
        {
            get { return _defaultImagePath; }
            set
            {
                if (_defaultImagePath == value)
                    return;
                _defaultImagePath = value;
                OnImagePathChanged().ConfigureAwait(false);

                if (string.IsNullOrEmpty(_errorImagePath))
                    ErrorImagePath = value;
            }
        }

        public string ErrorImagePath
        {
            get { return _errorImagePath; }
            set
            {
                if (_errorImagePath == value)
                    return;
                _errorImagePath = value;
                OnImagePathChanged().ConfigureAwait(false);
            }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value)
                    return;
                _imageUrl = value;
                RequestImageAsync(_imageUrl).ConfigureAwait(false);
            }
        }

#region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

#endregion IDisposable Members

        ~DynamicImageHelper()
        {
            Dispose(false);
        }

        public event EventHandler<ValueEventArgs<T>> ImageChanged;

        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }

        private void FireImageChanged(T image)
        {
            var handler = ImageChanged;
            handler?.Invoke(this, new ValueEventArgs<T>(image));
        }

        private async Task RequestImageAsync(string imageSource)
        {
            if (_cancellationSource != null)
            {
                _cancellationSource.Cancel();
                _cancellationSource = null;
            }

            var cancelTokenSource = new CancellationTokenSource();
            var cancelToken = cancelTokenSource.Token;
            _cancellationSource = cancelTokenSource;

            FireImageChanged(null);
            var logger = Resolver.Resolve<ILogger>();

            if (string.IsNullOrEmpty(imageSource))
            {
                await ShowDefaultImage().ConfigureAwait(false);
                return;
            }

            if (imageSource.ToUpper().StartsWith("HTTP"))
            {
                await NewHttpImageRequestedAsync().ConfigureAwait(false);

                var error = false;
                try
                {
                    var cache = Resolver.Resolve<IImageCache<T>>();
                    var image = await cache.RequestImage(imageSource).ConfigureAwait(false);

                    if (cancelToken.IsCancellationRequested)
                    {
                        return;
                    }

                    if (image == null)
                        await ShowErrorImage().ConfigureAwait(false);
                    else
                        NewImageAvailable(image);
                }
                catch (Exception ex)
                {
                    logger.Error(TAG, string.Format("failed to download image {0} : {1}", imageSource, ex.ToLongString()));
                    error = true;
                }

                if (error)
                    await HttpImageErrorSeenAsync().ConfigureAwait(false);
            }
            else
            {
                try
                {
                    var image = await ImageFromLocalFileAsync(imageSource).ConfigureAwait(false);

                    if (cancelToken.IsCancellationRequested)
                    {
                        return;
                    }

                    if (image == null)
                        await ShowErrorImage().ConfigureAwait(false);
                    else
                        NewImageAvailable(image);
                }
                catch (Exception ex)
                {
                    logger.Error(TAG, ex.Message);
                }
            }
        }

        private Task OnImagePathChanged()
        {
            switch (_currentImageState)
            {
                case ImageState.ErrorShown:
                    return ShowErrorImage();

                default:
                    return ShowDefaultImage();
            }
        }

        private Task ShowDefaultImage()
        {
            return ShowLocalFileAsync(_defaultImagePath);
        }

        private Task ShowErrorImage()
        {
            return ShowLocalFileAsync(_errorImagePath);
        }

        private async Task ShowLocalFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                FireImageChanged(null);
            }
            else
            {
                FireImageChanged(null);
                var logger = Resolver.Resolve<ILogger>();

                try
                {
                    var localImage = await ImageFromLocalFileAsync(filePath).ConfigureAwait(false);
                    if (localImage == null)
                       logger.Warn(TAG, string.Format("Failed to load local image for filePath {0}", filePath));

                    FireImageChanged(localImage);
                }
                catch (Exception ex)
                {
                    logger.Error(TAG, ex.Message);
                }
            }
        }

        private async Task<T> ImageFromLocalFileAsync(string path)
        {
            var loader = Resolver.Resolve<ILocalFileImageLoader<T>>();
            var img = await loader.Load(path, true, MaxWidth, MaxHeight).ConfigureAwait(false);
            return img.RawImage;
        }

        private Task NewHttpImageRequestedAsync()
        {
            _currentImageState = ImageState.DefaultShown;
            return ShowDefaultImage();
        }

        private Task HttpImageErrorSeenAsync()
        {
            _currentImageState = ImageState.ErrorShown;
            return ShowErrorImage();
        }

        private void NewImageAvailable(T image)
        {
            _currentImageState = ImageState.HttpImageShown;
            FireImageChanged(image);
        }

        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}
#endif