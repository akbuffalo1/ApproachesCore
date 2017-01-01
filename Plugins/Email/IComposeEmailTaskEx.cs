#if _MAIL_
using System;
using System.Collections.Generic;
using AD.Plugins.Email;

namespace AD
{
	public interface IComposeEmailTaskEx
	{
		/// <summary>
		/// Compose an E-mail
		/// </summary>
		/// <param name="to"><see cref="IEnumerable{T}"/> of <see cref="string"/> containing the e-mail addresses
		///     you want to send the e-mail to</param>
		/// <param name="cc"><see cref="IEnumerable{T}"/> of <see cref="string"/> containing the e-mail addresses
		///     you want to send a carbon copy to</param>
		/// <param name="subject">Subject of the e-mail</param>
		/// <param name="body">Body of the e-mail</param>
		/// <param name="isHtml">Set to true if the <see cref="body"/> contains HTML content</param>
		/// <param name="attachments"><see cref="IEnumerable{T}"/> of <see cref="EmailAttachment"/> containing
		///     attachments</param>
		/// <param name="dialogTitle">Title of the dialog shown on Android</param>
		void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null,
			string body = null, bool isHtml = false, IEnumerable<EmailAttachment> attachments = null,
			string dialogTitle = null);

		bool CanSendEmail { get; }
		bool CanSendAttachments { get; }
	}
}
#endif