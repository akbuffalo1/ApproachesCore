#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AD.Plugins.DownloadCache
{
    public class HttpFileDownloader : LockableObject, IHttpFileDownloader
    {
        private readonly Dictionary<FileDownloadRequest, bool> _currentRequests =
            new Dictionary<FileDownloadRequest, bool>();

        private readonly int _maxConcurrentDownloads = 20;
        private readonly Queue<FileDownloadRequest> _queuedRequests = new Queue<FileDownloadRequest>();

        public HttpFileDownloader(IDownloadCacheConfig config)
        {
            if (config != null)
            {
                _maxConcurrentDownloads = config.MaxConcurrentDownloads;
            }
        }

#region IHttpFileDownloader Members

        public void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error)
        {
            var request = new FileDownloadRequest(url, downloadPath);
            request.DownloadComplete += (sender, args) =>
            {
                OnRequestFinished(request);
                success?.Invoke();
            };
            request.DownloadFailed += (sender, args) =>
            {
                OnRequestFinished(request);
                error?.Invoke(args.Value);
            };

            RunSyncOrAsyncWithLock(() =>
            {
                _queuedRequests.Enqueue(request);
                if (_currentRequests.Count < _maxConcurrentDownloads)
                {
                    StartNextQueuedItem();
                }
            });
        }

#endregion IHttpFileDownloader Members

        private void OnRequestFinished(FileDownloadRequest request)
        {
            RunSyncOrAsyncWithLock(() =>
            {
                _currentRequests.Remove(request);
                if (_queuedRequests.Any())
                {
                    StartNextQueuedItem();
                }
            });
        }

        private void StartNextQueuedItem()
        {
            if (_currentRequests.Count >= _maxConcurrentDownloads)
                return;

            RunSyncOrAsyncWithLock(() =>
            {
                if (_currentRequests.Count >= _maxConcurrentDownloads)
                    return;

                if (!_queuedRequests.Any())
                    return;

                var request = _queuedRequests.Dequeue();
                _currentRequests.Add(request, true);
                request.Start();
            });
        }
    }
}
#endif