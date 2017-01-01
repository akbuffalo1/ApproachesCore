#if __ANDROID__ && _SMS_
using AD.Plugins.CurrentActivity;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.Sms.Droid
{
    public class SmsTaskDroid : ISmsTask
    {
        public void SendTo(string phoneNumber, string body)
        {
            var smsUri = Android.Net.Uri.Parse("sms:" + phoneNumber);
            var smsIntent = new Intent(Intent.ActionView, smsUri);
            smsIntent.PutExtra("sms_body", body);
            
            Resolver.Resolve<ICurrentActivity>().Activity.StartActivity(Intent.CreateChooser(smsIntent, "Send SMS with"));
        }
    }
}
#endif