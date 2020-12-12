using System;
using System.Windows.Data;
using System.Windows.Media;

namespace SmartFlow.views
{
        [ValueConversion(typeof(Byte[]), typeof(ImageSource))]
        public class ImageConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {

                return Users.ByteToImage((byte[])value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
}
