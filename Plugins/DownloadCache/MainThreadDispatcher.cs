#if _DL_CACHE_
using System;
using System.Reflection;
using AD.Exceptions;

namespace AD.Plugins.DownloadCache
{
	public abstract class MainThreadDispatcher : IMainThreadDispatcher
	{
		public static readonly string TAG = "AD.Plugins.DownloadCache.iOS.UIMainThreadDispatcher";
		public virtual bool RequestMainThreadAction (Action action) => false;
		
    	protected static void ExceptionMaskedAction(Action action)
        {
            var logger = Resolver.Resolve<ILogger>();

            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                logger?.Error(TAG, "TargetInvocateException masked " + exception.InnerException.ToLongString());
            }
            catch (Exception exception)
            {
                // note - all exceptions masked!
                logger?.Error(TAG, "Exception masked " + exception.ToLongString());
            }
        }
	}
}
#endif