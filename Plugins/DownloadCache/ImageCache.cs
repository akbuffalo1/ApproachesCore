#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AD.Plugins.DownloadCache
{
    public class ImageCache<T> : AllThreadDispatchingObject, IImageCache<T>
    {
        private ImmutableDictionary<string, Entry> _entriesByHttpUrl = ImmutableDictionary.Create<string, Entry>();

        private readonly IFileDownloadCache _fileDownloadCache;
        private readonly IDownloadCacheConfig _config;

        public ImageCache(IFileDownloadCache fileDownloadCache, IDownloadCacheConfig config)
        {
            _fileDownloadCache = fileDownloadCache;
            _config = config;
        }

#region IImageCache<T> Members

        public Task<T> RequestImage(string url)
        {
            var tcs = new TaskCompletionSource<T>();

            Task.Run(() =>
            {
                Entry entry;
                if (_entriesByHttpUrl.TryGetValue(url, out entry))
                {
                    entry.WhenLastAccessedUtc = DateTime.UtcNow;
                    tcs.TrySetResult(entry.Image.RawImage);
                    return;
                }

                try
                {
                    _fileDownloadCache.RequestLocalFilePath(url,
                        async s =>
                        {
                            var image = await Parse(s).ConfigureAwait(false);
                            _entriesByHttpUrl = _entriesByHttpUrl.SetItem(url, new Entry(url, image));
                            tcs.TrySetResult(image.RawImage);
                        },
                        exception =>
                        {
                            tcs.TrySetException(exception);
                        });
                }
                finally
                {
                    ReduceSizeIfNecessary();
                }
            });

            return tcs.Task;
        }

#endregion IImageCache<T> Members

        private void ReduceSizeIfNecessary()
        {
            RunSyncOrAsyncWithLock(() =>
            {
                var entries = _entriesByHttpUrl.Select(kvp => kvp.Value).ToList();

                var currentSizeInBytes = entries.Sum(x => x.Image.GetSizeInBytes());
                var currentCountFiles = entries.Count;

                if (currentCountFiles <= _config.MaxInMemoryFiles
                    && currentSizeInBytes <= _config.MaxInMemoryBytes)
                    return;

                // we don't use LINQ OrderBy here because of AOT/JIT problems on MonoTouch
                entries.Sort(new MvxImageComparer());

                var entriesToRemove = new List<Entry>();

                while (currentCountFiles > _config.MaxInMemoryFiles
                        || currentSizeInBytes > _config.MaxInMemoryBytes)
                {
                    var toRemove = entries[0];
                    entries.RemoveAt(0);

                    entriesToRemove.Add(toRemove);

                    currentSizeInBytes -= toRemove.Image.GetSizeInBytes();
                    currentCountFiles--;

                    _entriesByHttpUrl = _entriesByHttpUrl.Remove(toRemove.Url);
                }

                if (_config.DisposeOnRemoveFromCache && entriesToRemove.Count > 0)
                {
                    // It is important to Dispose images on UI-thread
                    // otherwise there could be a crash when the image could be disposed
                    // just about to be rendered
                    // see https://github.com/MvvmCross/MvvmCross-Plugins/issues/41#issuecomment-199833494
                    DisposeImagesOnMainThread(entriesToRemove);
                }
            });
        }

        private void DisposeImagesOnMainThread(List<Entry> entries)
        {
            InvokeOnMainThread(() => {
                foreach (var entry in entries)
                {
                    entry.Image.RawImage.DisposeIfDisposable();
                }
            });
        }

        private class MvxImageComparer : IComparer<Entry>
        {
            public int Compare(Entry x, Entry y)
            {
                return x.WhenLastAccessedUtc.CompareTo(y.WhenLastAccessedUtc);
            }
        }

        protected Task<Image<T>> Parse(string path)
        {
            var loader = Resolver.Resolve<ILocalFileImageLoader<T>>();
            return loader.Load(path, false, 0, 0);
        }

#region Nested type: Entry

        private class Entry
        {
            public Entry(string url, Image<T> image)
            {
                Url = url;
                Image = image;
                WhenLastAccessedUtc = DateTime.UtcNow;
            }

            public string Url { get; private set; }
            public Image<T> Image { get; private set; }
            public DateTime WhenLastAccessedUtc { get; set; }
        }

#endregion Nested type: Entry
    }
}
#endif