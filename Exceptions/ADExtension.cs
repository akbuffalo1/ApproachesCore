using System;

namespace AD.Exceptions
{
	// Officially exception should support serialisation, but we don't add it here - mainly because of 
	// serialization limits in PCLs 
	public class ADException : Exception
	{
		public ADException()
		{
		}

		public ADException(string message)
			: base(message)
		{
		}

		public ADException(string messageFormat, params object[] messageFormatArguments)
			: base(string.Format(messageFormat, messageFormatArguments))
		{
		}

		// the order of parameters here is slightly different to that normally expected in an exception
		// - but this order allows us to put string.Format in place 
		public ADException(Exception innerException, string messageFormat, params object[] formatArguments)
			: base(string.Format(messageFormat, formatArguments), innerException)
		{
		}
	}

	public static class ADExceptionExtensionMethods
	{
		public static string ToLongString(this Exception exception)
		{
			if (exception == null)
				return "null exception";

			if (exception.InnerException != null)
			{
				var innerExceptionText = exception.InnerException.ToLongString();
				return string.Format("{0}: {1}\n\t{2}\nInnerException was {3}",
					exception.GetType().Name,
					exception.Message ?? "-",
					exception.StackTrace,
					innerExceptionText);
			}
			else
			{
				return string.Format("{0}: {1}\n\t{2}",
					exception.GetType().Name,
					exception.Message ?? "-",
					exception.StackTrace);
			}
		}

		public static Exception ADWrap(this Exception exception)
		{
			if (exception is ADException)
				return exception;

			return ADWrap(exception, exception.Message);
		}

		public static Exception ADWrap(this Exception exception, string message)
		{
			return new ADException(exception, message);
		}

		public static Exception ADWrap(this Exception exception, string messageFormat, params object[] formatArguments)
		{
			return new ADException(exception, messageFormat, formatArguments);
		}
	}

}