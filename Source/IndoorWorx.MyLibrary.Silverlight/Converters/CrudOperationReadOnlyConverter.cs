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

namespace IndoorWorx.MyLibrary.Converters
{
    public class CrudOperationReadOnlyConverter : IValueConverter
    {
        public bool DefaultValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var op = (CrudOperation)value;
                switch (op)
                {
                    case CrudOperation.Create:
                        return false;
                    case CrudOperation.Read:
                        return true;
                    case CrudOperation.Update:
                        return false;
                    case CrudOperation.Delete:
                        return true;
                }
            }
            return DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
