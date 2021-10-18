using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MyPhotoAlbum.Converters
{
    public class AlbumFieldConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var parameterStr = parameter.ToString();
            string result = "";

            if (parameterStr == "Date")
                result = value.ToString().Replace("0:00:00", "");

            if (parameterStr == "PhotoName")
                result = value.ToString().Replace(".jpg", "");

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
