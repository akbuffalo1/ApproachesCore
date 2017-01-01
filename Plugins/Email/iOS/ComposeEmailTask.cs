#if _MAIL_ && __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using AD.Exceptions;
using AD.iOS;
using Foundation;
using MessageUI;
using UIKit;

namespace AD.Plugins.Email.iOS
{
	public class ComposeEmailTask : IComposeEmailTaskEx
	{
		private MFMailComposeViewController _mail;

		public void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
			bool isHtml = false, string dialogTitle = null)
		{
			var toArray = to == null ? null : new[] { to };
			var ccArray = cc == null ? null : new[] { cc };
			ComposeEmail(
				toArray,
				ccArray,
				subject,
				body,
				isHtml);
		}

		public void ComposeEmail(
			IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null,
			string body = null, bool isHtml = false,
			IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
		{
			if (!MFMailComposeViewController.CanSendMail)
				throw new ADException("This device cannot send mail");

			_mail = new MFMailComposeViewController();
			_mail.SetMessageBody(body ?? string.Empty, isHtml);
			_mail.SetSubject(subject ?? string.Empty);

			if (cc != null)
				_mail.SetCcRecipients(cc.ToArray());

			_mail.SetToRecipients(to?.ToArray() ?? new[] { string.Empty });
			if (attachments != null)
			{
				foreach (var a in attachments)
				{
					_mail.AddAttachmentData(NSData.FromStream(a.Content), a.ContentType, a.FileName);
				}
			}
			_mail.Finished += HandleMailFinished;

			UIApplication.SharedApplication.KeyWindow.RootViewController.TopmostViewController().PresentModalViewController(_mail, true);
		}

		public bool CanSendEmail => MFMailComposeViewController.CanSendMail;

		public bool CanSendAttachments => CanSendEmail;

		private void HandleMailFinished(object sender, MFComposeResultEventArgs e)
		{
			var uiViewController = sender as UIViewController;
			if (uiViewController == null)
			{
				throw new ArgumentException("sender");
			}

			uiViewController.DismissViewController(true, () => { });
		}
	}
}
#endif