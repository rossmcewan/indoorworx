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
    public class TemplateNavigationUriConverter : IValueConverter
    {
        private string TemplateDetailsUriTemplate = "/IndoorWorx.MyLibrary.Silverlight;component/Pages/TemplateDetailsPage.xaml?id={0}";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var template = value as TrainingSetTemplate;
            if (template != null)
            {
                return new Uri(string.Format(TemplateDetailsUriTemplate, template.Id.ToString()), UriKind.Relative);
            }
            return new Uri(string.Format(TemplateDetailsUriTemplate, "error"), UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
