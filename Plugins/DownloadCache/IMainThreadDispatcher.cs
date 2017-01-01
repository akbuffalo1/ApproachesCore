#if _DL_CACHE_
using System;
using System.Collections.Generic;
using System.Text;

namespace AD
{
    public interface IMainThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}
#endif