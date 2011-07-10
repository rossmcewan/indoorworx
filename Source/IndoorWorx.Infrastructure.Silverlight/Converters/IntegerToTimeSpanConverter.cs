using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace IndoorWorx.Infrastructure.Converters
{
    public class IntegerToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                var param = parameter.ToString().ToLower();
                var timeSpan = (TimeSpan)value;
                if(param == "minutes")
                    return timeSpan.Minutes;
                if (param == "seconds")
                    return timeSpan.Seconds;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Int32)
            {
                var param = parameter.ToString().ToLower();
                var val = (Int32)value;
                if(param == "minutes")
                    return TimeSpan.FromMinutes(System.Convert.ToDouble(value));
                if (param == "seconds")
                    return TimeSpan.FromSeconds(System.Convert.ToDouble(value));
            }
            return TimeSpan.Zero;
        }
    }
}
