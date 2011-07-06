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
    public class TemplateOperationToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is CrudOperation)
            {
                var crud = (CrudOperation)value;
                switch (crud)
                {
                    case CrudOperation.Create:
                        return MyLibraryResources.CreateTemplate;
                    case CrudOperation.Read:
                        return MyLibraryResources.ReadTemplate;
                    case CrudOperation.Update:
                        return MyLibraryResources.UpdateTemplate;
                    case CrudOperation.Delete:
                        return MyLibraryResources.DeleteTemplate;
                    default:
                        break;
                }
            }
            throw new Exception("value cannot be null and must be an instance of CrudOperation");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
