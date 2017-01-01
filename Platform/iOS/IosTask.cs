#if __IOS__
using System;
using Foundation;
using UIKit;

namespace AD.Platform.iOS
{
	public class IosTask
	{
		protected bool DoUrlOpen(NSUrl url)
		{
			return UIApplication.SharedApplication.OpenUrl(url);
		}
	}
}
#endif