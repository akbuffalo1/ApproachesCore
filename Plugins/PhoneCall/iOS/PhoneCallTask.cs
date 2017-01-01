#if _PHONE_CALL_ && __IOS__
using System;
using UIKit;
using Foundation;

namespace AD.Plugins.PhoneCall.iOS
{
	public class PhoneCallTask : AD.Platform.iOS.IosTask, IPhoneCallTask
	{
		#region IPhoneCallTask implementation

		public void MakePhoneCall(string name, string number)
		{
 			var url = new NSUrl("tel:" + number);
			DoUrlOpen(url);
		}

		#endregion IMvxPhoneCallTask Members
	}
}
#endif