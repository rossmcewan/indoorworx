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
using IndoorWorx.Designer.Views;
using System.Collections.Generic;
using System.Collections;
using IndoorWorx.Designer.Controls;
using Telerik.Windows.Controls;

namespace IndoorWorx.Designer.Converters
{
    public class IntervalToTabItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var interval = value as Interval;
            if (interval != null)
            {
                var view = IoC.Resolve<IIntervalDesignerView>();
                view.Model.Interval = interval;
                if (parameter != null && "nochoice".Equals(parameter.ToString()))
                    view.Model.AllowSingleOrMultipleVideoSelection = false;

                var tabItem = new RadTabItem();
                tabItem.SetValue(RadTabItem.TabOrientationProperty, Orientation.Horizontal);
                tabItem.Header = interval.Title;
                tabItem.Content = view;

                return tabItem;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var tabItem = value as RadTabItem;
            if (tabItem != null)
            {
                var view = tabItem.Content as IIntervalDesignerView;
                if (view != null)
                {
                    return view.Model.Interval;
                }
            }
            return null;
        }
    }
}
