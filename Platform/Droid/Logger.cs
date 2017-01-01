#if __ANDROID__
using System;
using Android.Util;
using Java.Lang;
using AD.Exceptions;

namespace AD.Droid
{
	public class Logger : ILogger
	{
		#region ILogger implementation

		public virtual void Debug (string tag, string message, params object[] args)
		{
			Log.Debug(tag, string.Format (message, args));
		}

		public virtual void Error (string tag, string message, params object[] args)
		{
			Log.Error(tag, string.Format (message, args));
		}

		public virtual void Info (string tag, string message, params object[] args)
		{
			Log.Info(tag, string.Format (message, args));
		}

		public virtual void Verbose (string tag, string message, params object[] args)
		{
			Log.Verbose(tag, string.Format (message, args));
		}

		public virtual void Warn (string tag, string message, params object[] args)
		{
			Log.Warn(tag, string.Format (message, args));
		}

		public virtual void Exception (string tag, object exception, string message, params object[] args)
		{
			var ex = new Throwable(((System.Exception)exception).ToLongString());
			Log.Error (tag, string.Format(message, args), ex);
		}


		#endregion

		public Logger ()
		{
		}
	}
}
#endif

