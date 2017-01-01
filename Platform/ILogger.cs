using System;

namespace AD
{
	public interface ILogger
	{
		void Debug(string tag, string message, params object[] args);
		void Error(string tag, string message, params object[] args);
		void Info(string tag, string message, params object[] args);
		void Verbose(string tag, string message, params object[] args);
		void Warn(string tag, string message, params object[] args);
		void Exception(string tag, object exception, string message, params object[] args); // java.Lang.Throwable (Droid)
	}
}

