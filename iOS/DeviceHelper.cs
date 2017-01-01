#if __IOS__
using System;
using UIKit;

namespace AD.iOS
{
    public static class DeviceHelper
    {
        /* See https://www.paintcodeapp.com/news/ultimate-guide-to-iphone-resolutions */
        public static bool IsIphone5() => UIScreen.MainScreen.Bounds.Height == 568;
        public static bool IsIphone4() => UIScreen.MainScreen.Bounds.Height == 480;
        public static bool IsIphone6() => UIScreen.MainScreen.Bounds.Height == 667;
        public static bool IsIphone6P() => UIScreen.MainScreen.Bounds.Height == 736;

        public static void OnIphone6P(Action act)
        {
            if (IsIphone6P())
            {
                act();
            }
        }

        public static void OnIphone6(Action act)
        {
            if (IsIphone6())
            {
                act();
            }
        }

        public static void OnIphone5(Action act)
        {
            if (IsIphone5())
            {
                act();
            }
        }

        public static void OnIphone4(Action act)
        {
            if (IsIphone4())
            {
                act();
            }
        }
    }
}
#endif