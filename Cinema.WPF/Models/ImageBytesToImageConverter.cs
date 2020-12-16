using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Cinema.WPF.Models
{
    public class ImageBytesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream((byte[])value);
            var t = new BitmapImage();
            t.BeginInit();
            t.StreamSource = ms;
            t.EndInit();
            return t;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
