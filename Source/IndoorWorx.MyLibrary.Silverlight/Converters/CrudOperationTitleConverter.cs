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
using IndoorWorx.MyLibrary.Resources;

namespace IndoorWorx.MyLibrary.Converters
{
    public class CrudOperationTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var title = value == null ? string.Empty : value.ToString();
            if(parameter != null)
            {
                var mode = (CrudOperation)parameter;
                switch (mode)
                {
                    case CrudOperation.Create:
                        return MyLibraryResources.NewIntervalTitle;
                    case CrudOperation.Read:
                        return string.Format(MyLibraryResources.ViewIntervalTitle, title);
                    case CrudOperation.Update:
                        return string.Format(MyLibraryResources.EditIntervalTitle, title);
                    case CrudOperation.Delete:
                        return string.Format(MyLibraryResources.DeleteIntervalTitle, title);
                }
            }
            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
