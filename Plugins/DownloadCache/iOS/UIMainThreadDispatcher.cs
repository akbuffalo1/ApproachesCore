#if _DL_CACHE_ && __IOS__
using System;
using System.Threading;

// TODO: Extract MainThreadDispatcher and ValueEventArgs
using UIKit;

namespace AD.Plugins.DownloadCache.iOS
{
	public class UIMainThreadDispatcher : MainThreadDispatcher
	{
		private readonly SynchronizationContext _uiSynchronizationContext;

		public UIMainThreadDispatcher ()
		{
			this._uiSynchronizationContext = SynchronizationContext.Current;
		}
		
		public override bool RequestMainThreadAction (Action action)
		{
			if (this._uiSynchronizationContext == SynchronizationContext.Current)
				action ();
			else
				UIApplication.SharedApplication.BeginInvokeOnMainThread (action);
			return true;
		}
	}
}
#endif