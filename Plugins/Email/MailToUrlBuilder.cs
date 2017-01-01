#if _MAIL_
using System;
using System.Text;

namespace AD.Plugins.Email
{
	public class MailToUrlBuilder
	{
		public string Build(string to, string cc, string subject, string body)
		{
			var builder = new StringBuilder();
			builder.Append("mailto:" + to);

			var sep = "?";
			AddParam(builder, "cc", cc, ref sep);
			AddParam(builder, "subject", subject, ref sep);
			AddParam(builder, "body", body, ref sep);

			var url = builder.ToString();
			return url;
		}

		private static void AddParam(StringBuilder builder, string param, string value, ref string separator)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}

			builder.Append(separator);
			separator = "&";
			builder.Append(param);
			builder.Append("=");
			builder.Append(Uri.EscapeDataString(value));
		}
	}
}
#endif