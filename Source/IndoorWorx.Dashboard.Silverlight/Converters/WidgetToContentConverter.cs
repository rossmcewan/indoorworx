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
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure;
using IndoorWorx.Infrastructure.Widgets;

namespace IndoorWorx.Dashboard.Converters
{
    public class WidgetToContentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var widget = value as Widget;
            if (widget != null)
            {
                var type = Type.GetType(widget.ContentRenderer);
                var renderer = IoC.Resolve(type) as IWidgetContentRenderer;
                if (renderer != null)
                {
                    return renderer.Render();
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
