#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.DownloadCache
{
    public abstract class MainThreadDispatchingObject
    {
        protected IMainThreadDispatcher Dispatcher => AD.Resolver.Resolve<IMainThreadDispatcher>();

        protected void InvokeOnMainThread(Action action)
        {
            this.Dispatcher?.RequestMainThreadAction(action);
        }
    }
}
#endif