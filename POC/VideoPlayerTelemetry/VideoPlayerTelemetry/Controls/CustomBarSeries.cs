using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;

namespace VideoPlayerTelemetry.Controls
{
    public class CustomBarSeries : ColumnSeries
    {
        protected override void UpdateDataPoint(DataPoint dataPoint)
        {
            // That set the height and width.
            base.UpdateDataPoint(dataPoint);
            // Now we override the part about setting the width
            object category = dataPoint.ActualIndependentValue;
            var coordinateRange = GetCategoryRange(category);
            double minimum = (double)coordinateRange.Minimum.Value;
            double maximum = (double)coordinateRange.Maximum.Value;
            double coordinateRangeWidth = (maximum - minimum);
            const int WIDTH_MULTIPLIER = 1; // Harcoded to 0.8 in the parent. Could make this a dependency property
            double segmentWidth = coordinateRangeWidth * WIDTH_MULTIPLIER;
            var columnSeries = SeriesHost.Series.OfType<ColumnSeries>().Where(series => series.ActualIndependentAxis == ActualIndependentAxis);
            int numberOfSeries = columnSeries.Count();
            double columnWidth = segmentWidth / numberOfSeries;
            int seriesIndex = columnSeries.ToList().IndexOf(this);
            double offset = seriesIndex * Math.Round(columnWidth) + coordinateRangeWidth * 0.1;
            double dataPointX = minimum + offset;
            double left = Math.Round(dataPointX);
            double width = Math.Round(columnWidth);
            Canvas.SetLeft(dataPoint, left);
            dataPoint.Width = width;
        } 
    }
}
