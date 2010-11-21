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
    public class SecondsToOADateConverter : IValueConverter
    {
        private static DateTime today = DateTime.Today;
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            var valueAsSeconds = System.Convert.ToDouble(value);
            var timespan = TimeSpan.FromSeconds(valueAsSeconds);
            var datetime = new DateTime(today.Year, today.Month, today.Day, timespan.Hours, timespan.Minutes, timespan.Seconds);
            return datetime.ToOADate();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
