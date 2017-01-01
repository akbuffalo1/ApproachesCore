#if __IOS__
namespace AD.iOS
{
	using System;
	using UIKit;
	using CoreGraphics;
	using Accelerate;
	
	public static class UIImageExtensionMethods
	{
        public static UIImage GetImageFromColor(this UIColor color)
        {
            CGRect rect = new CGRect(0, 0, 1, 1);
            UIGraphics.BeginImageContext(rect.Size);
            CGContext context = UIGraphics.GetCurrentContext();
            context.SetFillColor(color.CGColor);
            context.FillRect(rect);
            UIImage img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return img;
        }
        
        public static UIImage ApplyBlurWithRadius(this UIImage target, nfloat blurRadius, UIColor tintColor, nfloat saturationDeltaFactor, UIImage maskImage)
        {
            if (target.Size.Width < 1 || target.Size.Height < 1)
                throw new Exception(string.Format("*** error: invalid size: (%.2 x %.2f). Both dimensions must be >= 1: %@", (object)target.Size.Width, (object)target.Size.Height, (object)target));
            if (target.CGImage == null)
                throw new Exception(string.Format("*** error: image must be backed by a CGImage: %@", (object)target));
            if (maskImage != null && maskImage.CGImage == null)
                throw new Exception(string.Format("*** error: maskImage must be backed by a CGImage: %@", (object)maskImage));
            CGRect rect = new CGRect(CGPoint.Empty, target.Size);
            UIImage uiImage = target;
            bool flag1 = blurRadius > 1.401298E-45f;
            bool flag2 = Math.Abs((double)saturationDeltaFactor - 1.0) > 1.40129846432482E-45;
            if (flag1 || flag2)
            {
                UIGraphics.BeginImageContextWithOptions(target.Size, false, UIScreen.MainScreen.Scale);
                CGContext currentContext = UIGraphics.GetCurrentContext();
                currentContext.ScaleCTM(1f, -1f);
                currentContext.TranslateCTM(0, -target.Size.Height);
                currentContext.DrawImage(rect, target.CGImage);
                CGBitmapContext cgBitmapContext1 = currentContext.AsBitmapContext();
                vImageBuffer vImageBuffer1 = new vImageBuffer();
                vImageBuffer1.Data = cgBitmapContext1.Data;
                vImageBuffer1.Width = (int)cgBitmapContext1.Width;
                vImageBuffer1.Height = (int)cgBitmapContext1.Height;
                vImageBuffer1.BytesPerRow = (int)cgBitmapContext1.BytesPerRow;
                UIGraphics.BeginImageContextWithOptions(target.Size, false, UIScreen.MainScreen.Scale);
                CGBitmapContext cgBitmapContext2 = UIGraphics.GetCurrentContext().AsBitmapContext();
                vImageBuffer vImageBuffer2 = new vImageBuffer();
                vImageBuffer2.Data = cgBitmapContext2.Data;
                vImageBuffer2.Width = (int)cgBitmapContext2.Width;
                vImageBuffer2.Height = (int)cgBitmapContext2.Height;
                vImageBuffer2.BytesPerRow = (int)cgBitmapContext2.BytesPerRow;
                if (flag1)
                {
                    uint num1 = (uint)Math.Floor((double)(blurRadius * UIScreen.MainScreen.Scale * 3f) * Math.Sqrt(2.0 * Math.PI) / 4.0 + 0.5);
                    if ((int)(num1 % 2U) != 1)
                        ++num1;
                    long num2 = (long)vImage.BoxConvolveARGB8888(ref vImageBuffer1, ref vImageBuffer2, IntPtr.Zero, 0, 0, num1, num1, Pixel8888.Zero, vImageFlags.EdgeExtend);
                    long num3 = (long)vImage.BoxConvolveARGB8888(ref vImageBuffer2, ref vImageBuffer1, IntPtr.Zero, 0, 0, num1, num1, Pixel8888.Zero, vImageFlags.EdgeExtend);
                    long num4 = (long)vImage.BoxConvolveARGB8888(ref vImageBuffer1, ref vImageBuffer2, IntPtr.Zero, 0, 0, num1, num1, Pixel8888.Zero, vImageFlags.EdgeExtend);
                }
                bool flag3 = false;
                if (flag2)
                {
                    nfloat nfloat = saturationDeltaFactor;
                    nfloat[] nfloatArray1 = new nfloat[16];
                    int index1 = 0;
                    nfloatArray1[index1] = 0.0722f + 0.9278f * nfloat;
                    int index2 = 1;
                    nfloatArray1[index2] = 0.0722f - 0.0722f * nfloat;
                    int index3 = 2;
                    nfloatArray1[index3] = 0.0722f - 0.0722f * nfloat;
                    int index4 = 3;
                    nfloatArray1[index4] = 0.0f;
                    int index5 = 4;
                    nfloatArray1[index5] = 0.7152f - 0.7152f * nfloat;
                    int index6 = 5;
                    nfloatArray1[index6] = 0.7152f + 0.2848f * nfloat;
                    int index7 = 6;
                    nfloatArray1[index7] = 0.7152f - 0.7152f * nfloat;
                    int index8 = 7;
                    nfloatArray1[index8] = 0.0f;
                    int index9 = 8;
                    nfloatArray1[index9] = 0.2126f - 0.2126f * nfloat;
                    int index10 = 9;
                    nfloatArray1[index10] = 0.2126f - 0.2126f * nfloat;
                    int index11 = 10;
                    nfloatArray1[index11] = 0.2126f + 0.7873f * nfloat;
                    int index12 = 11;
                    nfloatArray1[index12] = 0.0f;
                    int index13 = 12;
                    nfloatArray1[index13] = 0.0f;
                    int index14 = 13;
                    nfloatArray1[index14] = 0.0f;
                    int index15 = 14;
                    nfloatArray1[index15] = 0.0f;
                    int index16 = 15;
                    nfloatArray1[index16] = 1f;
                    nfloat[] nfloatArray2 = nfloatArray1;
                    int divisor = 256;
                    int length = nfloatArray2.Length / 4;
                    short[] matrix = new short[length];
                    for (int index17 = 0; index17 < length; ++index17)
                        matrix[index17] = (short)Math.Round((double)(nfloatArray2[index17] * divisor));
                    if (flag1)
                    {
                        long num = (long)vImage.MatrixMultiplyARGB8888(ref vImageBuffer2, ref vImageBuffer1, matrix, divisor, null, null, vImageFlags.NoFlags);
                        flag3 = true;
                    }
                    else
                    {
                        long num1 = (long)vImage.MatrixMultiplyARGB8888(ref vImageBuffer1, ref vImageBuffer2, matrix, divisor, null, null, vImageFlags.NoFlags);
                    }
                }
                if (!flag3)
                    uiImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
                if (flag3)
                    uiImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
            }
            UIGraphics.BeginImageContextWithOptions(target.Size, false, UIScreen.MainScreen.Scale);
            CGContext currentContext1 = UIGraphics.GetCurrentContext();
            currentContext1.ScaleCTM(1f, -1f);
            currentContext1.TranslateCTM(0, -target.Size.Height);
            currentContext1.DrawImage(rect, target.CGImage);
            if (flag1)
            {
                currentContext1.SaveState();
                if (maskImage != null)
                    currentContext1.ClipToMask(rect, maskImage.CGImage);
                currentContext1.DrawImage(rect, uiImage.CGImage);
                currentContext1.RestoreState();
            }
            if (tintColor != null)
            {
                currentContext1.SaveState();
                currentContext1.SetFillColor(tintColor.CGColor);
                currentContext1.FillRect(rect);
                currentContext1.RestoreState();
            }
            UIImage currentImageContext = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return currentImageContext;
        }
	}
}
#endif