#if __IOS__
namespace AD.iOS
{
	using System;
	using UIKit;
	using ObjCRuntime;
	
	public static class UIViewExtensionMethods
	{
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		public static T FindSuperviewOfType<T>(this UIView view, UIView stopAt) where T : UIView
		{
			if (view.Superview != null)
			{
				if (view.Superview is T)
				{
					return view.Superview as T;
				}

				if (view.Superview != stopAt)
				{
					return view.Superview.FindSuperviewOfType<T>(stopAt);
				}
			}

			return null;
		}

		public static UIImage Screenshot(this UIView target)
		{
			UIGraphics.BeginImageContextWithOptions(target.Bounds.Size, false, UIScreen.MainScreen.Scale);
			if (target.RespondsToSelector(new Selector("drawViewHierarchyInRect:afterScreenUpdates:")))
			{
				target.DrawViewHierarchy(target.Bounds, true);
			}
			else
			{
				target.Layer.RenderInContext(UIGraphics.GetCurrentContext());
			}
			UIImage currentImageContext = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return currentImageContext;
		}
	}
}
#endif