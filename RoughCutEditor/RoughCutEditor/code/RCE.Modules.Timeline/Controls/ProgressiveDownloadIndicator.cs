// <copyright file="ProgressiveDownloadIndicator.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ProgressiveDownloadIndicator.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    [TemplatePart(Name = "IndicatorContainer", Type = typeof(Canvas))]
    public class ProgressiveDownloadIndicator : Control
    {
        public static readonly DependencyProperty DownloadingIndicatorBackgroundProperty = DependencyProperty.Register("DownloadingIndicatorBackground", typeof(Brush), typeof(ProgressiveDownloadIndicator), new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public static readonly DependencyProperty DownloadedPortionBackgroundProperty = DependencyProperty.Register("DownloadedPortionBackground", typeof(Brush), typeof(ProgressiveDownloadIndicator), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));

        private Canvas indicatorContainer;

        private Rectangle currentIndicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressiveDownloadIndicator"/> class.
        /// </summary>
        public ProgressiveDownloadIndicator()
        {
            this.DefaultStyleKey = typeof(ProgressiveDownloadIndicator);
        }
        
        public Brush DownloadedPortionBackground
        {
            get { return (Brush)GetValue(DownloadedPortionBackgroundProperty); }
            set { SetValue(DownloadedPortionBackgroundProperty, value); }
        }

        public Brush DownloadingIndicatorBackground
        {
            get { return (Brush)GetValue(DownloadingIndicatorBackgroundProperty); }
            set { SetValue(DownloadingIndicatorBackgroundProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.indicatorContainer = GetTemplateChild("IndicatorContainer") as Canvas;
        }

        public void Refresh()
        {
            if (this.indicatorContainer != null && this.indicatorContainer.Children != null)
            {
                foreach (UIElement uie in this.indicatorContainer.Children)
                {
                    Rectangle rectangle = uie as Rectangle;

                    if (rectangle != null)
                    {
                        ProgresIndicator progressIndicator = rectangle.Tag as ProgresIndicator;

                        if (progressIndicator != null)
                        {
                            this.currentIndicator.Width = this.ActualWidth * (progressIndicator.DownloadProgress - progressIndicator.DownloadProgressOffset);

                            rectangle.SetValue(Canvas.LeftProperty, this.ActualWidth * progressIndicator.DownloadProgressOffset);
                        }
                    }
                }
            }
        }
        
        public void ReportProgress(double downloadProgress, double downloadProgressOffset)
        {
            this.AddIndicator(downloadProgress, downloadProgressOffset);

            if (this.currentIndicator != null)
            {
                this.currentIndicator.Width = this.ActualWidth * (downloadProgress - downloadProgressOffset);
                if (downloadProgress == 1)
                {
                    this.currentIndicator.Fill = this.DownloadedPortionBackground;
                }
            } 
        }

        private void AddIndicator(double downloadProgress, double downloadProgressOffset)
        {
            if (this.currentIndicator != null && (double)this.currentIndicator.GetValue(Canvas.LeftProperty) == ((downloadProgressOffset == 0) ? 0 : this.ActualWidth * downloadProgressOffset))
            {
                return;
            }

            if (this.currentIndicator != null)
            {
                this.currentIndicator.Fill = this.DownloadedPortionBackground;
            }

            // check to see if there is an indicator already added for this byte range
            this.currentIndicator = this.indicatorContainer != null ? this.indicatorContainer.Children.Where((uie) =>
                                                                                               uie is Rectangle && (double)(uie as Rectangle).GetValue(Canvas.LeftProperty) ==
                                                                                               ((downloadProgressOffset == 0) ? 0 : this.ActualWidth * downloadProgressOffset))
                                                                                               .FirstOrDefault() as Rectangle : null;

            // no indicators have been added so far
            if (this.currentIndicator == null && this.indicatorContainer != null)
            {
                this.currentIndicator = new Rectangle { Width = 0, Height = this.indicatorContainer.ActualHeight, Fill = this.DownloadingIndicatorBackground, Tag = new ProgresIndicator(downloadProgress, downloadProgressOffset) };
                this.indicatorContainer.Children.Add(this.currentIndicator);
                this.currentIndicator.SetValue(Canvas.LeftProperty, ((downloadProgressOffset == 0) ? 0 : this.ActualWidth * downloadProgressOffset));
                this.currentIndicator.SetValue(Canvas.TopProperty, 0.0);
            }
        }

        private class ProgresIndicator
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ProgressiveDownloadIndicator"/> class.
            /// </summary>
            public ProgresIndicator(double downloadProgress, double downloadProgressOffset)
            {
                this.DownloadProgress = downloadProgress;
                this.DownloadProgressOffset = downloadProgressOffset;
            }

            public double DownloadProgress { get; private set; }

            public double DownloadProgressOffset { get; private set; }
        }
    }
}