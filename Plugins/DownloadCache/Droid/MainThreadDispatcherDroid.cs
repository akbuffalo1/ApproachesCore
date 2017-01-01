#if _DL_CACHE_ && __ANDROID__
using Android.App;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AD.Plugins.DownloadCache.Droid
{
    public class MainThreadDispatcherDroid : MainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        public MainThreadDispatcherDroid()
        {
            this._uiSynchronizationContext = SynchronizationContext.Current;
        }

        public override bool RequestMainThreadAction(Action action)
        {
            if (this._uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                Application.SynchronizationContext.Post(_ => { action(); }, null);
            return true;
        }
    }
}
#endif