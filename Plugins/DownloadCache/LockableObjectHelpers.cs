#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AD.Plugins.DownloadCache
{
    public static class LockableObjectHelpers
    {
        public static void RunSyncWithLock(object lockObject, Action action)
        {
            lock (lockObject)
            {
                action();
            }
        }

        public static void RunAsyncWithLock(object lockObject, Action action)
        {
            AsyncDispatcher.BeginAsync(() =>
            {
                lock (lockObject)
                {
                    action();
                }
            });
        }

        public static void RunSyncOrAsyncWithLock(object lockObject, Action action, Action whenComplete = null)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    action();
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                whenComplete?.Invoke();
            }
            else
            {
                AsyncDispatcher.BeginAsync(() =>
                {
                    lock (lockObject)
                    {
                        action();
                    }

                    whenComplete?.Invoke();
                });
            }
        }
    }
}
#endif