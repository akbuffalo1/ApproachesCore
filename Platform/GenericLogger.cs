using System;

namespace AD
{
	using D = System.Diagnostics.Debug;
	public class GenericLogger : ILogger
	{
		#region ILogger implementation

		public void Debug (string tag, string message, params object[] args)
		{
			D.WriteLine(string.Format("@@ [DEBUG] {0} - {1}", tag, string.Format(message, args)));
		}

		public void Error (string tag, string message, params object[] args)
		{
			D.WriteLine(string.Format("@@ [ERROR] {0} - {1}", tag, string.Format(message, args)));
		}

		public void Info (string tag, string message, params object[] args)
		{
			D.WriteLine(string.Format("@@ [INFO] {0} - {1}", tag, string.Format(message, args)));
		}

		public void Verbose (string tag, string message, params object[] args)
		{
			D.WriteLine(string.Format("@@ [VERBOSE] {0} - {1}", tag, string.Format(message, args)));
		}

		public void Warn (string tag, string message, params object[] args)
		{
			D.WriteLine(string.Format("@@ [WARN] {0} - {1}", tag, string.Format(message, args)));
		}

		public void Exception (string tag, object exception, string message, params object[] args)
		{
			D.WriteLine(string.Format("@@ [EXCEPTION] {0} - {1}", tag, exception.ToString()));
		}

		#endregion

		public GenericLogger ()
		{
		}
	}
}

