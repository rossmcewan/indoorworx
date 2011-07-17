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
using System.Globalization;

namespace IndoorWorx.MyLibrary.Converters
{
    public class AlternatingRowBackgroundConverter : IValueConverter
    {
        public Brush NormalBrush { get; set; }
        public Brush AlternateBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Grid element = (Grid)value;
            element.Loaded += Element_Loaded;

            return NormalBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void Element_Loaded(object sender, RoutedEventArgs e)
        {
            Grid element = sender as Grid;

            var parent = this.FindParent(element, x => x is ItemsControl) as ItemsControl;

            if (parent != null)
            {
                var container = parent.ItemContainerGenerator.ContainerFromItem(element.DataContext);

                if (container != null)
                {
                    int index = parent.ItemContainerGenerator.IndexFromContainer(container);

                    if (index % 2 != 0)
                        element.Background = AlternateBrush;
                }
            }
        }

        private DependencyObject FindParent(DependencyObject element, Func<DependencyObject, bool> filter)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);

            if (parent != null)
            {
                if (filter(parent))
                {
                    return parent;
                }

                return FindParent(parent, filter);
            }

            return null;
        }
    }
}
