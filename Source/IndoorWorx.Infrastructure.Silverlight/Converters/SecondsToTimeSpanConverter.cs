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
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            var valueAsSeconds = System.Convert.ToDouble(value);
            var result = TimeSpan.FromSeconds(valueAsSeconds);
            if ("string".Equals(parameter))
                return string.Format("{0:N2}:{1:N2}:{2:N2}", result.Hours, result.Minutes, result.Seconds);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
