using System;
using System.Drawing;

namespace Testinator.Server
{
    public static class BitmapExtensionMethods
    {
        public static Image GetThumbnail(this Bitmap bitmap)
        {
            // w 40
            // h 30
            double scaleX = bitmap.Width / 40;
            double scaleY = bitmap.Height / 30;

            if (scaleX > scaleY)
            {
                var Height = (int)Math.Floor(bitmap.Height / scaleX);
                var Width = (int)Math.Floor(bitmap.Width / scaleX);
                return bitmap.GetThumbnailImage(Width, Height, () => false, IntPtr.Zero);
            }
            else
            {
                var Height = (int)Math.Floor(bitmap.Height / scaleY);
                var Width = (int)Math.Floor(bitmap.Width / scaleY);
                return bitmap.GetThumbnailImage(Width, Height, () => false, IntPtr.Zero);
            }

        }

    }
}
