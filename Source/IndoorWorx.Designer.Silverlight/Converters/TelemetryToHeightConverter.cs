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
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;

namespace IndoorWorx.Designer.Converters
{
    public class TelemetryToHeightConverter : IValueConverter
    {
        public static readonly double HeightPerPercent = 1.0;//1px per 1 percent.
        
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var telemetry = value as ICollection<Telemetry>;
            if (telemetry != null)
            {
                return telemetry.Max(x => x.PercentageThreshold) * 100 * HeightPerPercent;
            }
            throw new Exception("This converter can only be used for a collection of Telemetry.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
