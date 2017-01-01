#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AD.Plugins.DownloadCache
{
    public static class AsyncDispatcher
    {
        public static void BeginAsync(Action action)
        {
            Task.Run(action);
        }

        public static void BeginAsync(Action<object> action, object state)
        {
            Task.Run(() => action.Invoke(state));
        }
    }
}
#endif