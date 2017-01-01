using System;

#if __IOS__
using UIKit;
#endif

namespace AD
{
	public enum AlertAlignment
	{
		Center, Left, Right, Justified
	}

	public interface IAlertDialog
	{
		AlertAlignment MessageAlignment { get; set; }
		string Title { get; set; }
		string Message { get; set; }
		Action Continuation { get; set; }
	}

	public static class AlertDialogExtensionMethods
	{
#if __IOS__
		public static UITextAlignment ToUITextAlignment(this AlertAlignment val)
		{
			switch (val)
			{
				case AlertAlignment.Left:
					return UITextAlignment.Left;
				case AlertAlignment.Right:
					return UITextAlignment.Right;
				case AlertAlignment.Center:
					return UITextAlignment.Center;
				case AlertAlignment.Justified:
					return UITextAlignment.Justified;
			}
			return default(UITextAlignment);
		}
#endif
	}
}

