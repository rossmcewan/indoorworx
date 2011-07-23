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
    public class SecondsToTimeSpanConverter : IValueConverter
    {
        private static DateTime today = DateTime.Today;
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            var valueAsSeconds = System.Convert.ToDouble(value);
            var result = TimeSpan.FromSeconds(valueAsSeconds);
            if ("string".Equals(parameter))
            {
                var asDateTime = new DateTime(today.Year, today.Month, today.Day, result.Hours, result.Minutes, result.Seconds);
                return asDateTime.ToString("HH:mm:ss");
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter != null && "string".Equals(parameter))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(value.ToString(), out result))
                        return result.TotalSeconds;
                }
            }
            return null;
        }

        #endregion
    }
}
