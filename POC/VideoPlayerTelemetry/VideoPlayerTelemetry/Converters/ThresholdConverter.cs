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
using VideoPlayerTelemetry.DataServiceReference;
using VideoPlayerTelemetry.ViewModels;

namespace VideoPlayerTelemetry.Converters
{
    public class ThresholdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var telemetry = value as Telemetry;
            var telemetryPos = telemetry.TimePosition;
            var playbackPos = PlayerViewModel._CurrentPlaybackPosition;            
            if (telemetryPos.TotalSeconds == Math.Round(playbackPos.TotalSeconds) && telemetryPos.TotalSeconds > 0 && playbackPos.TotalSeconds > 0)
            {
                return telemetry.PercentageOfThreshold;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
