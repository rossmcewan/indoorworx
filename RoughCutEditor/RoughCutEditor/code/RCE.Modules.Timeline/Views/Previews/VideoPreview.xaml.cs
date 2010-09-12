// <copyright file="VideoPreview.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: VideoPreview.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.Practices.ServiceLocation;
    using SMPTETimecode;

    /// <summary>
    /// Preview control for the <see cref="VideoAsset"/> in the timeline.
    /// </summary>
    public partial class VideoPreview : IPreview
    {
        /// <summary>
        /// Width of the frame.
        /// </summary>
        private const double FrameWidth = 80;

        /// <summary>
        /// Height of the frame.
        /// </summary>
        private const double FrameHeight = 60;

        /// <summary>
        /// The <see cref="IThumbnailService"/> service used to get the asset thumbnail frame.
        /// </summary>
        private readonly IThumbnailService thumbnailService;

        /// <summary>
        /// List maintains the frame images of the asets.
        /// </summary>
        private IList<Image> frameImages;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPreview"/> class.
        /// </summary>
        /// <param name="element">The <see cref="TimelineElement"/> element.</param>
        public VideoPreview(TimelineElement element)
        {
            InitializeComponent();

            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.DataContext = element;
            this.thumbnailService = ServiceLocator.Current.GetInstance(typeof(IThumbnailService)) as IThumbnailService;
        }

        private TimelineElement Element
        {
            get { return this.DataContext as TimelineElement; }
        }

        /// <summary>
        /// Sets the current video asset as selected asset.
        /// </summary>
        /// <param name="selected">If set to <c>true</c> [selected].</param>
        public void SetSelected(bool selected)
        {
            this.SelectionBox.Visibility = selected ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the download progress bar.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <param name="offset">The offset.</param>
        public void ShowDownloadProgressBar(double progress, double offset)
        {
            if (this.DownloadProgressBar.Visibility != Visibility.Visible)
            {
                this.DownloadProgressBar.Visibility = Visibility.Visible;
            }

            this.DownloadProgressBar.ReportProgress(progress, offset);
        }

        private double beforeRefreshSize = 0D;
        public void BeforeRefresh()
        {
            beforeRefreshSize = this.TelemetryView.Width;
        }

        public void AfterRefresh()
        {
            this.TelemetryView.Width = beforeRefreshSize;
        }

        private bool firstCall = true;
        private double initialWidth = 0D;
        /// <summary>
        /// Refreshes the preview. This includes the progress bar and the filmstrip.
        /// </summary>
        /// <param name="currentWidth">Width of the current element.</param>
        public void Refresh(double currentWidth)
        {
            Refresh(currentWidth, RefreshSource.Any);
        }

        public void Refresh(double currentWidth, RefreshSource refreshSource)
        {
            //if (firstCall || refreshSource == RefreshSource.Drag || refreshSource == RefreshSource.Zoom)
            //{
            //    this.initialWidth = currentWidth;
            //    firstCall = false;
            //}
            this.DownloadProgressBar.Refresh();
            this.UpdateFilmstrip(currentWidth);
            this.Element.UpdateTelemetry();
            //this.TelemetryView.Width = initialWidth;
        }

        /// <summary>
        /// Updates the filmstrip.
        /// </summary>
        /// <param name="currentWidth">Width of the current element.</param>
        public void UpdateFilmstrip(double currentWidth)
        {
            if (this.frameImages == null)
            {
                this.frameImages = new List<Image>();
            }

            int i;

            double numberOfFrames = Math.Ceiling(currentWidth / FrameWidth);

            for (i = this.frameImages.Count; i < numberOfFrames; i++)
            {
                Image image = new Image
                                  {
                                      Visibility = Visibility.Collapsed,
                                      Width = FrameWidth,
                                      Height = FrameHeight
                                  };

                this.frameImages.Add(image);
            }

            this.UpdateFrames(numberOfFrames, currentWidth);

            i = 0;

            while (i < numberOfFrames)
            {
                Image image = this.frameImages[i];
                image.Visibility = Visibility.Visible;
                i++;
            }

            while (i < this.frameImages.Count)
            {
                this.frameImages[i].Visibility = Visibility.Collapsed;
                i++;
            }
        }

        /// <summary>
        /// Converts pixel to seconds.
        /// </summary>
        /// <param name="px">The pixel value.</param>
        /// <param name="element">The element.</param>
        /// <param name="width">The element width.</param>
        /// <returns>Conversion value fromPixel to seconds as <see cref="double"/>.</returns>
        private static double PixelToSeconds(double px, TimelineElement element, double width)
        {
            width = (width == 0 || double.IsNaN(width)) ? 1 : width;
            
            double absouluteTime = element.Duration.TotalSeconds * px / width;

            if (Double.IsNaN(absouluteTime) || Double.IsInfinity(absouluteTime))
            {
                absouluteTime = 0;
            }

            return Math.Floor(TimeCode.FromAbsoluteTime(absouluteTime, element.Duration.FrameRate).TotalSeconds);
        }

        /// <summary>
        /// Updates the frames.
        /// </summary>
        /// <param name="numberOfFrames">The number of frames.</param>
        /// <param name="currentWidth">Width of the current.</param>
        private void UpdateFrames(double numberOfFrames, double currentWidth)
        {
            this.FramesStackPanel.Children.Clear();

            TimelineElement element = DataContext as TimelineElement;

            if (element != null)
            {
                int totalSecondsPerFrame = (int)PixelToSeconds(FrameWidth, element, currentWidth);
                int startSeconds = (int) element.InPosition.TotalSeconds;

                for (int i = 0; i < numberOfFrames; i++)
                {
                    Image image = this.frameImages[i];

                    string uriString = this.thumbnailService.GetThumbnailSource(element.Asset, startSeconds, image.Width, image.Height);

                    image.Source = new BitmapImage(new Uri(Uri.EscapeUriString(uriString), UriKind.RelativeOrAbsolute));
                    
                    this.FramesStackPanel.Children.Add(image);
                    startSeconds += totalSecondsPerFrame;
                }
            }
        }
    }
}