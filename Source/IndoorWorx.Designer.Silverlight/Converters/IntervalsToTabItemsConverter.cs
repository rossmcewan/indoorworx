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
using System.Collections;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure;
using IndoorWorx.Designer.Views;
using IndoorWorx.Designer.Controls;
using Telerik.Windows.Controls;

namespace IndoorWorx.Designer.Converters
{
    public class IntervalsToTabItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var intervals = value as IEnumerable;
            if (intervals != null)
            {
                var result = new List<RadTabItem>();
                foreach (Interval interval in intervals)
                {
                    var view = IoC.Resolve<IIntervalDesignerView>();
                    view.Model.Interval = interval;
                    if (parameter != null && "nochoice".Equals(parameter.ToString()))
                        view.Model.AllowSingleOrMultipleVideoSelection = false;

                    var tabItem = new RadTabItem();
                    tabItem.SetValue(RadTabItem.TabOrientationProperty, Orientation.Horizontal);
                    tabItem.Header = interval.Title;
                    tabItem.Content = view;

                    result.Add(tabItem);
                }
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
