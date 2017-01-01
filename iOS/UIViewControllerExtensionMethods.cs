#if __IOS__

using System;
using UIKit;

namespace AD.iOS
{
    public static class UIViewControllerExtensionMethods
    {
        public static UIViewController TopmostViewController(this UIViewController vc)
        {
            // TODO: More of these are needed, uitableviewcontroller for example is left out
            if (vc is UINavigationController)
            {
                vc = ((UINavigationController)vc).VisibleViewController;
            }

            if (vc.PresentedViewController != null)
            {
                return vc.PresentedViewController.TopmostViewController();
            }
            else
            {
                foreach (var view in vc.View.Subviews)
                {
                    if (view.NextResponder is UIViewController)
                    {
                        return (view.NextResponder as UIViewController).TopmostViewController();
                    }
                }
                return vc;
            }
        }
    }
}

#endif