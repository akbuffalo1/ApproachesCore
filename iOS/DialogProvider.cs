#if __IOS__
using System;
using UIKit;
using System.Linq;
using Foundation;

namespace AD.iOS
{
    public class DialogProvider : IDialogProvider
    {
        public void DisplayAlertDialog(IAlertDialog dlg)
        {
            UIKit.UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var paragraphStyle = new NSMutableParagraphStyle { Alignment = dlg.MessageAlignment.ToUITextAlignment() };
                var attributedMessage = new NSMutableAttributedString(dlg.Message, paragraphStyle: paragraphStyle, font: UIFont.PreferredBody);

                var alert = UIAlertController.Create(dlg.Title, string.Empty, UIAlertControllerStyle.Alert);
                alert.SetValueForKeyPath(attributedMessage, new NSString("attributedMessage"));

                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, action =>
                {
                    dlg.Continuation?.Invoke();
                }));

                UIApplication.SharedApplication.KeyWindow.RootViewController
            .TopmostViewController().PresentViewController(alert, true, null);
            });
        }

        public void DisplayError(string message)
        {
            DisplayAlertDialog(new ErrorAlertDialog
            {
                Message = message
            });
        }

        public void DisplayConfirmation(string title, string message, Action continuation)
        {
            DisplayAlertDialog(new BaseAlertDialog
            {
                Title = title,
                Message = message,
                Continuation = continuation
            });
        }
    }
}
#endif