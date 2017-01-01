#if _DL_CACHE_ && __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace AD.Plugins.DownloadCache.iOS
{
    public class LocalFileImageLoaderiOS : AllThreadDispatchingObject, ILocalFileImageLoader<UIImage>
    {
        private const string ResourcePrefix = "res:";

        public Task<Image<UIImage>> Load(string localPath, bool shouldCache, int width, int height)
        {
            var tcs = new TaskCompletionSource<Image<UIImage>>();

            InvokeOnMainThread(() =>
            {
                UIImage uiImage;

                uiImage = localPath.StartsWith(ResourcePrefix) ? LoadResourceImage(localPath.Substring(ResourcePrefix.Length)) : LoadUiImage(localPath);

                var result = (Image<UIImage>)new ImageiOS(uiImage);

                tcs.TrySetResult(result);
            });

            return tcs.Task;
        }

        private static UIImage LoadUiImage(string localPath)
        {
            var file = Resolver.Resolve<IFileStore>();
            var nativePath = file.NativePath(localPath);
            return UIImage.FromFile(nativePath);
        }

        private static UIImage LoadResourceImage(string resourcePath)
        {
            return UIImage.FromBundle(resourcePath);
        }
    }
}
#endif