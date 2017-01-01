#if _DL_CACHE_
using AD.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AD.Plugins.DownloadCache
{
    public class FileDownloadCache : LockableObject, IFileDownloadCache
    {
        private const string TAG = "AD.Plugins.DownloadCache.FileDownloadCache";
        private const string CacheIndexFileName = "_CacheIndex.txt";
        private static readonly TimeSpan PeriodSaveInterval = TimeSpan.FromSeconds(1.0);

        private ITextSerializer _textConvert;
        private bool _textConvertTried;

        private ILogger _logger;

        protected ITextSerializer TextConvert
        {
            get
            {
                if (_textConvert != null)
                    return _textConvert;

                if (_textConvertTried)
                    return null;
                
                _textConvertTried = true;
                _textConvert = Resolver.Resolve<ITextSerializer>();
                if (_textConvert == null)
                    _logger.Warn(TAG, "Persistent download cache will not be available - no text serializer available");

                return _textConvert;
            }
        }

        public class Entry
        {
            public string HttpSource { get; set; }
            public string DownloadedPath { get; set; }
            public DateTime WhenLastAccessedUtc { get; set; }
            public DateTime WhenDownloadedUtc { get; set; }
        }

        private readonly IDownloadCacheConfig _config;

        private readonly Dictionary<string, Entry> _entriesByHttpUrl;

        private readonly Dictionary<string, List<CallbackPair>> _currentlyRequested =
            new Dictionary<string, List<CallbackPair>>();

        private class CallbackPair
        {
            public CallbackPair(Action<string> success, Action<Exception> error)
            {
                Error = error;
                Success = success;
            }

            public Action<string> Success { get; private set; }
            public Action<Exception> Error { get; private set; }
        }

        private readonly List<string> _toDeleteFiles = new List<string>();

        private readonly Timer _periodicTaskTimer;
        private bool _indexNeedsSaving;

        private string IndexFilePath
        {
            get
            {
                var fileService = FileStoreHelper.SafeGetFileStore();
                return fileService.PathCombine(_config.CacheFolderPath, _config.CacheName + CacheIndexFileName);
            }
        }

        public FileDownloadCache(IDownloadCacheConfig config)
        {
            _config = config;

            EnsureCacheFolderExists();
            _entriesByHttpUrl = LoadIndexEntries();

            QueueUnindexedFilesForDelete();
            QueueOutOfDateFilesForDelete();

            _indexNeedsSaving = false;
            _periodicTaskTimer = new Timer((ignored) => DoPeriodicTasks(), null, PeriodSaveInterval, PeriodSaveInterval);

            _logger = Resolver.Resolve<ILogger>();
        }

#region Constructor helper methods

        private void QueueOutOfDateFilesForDelete()
        {
            var now = DateTime.UtcNow;
            var toRemove = _entriesByHttpUrl.Values.Where(x => (now - x.WhenDownloadedUtc) > _config.MaxFileAge).ToList();
            foreach (var entry in toRemove)
            {
                _entriesByHttpUrl.Remove(entry.HttpSource);
            }
            _toDeleteFiles.AddRange(toRemove.Select(x => x.DownloadedPath));
        }

        private void QueueUnindexedFilesForDelete()
        {
            var store = FileStoreHelper.SafeGetFileStore();
            var files = store.GetFilesIn(_config.CacheFolderPath);

            // we don't use Linq because of AOT/JIT problem on MonoTouch :/
            //var cachedFiles = _entriesByHttpUrl.ToDictionary(x => x.Value.DownloadedPath);
            var cachedFiles = new Dictionary<string, Entry>();
            foreach (var e in _entriesByHttpUrl)
            {
                cachedFiles[store.NativePath(e.Value.DownloadedPath)] = e.Value;
            }

            var toDelete = new List<string>();
            
			/* 
				Earlier we had entries in cachedfiles:
					../Library/Caches/AD.Pictures/bbd0974bdb774f59b340ec4638db21a0
				and entries in files:
					/Users/salemander/Library/Developer/CoreSimulator/Devices/3A473D56-97EA-4DAD-81FD-781D8EE65932/data/Containers/Data/Application/0DFAD8E9-B9E3-4C83-ABF5-CBC6A549FCA6/Documents/../Library/Caches/AD.Pictures/1c4bfa4a0ae540cab06a9f18bb2937e8"	string
			*/
			
			foreach (var file in files) {
				if (!cachedFiles.ContainsKey (file)) {
					if (!file.EndsWith (CacheIndexFileName)) {
						toDelete.Add (file);
					}
				}
            }

            RunSyncOrAsyncWithLock(() =>
            {
                _toDeleteFiles.AddRange(toDelete);
            });
        }

        private void EnsureCacheFolderExists()
        {
            var store = FileStoreHelper.SafeGetFileStore();
            store.EnsureFolderExists(_config.CacheFolderPath);
        }

        private Dictionary<string, Entry> LoadIndexEntries()
        {
            try
            {
                var store = FileStoreHelper.SafeGetFileStore();
                string text;
                if (store.TryReadTextFile(IndexFilePath, out text))
                {
                    var textConvert = TextConvert;
                    if (textConvert == null)
                        return new Dictionary<string, Entry>();

                    var list = textConvert.DeserializeObject<List<Entry>>(text);
                    return list.ToDictionary(x => x.HttpSource, x => x);
                }
            }
            //catch (ThreadAbortException)
            //{
            //    throw;
            //}
            catch (Exception exception)
            {
                _logger.Warn(TAG, "Failed to read cache index {0} - reason {1}", _config.CacheFolderPath,
                               exception.ToLongString());
            }

            return new Dictionary<string, Entry>();
        }

#endregion Constructor helper methods

#region Periodic Tasks

        private void DoPeriodicTasks()
        {
            SaveIndexIfDirty();
            DeleteOldestOneIfTooManyFiles();
            DeleteNextUnneededFile();
        }

        private void DeleteOldestOneIfTooManyFiles()
        {
            RunSyncWithLock(() =>
            {
                if (_entriesByHttpUrl.Count <= _config.MaxFiles)
                    return;

                var nextToDelete = _entriesByHttpUrl.Values.First();
                foreach (var entry in _entriesByHttpUrl.Values)
                {
                    if (entry.WhenLastAccessedUtc < nextToDelete.WhenLastAccessedUtc)
                        nextToDelete = entry;
                }
                _entriesByHttpUrl.Remove(nextToDelete.HttpSource);
                _toDeleteFiles.Add(nextToDelete.DownloadedPath);
            });
        }

        private void DeleteNextUnneededFile()
        {
            string nextFileToDelete = "";
            RunSyncWithLock(() =>
            {
                nextFileToDelete = _toDeleteFiles.FirstOrDefault();
            });

            if (string.IsNullOrEmpty(nextFileToDelete))
                return;

            try
            {
                var fileService = FileStoreHelper.SafeGetFileStore();
                if (fileService.Exists(nextFileToDelete))
                    fileService.DeleteFile(nextFileToDelete);
            }
            catch (Exception exception)
            {
                _logger.Warn("Problem seen deleting file {0} problem {1}", nextFileToDelete,
                               exception.ToLongString());
            }
        }

        private void SaveIndexIfDirty()
        {
            if (!_indexNeedsSaving)
                return;

            RunSyncWithLock(() =>
            {
                List<Entry> toSave = _entriesByHttpUrl.Values.ToList();
                _indexNeedsSaving = false;

                try
                {
                    var textConvert = TextConvert;
                    if (textConvert == null)
                        return;
                    var text = TextConvert.SerializeObject(toSave);

                    var store = FileStoreHelper.SafeGetFileStore();
                    store.WriteFile(IndexFilePath, text);
                }
                catch (Exception exception)
                {
                    _logger.Warn("Failed to save cache index {0} - reason {1}", _config.CacheFolderPath,
                                   exception.ToLongString());
                }
            });
        }

#endregion Periodic Tasks

        public void RequestLocalFilePathWithExtension(string httpSource, Action<string> success, Action<Exception> error)
        {
            Task.Run(() => DoRequestLocalFilePath(httpSource, true, success, error)).ConfigureAwait(false);
        }
        
        public void RequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error)
        {
            Task.Run(() => DoRequestLocalFilePath(httpSource, false, success, error)).ConfigureAwait(false);
        }

        private void DoRequestLocalFilePath(string httpSource, bool useExt, Action<string> success, Action<Exception> error)
        {
            RunSyncOrAsyncWithLock(() =>
            {
                Entry diskEntry;
                if (_entriesByHttpUrl.TryGetValue(httpSource, out diskEntry))
                {
                    var service = FileStoreHelper.SafeGetFileStore();
                    if (!service.Exists(diskEntry.DownloadedPath))
                    {
                        _entriesByHttpUrl.Remove(httpSource);
                    }
                    else
                    {
                        diskEntry.WhenLastAccessedUtc = DateTime.UtcNow;
                        DoFilePathCallback(diskEntry, success, error);
                        return;
                    }
                }

                List<CallbackPair> currentlyRequested;
                if (_currentlyRequested.TryGetValue(httpSource, out currentlyRequested))
                {
                    currentlyRequested.Add(new CallbackPair(success, error));
                    return;
                }

                currentlyRequested = new List<CallbackPair>
                        {
                            new CallbackPair(success, error)
                        };
                _currentlyRequested.Add(httpSource, currentlyRequested);
                var downloader = Resolver.Resolve<IHttpFileDownloader>();
                var fileService = FileStoreHelper.SafeGetFileStore();
				var ext = useExt ? System.IO.Path.GetExtension (httpSource) : String.Empty;
                var pathForDownload = fileService.PathCombine(_config.CacheFolderPath, $"{Guid.NewGuid().ToString("N")}{ext}");
                downloader.RequestDownload(httpSource, pathForDownload,
                                           () => OnDownloadSuccess(httpSource, pathForDownload),
                                           exception => OnDownloadError(httpSource, exception));
            });
        }

        public void ClearAll()
        {
            RunSyncWithLock(() =>
            {
                var service = FileStoreHelper.SafeGetFileStore();
                foreach (var entries in _entriesByHttpUrl)
                {
                    service.DeleteFile(entries.Value.DownloadedPath);
                }
                _entriesByHttpUrl.Clear();
                _indexNeedsSaving = true;
            });
        }

        public void Clear(string httpSource)
        {
            RunSyncWithLock(() =>
            {
                Entry diskEntry;
                if (_entriesByHttpUrl.TryGetValue(httpSource, out diskEntry))
                {
                    _toDeleteFiles.Add(diskEntry.DownloadedPath);
                    _entriesByHttpUrl.Remove(httpSource);
                    _indexNeedsSaving = true;
                }
            });
        }

        private void OnDownloadSuccess(string httpSource, string pathForDownload)
        {
            RunSyncOrAsyncWithLock(() =>
            {
                var diskEntry = new Entry
                {
                    DownloadedPath = pathForDownload,
                    HttpSource = httpSource,
                    WhenDownloadedUtc = DateTime.UtcNow,
                    WhenLastAccessedUtc = DateTime.UtcNow
                };
                _entriesByHttpUrl[httpSource] = diskEntry;
                _indexNeedsSaving = true;

                var toCallback = _currentlyRequested[httpSource];
                _currentlyRequested.Remove(httpSource);

                foreach (var callbackPair in toCallback)
                {
                    DoFilePathCallback(diskEntry, callbackPair.Success, callbackPair.Error);
                }
            });
        }

        private void OnDownloadError(string httpSource, Exception exception)
        {
            List<CallbackPair> toCallback = null;
            RunSyncOrAsyncWithLock(() =>
            {
                toCallback = _currentlyRequested[httpSource];
                _currentlyRequested.Remove(httpSource);
            },
            () =>
            {
                foreach (var callbackPair in toCallback)
                {
                    callbackPair.Error(exception);
                }
            });
        }

        private void DoFilePathCallback(Entry diskEntry, Action<string> success, Action<Exception> error)
        {
            success(diskEntry.DownloadedPath);
        }

        public delegate void TimerCallback(object state);

        public sealed class Timer : CancellationTokenSource, IDisposable
        {
            public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
            {
                Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
                {
                    var tuple = (Tuple<TimerCallback, object>)s;

                    while (true)
                    {
                        if (IsCancellationRequested)
                            break;
                        Task.Run(() => tuple.Item1(tuple.Item2));
                        await Task.Delay(period);
                    }
                }, Tuple.Create(callback, state), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
            }

            public new void Dispose()
            {
                base.Cancel();
            }
        }
    }
}
#endif