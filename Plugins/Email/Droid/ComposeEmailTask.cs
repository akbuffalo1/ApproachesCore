#if _MAIL_ && __ANDROID__
using AD.Plugins.CurrentActivity;
using Android.Content;
using Android.OS;
using Android.Text;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AD.Plugins.Email.Droid
{
    public class ComposeEmailTask : IComposeEmailTaskEx
    {
        public bool CanSendAttachments => true;

        public bool CanSendEmail => true;

        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false, string dialogTitle = null)
        {
            var toArray = to == null ? null : new[] { to };
            var ccArray = cc == null ? null : new[] { cc };
            ComposeEmail(
                toArray,
                ccArray,
                subject,
                body,
                isHtml,
                null,
                dialogTitle);
        }

        public void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null, string body = null, bool isHtml = false, IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
        {
            var emailIntent = new Intent(Intent.ActionSendMultiple);

            if (to != null)
            {
                emailIntent.PutExtra(Intent.ExtraEmail, to.ToArray());
            }
            if (cc != null)
            {
                emailIntent.PutExtra(Intent.ExtraCc, cc.ToArray());
            }
            emailIntent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

            body = body ?? string.Empty;

            if (isHtml)
            {
                emailIntent.SetType("text/html");
                emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(body));
            }
            else
            {
                emailIntent.SetType("text/plain");
                emailIntent.PutExtra(Intent.ExtraText, body);
            }

            var activity = Resolver.Resolve<ICurrentActivity>().Activity;

            if (attachments != null)
            {
                var uris = new List<IParcelable>();

                activity.RunOnUiThread(() =>
                {
                    foreach (var file in attachments)
                    {
                        File localfile;
                        using (var localFileStream = activity.OpenFileOutput(
                            file.FileName, FileCreationMode.WorldReadable))
                        {
                            localfile = activity.GetFileStreamPath(file.FileName);
                            file.Content.CopyTo(localFileStream);
                        }
                        localfile.SetReadable(true, false);
                        uris.Add(Android.Net.Uri.FromFile(localfile));
                        localfile.DeleteOnExit(); // Schedule to delete file when VM quits.
                    }
                });

                if (uris.Any())
                    emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
            }

            // fix for GMail App 5.x (File not found / permission denied when using "StartActivity")
            activity.StartActivityForResult(Intent.CreateChooser(emailIntent, dialogTitle ?? string.Empty), 0);
        }
    }
}
#endif