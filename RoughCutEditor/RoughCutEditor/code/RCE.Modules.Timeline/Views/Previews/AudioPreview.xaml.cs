// <copyright file="AudioPreview.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: AudioPreview.xaml.cs                     
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
    using System.Windows;
    using Infrastructure.Models;

    /// <summary>
    /// Preview control for the <see cref="AudioAsset"/>.
    /// </summary>
    public partial class AudioPreview : IPreview
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPreview"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public AudioPreview(TimelineElement element)
        {
            InitializeComponent();

            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.DataContext = element;
        }

        /// <summary>
        /// Sets the selected.
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

        /// <summary>
        /// Refreshes the preview. This includes the progress bar.
        /// </summary>
        public void Refresh()
        {
            this.DownloadProgressBar.Refresh();
        }
    }
}
