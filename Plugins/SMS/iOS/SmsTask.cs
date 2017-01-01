#if __IOS__ && _SMS_
using System;
using AD.iOS;
using AD.Platform.iOS;
using Foundation;
using MessageUI;
using UIKit;

namespace AD.Plugins.Sms.iOS
{
	public class SmsTask : MFMessageComposeViewControllerDelegate, ISmsTask
	{
		public override void Finished(MFMessageComposeViewController controller, MessageComposeResult result)
		{
			controller.DismissViewController(true, null);
		}

		MFMessageComposeViewController _sms;

		public void SendTo(string phoneNumber, string body)
		{
			_sms = new MFMessageComposeViewController();
			_sms.Recipients = new[] { phoneNumber } ?? new[] { string.Empty };
			_sms.Body = body;
			_sms.MessageComposeDelegate = this;

			UIApplication.SharedApplication.KeyWindow.RootViewController.TopmostViewController()
			.PresentViewController(_sms, true, null);
		}
	}
}
#endif