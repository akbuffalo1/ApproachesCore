#if __ANDROID__
using System;
using Android.Widget;
using Android.App;

namespace AD.Droid
{
    public class DialogProvider : IDialogProvider
    {
        public void DisplayAlertDialog(IAlertDialog dlg)
        {
            var currentActivity = AD.Resolver.Resolve<AD.Plugins.CurrentActivity.ICurrentActivity>();
            currentActivity.Activity.RunOnUiThread(() =>
            {
                var builder = new AlertDialog.Builder(currentActivity.Activity);

                builder.SetMessage(dlg.Message)
                       .SetTitle(dlg.Title);

                builder.SetPositiveButton(Android.Resource.String.Ok, (sender, e) =>
                {
                    dlg.Continuation?.Invoke();
                    (sender as AlertDialog).Dismiss();
                });

                var dialog = builder.Show();

                var msg = dialog.FindViewById<TextView>(Android.Resource.Id.Message);

                switch (dlg.MessageAlignment)
                {
                    case AlertAlignment.Center:
                        msg.Gravity = Android.Views.GravityFlags.Center;
                        break;
                    case AlertAlignment.Left:
                        msg.Gravity = Android.Views.GravityFlags.Left;
                        break;
                    case AlertAlignment.Right:
                        msg.Gravity = Android.Views.GravityFlags.Right;
                        break;
                    case AlertAlignment.Justified:
                        msg.Gravity = Android.Views.GravityFlags.Start;
                        break;
                }
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