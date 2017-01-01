using System;

namespace AD
{
	public static class CastExtensions
	{
		public static void WithType<T>(this object obj, Action<T> doWith)
			where T : class
		{
			if (obj == null || doWith == null)
			{
				return;
			}

			var converted = obj as T;

			if (converted == null)
			{
				return;
			}

			doWith(converted);
		}

		public static TOut WithType<T, TOut>(this object withObject, Func<T, TOut> action)
			where T : class
		{
			if (action == null)
			{
				return default(TOut);
			}

			var targetTypeObject = withObject as T;

			return targetTypeObject == null ? default(TOut) : action(targetTypeObject);
		}

		public static void With<T>(this T withObject, Action<T> action)
			where T : class
		{
			if (withObject == null || action == null)
			{
				return;
			}

			action(withObject);
		}
	}
}

