using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Jeopardy.Core.Wpf.Converters
{
    public class ByteArrayToBitmapImageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is byte[] bytes && bytes.Length > 0)
            {
                using var ms = new MemoryStream(bytes);
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
                //return new ImageSourceConverter().ConvertFrom(bytes);
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BitmapImage bi)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bi));
                using var ms = new MemoryStream();
                encoder.Save(ms);
                return ms.ToArray();
            }

            return null;
        }

        public static BitmapImage? Convert(byte[] data) => (BitmapImage?)new ByteArrayToBitmapImageConverter().Convert(data, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
        public static byte[]? ConvertBack(BitmapImage bitmapImage) => (byte[]?)new ByteArrayToBitmapImageConverter().ConvertBack(bitmapImage, typeof(byte[]), null, CultureInfo.CurrentCulture);
    }
}
