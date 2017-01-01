#if _PHONE_CALL_ && __ANDROID__
using AD.Plugins.CurrentActivity;
using Android.Content;
using Android.Telephony;
using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Plugins.PhoneCall.Droid
{
    public class PhoneCallTaskDroid : IPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {
            var phoneNumber = PhoneNumberUtils.FormatNumber(number);
            var newIntent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + phoneNumber));
            Resolver.Resolve<ICurrentActivity>().Activity.StartActivity(newIntent);
        }
    }
}
#endif