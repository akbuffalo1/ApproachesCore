using System;
using System.Collections.Generic;
using System.Linq;

namespace AD
{
	public static class EnumExtensionMethods
	{
		public static string GetName(this Enum self)
		{
			var type = self.GetType();
			if (!type.IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetName(type, self);
		}

		public static List<T> ToList<T>() where T : struct, IConvertible, IFormattable, IComparable
		{
			var type = typeof(T);
			if (!type.IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(type).Cast<T>().ToList();
		}
	}
}

