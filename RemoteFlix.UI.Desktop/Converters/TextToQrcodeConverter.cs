using QRCoder;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RemoteFlix.UI.Desktop.Converters
{
    class TextToQrcodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(value.ToString(), QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, false);

                return BitmapToImageSource(qrCodeImage);
            }

            return null;
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
