#if _MAIL_
using System;

namespace AD
{
	public interface IComposeEmailTask
	{
		/// <summary>
		/// Compose an E-mail
		/// </summary>
		/// <param name="to">Who the e-mail is to</param>
		/// <param name="cc">Who you want to send a carbon copy to</param>
		/// <param name="subject">Subject of the e-mail</param>
		/// <param name="body">Body of the e-mail</param>
		/// <param name="isHtml">Set to true if the <see cref="body"/> contains HTML content</param>
		/// <param name="dialogTitle">Title of the dialog shown on Android</param>
		void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
			bool isHtml = false, string dialogTitle = null);
	}
}
#endif