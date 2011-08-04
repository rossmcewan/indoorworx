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
using IndoorWorx.Infrastructure.Helpers;

namespace IndoorWorx.Infrastructure.Converters
{
    public class TimeSpanToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value);
        }

        private object Convert(object value)
        {
            if (value is TimeSpan)
            {
                var timespan = (TimeSpan)value;
                return DateTimeHelper.ZeroTime.Add(timespan);
            }
            if (value is DateTime)
            {
                var datetime = (DateTime)value;
                return new TimeSpan(0, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
            }           
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value);
        }
    }
}
