using System;
using System.Linq;
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
using System.Collections;

namespace IndoorWorx.Infrastructure.Converters
{
    public class CollectionSizeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;
            if (value is IEnumerable)
            {
                var count = (value as IEnumerable).Cast<object>().Count();
                int mustBeGreaterThan = 0;
                if (parameter != null)
                {
                    int.TryParse(parameter.ToString(), out mustBeGreaterThan);
                }
                if (count > mustBeGreaterThan)
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
