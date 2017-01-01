#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public abstract class AllThreadDispatchingObject : MainThreadDispatchingObject
    {
        private readonly object _lockObject = new object();

        protected void RunSyncWithLock(Action action)
        {
            LockableObjectHelpers.RunSyncWithLock(this._lockObject, action);
        }

        protected void RunAsyncWithLock(Action action)
        {
            LockableObjectHelpers.RunAsyncWithLock(this._lockObject, action);
        }

        protected void RunSyncOrAsyncWithLock(Action action, Action whenComplete = null)
        {
            LockableObjectHelpers.RunSyncOrAsyncWithLock(this._lockObject, action, whenComplete);
        }
    }
}
#endif