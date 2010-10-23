﻿using System;
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

namespace VideoPlayerTelemetry.Converters
{
    public class IntensityColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = Colors.White;    
            if (value != null)
            {
                
                double temp = System.Convert.ToDouble(value);
                if (temp > 100)
                    color= Colors.Red;
                else if (temp > 80)
                     color= Colors.Orange;
                else if (temp > 60)
                   color= Colors.Yellow;
                else  color= Colors.Blue;

            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
