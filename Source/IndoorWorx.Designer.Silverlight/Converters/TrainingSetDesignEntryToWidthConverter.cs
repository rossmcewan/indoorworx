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
using System.Collections.Generic;
using IndoorWorx.Designer.Models;

namespace IndoorWorx.Designer.Converters
{
    public class TrainingSetDesignEntryToWidthConverter : IValueConverter
    {
        private static readonly double WidthPerSecond = 0.2;//0.2 pixels per second of video

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var entry = value as TrainingSetDesignEntry;
            if (entry != null)
            {
                return (entry.TimeEnd.TotalSeconds - entry.TimeStart.TotalSeconds) * WidthPerSecond;
            }
            throw new Exception("This converter can only be used for a TrainingSetDesignEntry");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
