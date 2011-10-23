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
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Converters
{
    public class TelemetryToMaxYValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 100;
            var telemetry = value as ICollection<Telemetry>;
            if (telemetry != null)
            {
                if (telemetry.Any())
                {
                    if(parameter == null || "power".Equals(parameter.ToString().ToLower()))
                        return telemetry.Max(x => x.PercentageThreshold) * 1.1;
                    if ("profile".Equals(parameter.ToString().ToLower()))
                        return telemetry.Max(x => x.Altitude) * 1.1;
                }
                return 100;
            }
            throw new Exception("Value must be a collection of Telemetry");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
