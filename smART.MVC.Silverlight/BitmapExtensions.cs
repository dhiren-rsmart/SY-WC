using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO.Png;

namespace smART.MVC.Silverlight
{
public static class BitmapExtensions
{
    public static void SaveToPNG(this WriteableBitmap bitmap)
    {
        if (bitmap != null)
        {
            SaveFileDialog sfd = new SaveFileDialog
                                     {
                                         Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*",
                                         DefaultExt = ".png",
                                         FilterIndex = 1
                                     };

            if ((bool)sfd.ShowDialog())
            {
                var img = bitmap.ToImage();

                #region Not needed anymore

                //var img = new ImageTools.Image(bitmap.PixelWidth, bitmap.PixelHeight);
                //try
                //{
                //    for (int y = 0; y < bitmap.PixelHeight; ++y)
                //    {
                //        for (int x = 0; x < bitmap.PixelWidth; ++x)
                //        {
                //            int pixel = bitmap.Pixels[bitmap.PixelWidth * y + x];
                //            img.SetPixel(x, y,
                //                         (byte)((pixel >> 16) & 0xFF),
                //                         (byte)((pixel >> 8) & 0xFF),
                //                         (byte)(pixel & 0xFF), (byte)((pixel >> 24) & 0xFF)
                //                );
                //        }
                //    }
                //}
                //catch (System.Security.SecurityException)
                //{
                //    //Todo decent message
                //}

                #endregion

                var encoder = new PngEncoder();
                using (Stream stream = sfd.OpenFile())
                {


                    //PngEncoder.Encode(img, stream);
                    stream.Close();
                }
            }
        }
    }
    
}
}