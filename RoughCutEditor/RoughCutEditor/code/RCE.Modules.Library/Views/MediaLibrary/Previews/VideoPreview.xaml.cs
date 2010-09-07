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

namespace RCE.Modules.Library
{
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Web.Media.SmoothStreaming;
    using Services.Contracts;
    using SMPTETimecode;

    /// <summary>
    /// Preview control for <see cref="VideoAsset"/>.
    /// </summary>
    public partial class VideoPreview : AssetPreview
    {
        /// <summary>
        /// The Seperator used between the position and duration of the video.
        /// </summary>
        private const string DurationSeparator = " | ";

        /// <summary>
        /// Margin in the X axis.
        /// </summary>
        private const double MarginX = 24;

        /// <summary>
        /// Margin in the Y axis. It is used in resizing the preview.
        /// </summary>
        private const double MarginY = 35;

        /// <summary>
        /// The <see cref="IThumbnailService"/> service used to get the asset thumbnail frame.
        /// </summary>
        private readonly IThumbnailService thumbnailService;

        /// <summary>
        /// The <see cref="DependencyProperty"/> to have the Asset of the preview.
        /// </summary>
        private static readonly DependencyProperty AssetProperty =
            DependencyProperty.RegisterAttached("Asset", typeof(Asset), typeof(VideoPreview), new PropertyMetadata(AssetChanged));

        /// <summary>
        /// Flag indiating if the control is loading for the first time.
        /// true if has been loaded.
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPreview"/> class.
        /// </summary>
        public VideoPreview()
        {
            InitializeComponent();

            this.MouseEnter += (sender, e) => this.ShowInterface.Begin();
            this.MouseLeave += (sender, e) => this.HideInterface.Begin();
            this.Player.StartMediaPlay += this.Player_StartPlay;
            this.Player.ExpandToFullScreen += this.Player_ExpandToFullScreen;
            this.Player.MetadataClick += this.OnPlayerMetadataClick;
            this.Player.MediaPositionChanged += this.Player_MediaPositionChanged;
            this.Player.FrameRateParsed += this.Player_FrameRateParsed;
            this.Player.MediaErrorExpandedChanged += this.Player_MediaErrorExpandedChanged;
            this.Loaded += this.VideoPreview_Loaded;
            this.thumbnailService = ServiceLocator.Current.GetInstance(typeof(IThumbnailService)) as IThumbnailService;
        }

        /// <summary>
        /// Occurs when metadata button clicked in player control.
        /// </summary>
        public event EventHandler MetadataClick;

        /// <summary>
        /// Occurs when Play button clicked in player control.
        /// </summary>
        public event EventHandler Playing;

        /// <summary>
        /// Occurs when Stop button clicked in player control.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Occurs when FullScreen button clicked in player control.
        /// </summary>
        public event EventHandler TogglingFullScreen;

        /// <summary>
        /// Gets or sets a value indicating whether preview is in full screen.
        /// </summary>
        /// <value>
        /// A <seealso cref="bool"/> value. <c>true</c> if this instance is full screen; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullScreen { get; set; }

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The <see cref="Asset"/>.</value>
        public override Asset Asset
        {
            get 
            { 
                return GetAsset(this); 
            }

            set 
            { 
                SetAsset(this, value);
            }
        }

        /// <summary>
        /// Gets the video asset associated with the preview.
        /// </summary>
        /// <value>The video asset.</value>
        private VideoAsset VideoAsset
        {
            get { return this.Asset as VideoAsset; }
        }

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/>.</param>
        /// <returns>The <see cref="VideoAsset"/>.</returns>
        public static Asset GetAsset(DependencyObject obj)
        {
            object value = obj.GetValue(AssetProperty);

            return value as Asset;
        }

        /// <summary>
        /// Sets the asset.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/>.</param>
        /// <param name="value">The <see cref="VideoAsset"/>.</param>
        public static void SetAsset(DependencyObject obj, Asset value)
        {
            obj.SetValue(AssetProperty, value);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            if (this.Player.MediaElement != null)
            {
                this.AssetContainer.Children.Remove(this.Player.MediaElement);
                this.Player.MediaElement.MediaOpened -= this.MediaElement_MediaOpened;
                this.Player.StopMedia();
                this.FramePreviewImage.Visibility = Visibility.Visible;
                this.CurrentPositionLabel.Text = String.Empty;
                this.UpdateMediaError(false);
                this.OnStopping();
            }
        }

        /// <summary>
        /// Scales the current preview control to the specified size.
        /// </summary>
        /// <param name="size">The size to which the preview control is to be scaled.</param>
        public override void Scale(Size size)
        {
            if (size.Width > MarginX && size.Width > MarginY)
            {
                VideoGrid.Width = size.Width;
                VideoGrid.Height = size.Height;
                
                Size previewSize = this.GetSizeMaintainingAspectRatio(size.Width, size.Height);
                FramePreviewImage.Width = previewSize.Width;
                FramePreviewImage.Height = previewSize.Height;

                if (this.Player.MediaElement != null)
                {
                    this.Player.MediaElement.Width = previewSize.Width;
                    this.Player.MediaElement.Height = previewSize.Height;
                }
            }
        }

        /// <summary>
        /// Change the preview image of the asset as asset is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/>.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void AssetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoPreview preview = d as VideoPreview;

            if (preview != null && preview.thumbnailService != null)
            {
                Uri frameUri = new Uri(preview.thumbnailService.GetThumbnailSource(preview.Asset), UriKind.RelativeOrAbsolute);
                preview.FramePreviewImage.Source = new BitmapImage(frameUri);
            }
        }

        /// <summary>
        /// Triggers the <see cref="MetadataClick"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPlayerMetadataClick(object sender, EventArgs e)
        {
            this.OnMetadataClick();
        }

        /// <summary>
        /// Handles the MediaPositionChanged event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Player_MediaPositionChanged(object sender, EventArgs e)
        {
            if (this.Player.MediaElement != null)
            {
                this.CurrentPositionLabel.Text = TimeCode.FromSeconds(this.Player.Position.TotalSeconds, this.VideoAsset.Duration.FrameRate).ToString() + DurationSeparator;
            }
        }

        /// <summary>
        /// Handles the StartPlay event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Player_StartPlay(object sender, EventArgs e)
        {
            if (this.Player.MediaElement == null)
            {
                this.OnPlaying();

                this.Player.MediaElement = new CoreSmoothStreamingMediaElement
                {
                    Width = FramePreviewImage.Width * FramePreviewImageRenderTransform.ScaleX,
                    Height = FramePreviewImage.Height * FramePreviewImageRenderTransform.ScaleY,
                    AutoPlay = false
                };

                this.Player.MediaElement.MediaOpened += this.MediaElement_MediaOpened;

                this.Player.SetSource(this.Asset);
                
                this.FramePreviewImage.Visibility = Visibility.Collapsed;
                this.AssetContainer.Children.Add(this.Player.MediaElement);

                this.Player.SetSmpteFrameRate(((VideoAsset)this.Asset).Duration.FrameRate);
            }
            else
            {
                if (this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Closed || this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Playing)
                {
                    this.UpdateMediaError(false);
                    this.Player.RetrySource(this.Asset);
                }
            }
        }

        /// <summary>
        /// Handles the MediaOpened event of the CurrentMediaElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeCode currentDuration = TimeCode.FromSeconds(this.Player.MediaElement.Duration.TotalSeconds, this.VideoAsset.FrameRate);

            this.VideoAsset.Duration = currentDuration;

            SmoothStreamingVideoAsset smoothStreamingVideoAsset = this.Asset as SmoothStreamingVideoAsset;

            if (smoothStreamingVideoAsset != null && smoothStreamingVideoAsset.StartPosition != this.Player.MediaElement.StartPosition.TotalSeconds)
            {
                smoothStreamingVideoAsset.StartPosition = this.Player.MediaElement.StartPosition.TotalSeconds;
            }
        }

        /// <summary>
        /// Handles the FrameRateParsed event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Player_FrameRateParsed(object sender, DataEventArgs<SmpteFrameRate> e)
        {
            if (e.Data != SmpteFrameRate.Unknown)
            {
                this.Player.SetSmpteFrameRate(e.Data);
                ((VideoAsset)this.Asset).Duration = TimeCode.FromSeconds(((VideoAsset)this.Asset).Duration.TotalSeconds, e.Data);
                ((VideoAsset) this.Asset).FrameRate = e.Data;
            }
        }

        /// <summary>
        /// Handles the ExpandToFullScreen event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Player_ExpandToFullScreen(object sender, EventArgs e)
        {
            this.IsFullScreen = !this.IsFullScreen;

            this.OnTogglingFullScreen();
        }

        /// <summary>
        /// Handles the Loaded event of the VideoPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void VideoPreview_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.isLoaded)
            {
                Size aspectSize = this.GetSizeMaintainingAspectRatio(this.ActualWidth, this.ActualHeight);
                this.FramePreviewImage.Width = aspectSize.Width;
                this.FramePreviewImage.Height = aspectSize.Height;
                this.FramePreviewImage.Visibility = Visibility.Visible;
                this.isLoaded = true;

                this.Player.IsLive = this.Asset.ResourceType == ResourceType.LiveSmoothStream;
                this.Player.IsAdaptiveAsset = this.Asset.IsAdaptiveAsset;
            }
        }

        /// <summary>
        /// Triggers the <see cref="Playing"/> event.
        /// </summary>
        private void OnPlaying()
        {
            EventHandler playingHandler = this.Playing;
            if (playingHandler != null)
            {
                playingHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Triggers the <see cref="Stopping"/> event.
        /// </summary>
        private void OnStopping()
        {
            EventHandler stoppingHandler = this.Stopping;
            if (stoppingHandler != null)
            {
                stoppingHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the AddAsset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddAsset_Click(object sender, RoutedEventArgs e)
        {
            this.OnAddingAsset();
        }

        /// <summary>
        /// Triggers the <see cref="TogglingFullScreen"/> event.
        /// </summary>
        private void OnTogglingFullScreen()
        {
            EventHandler togglingFullScreenHandler = this.TogglingFullScreen;
            if (togglingFullScreenHandler != null)
            {
                togglingFullScreenHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns the best fit size for the asset in the given size.
        /// </summary>
        /// <param name="width">Max possible width.</param>
        /// <param name="height">Max possible height.</param>
        /// <returns>Returns the best fit size for the asset.</returns>
        private Size GetSizeMaintainingAspectRatio(double width, double height)
        {
            width -= MarginX;

            if (width <= 0)
            {
                width = 1;
            }

            height -= MarginY;

            if (height <= 0)
            {
                height = 1;
            }
           
            VideoAsset videoAsset = this.Asset as VideoAsset;

            if (videoAsset != null)
            {
                double aspectRatioWidth = Convert.ToDouble(videoAsset.Width.GetValueOrDefault());
                double aspectRatioHeight = Convert.ToDouble(videoAsset.Height.GetValueOrDefault());

                if (aspectRatioWidth == 0 || aspectRatioHeight == 0)
                {
                    return new Size(width, height);
                }

                if (width >= height * aspectRatioWidth / aspectRatioHeight)
                {
                    return new Size(height * aspectRatioWidth / aspectRatioHeight, height);
                }
                else
                {
                    return new Size(width, width * aspectRatioHeight / aspectRatioWidth);
                }
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Triggers the <see cref="MetadataClick"/> event.
        /// </summary>
        private void OnMetadataClick()
        {
            EventHandler metadataClickHandler = this.MetadataClick;
            if (metadataClickHandler != null)
            {
                metadataClickHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the MediaErrorExpandedChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void Player_MediaErrorExpandedChanged(object sender, RoutedEventArgs e)
        {
            this.UpdateMediaError(true);
        }

        /// <summary>
        /// Update the media error state.
        /// </summary>
        /// <param name="expanded">If the media error should be expanded or not.</param>
        private void UpdateMediaError(bool expanded)
        {
            if (expanded)
            {
                VisualStateManager.GoToState(this, "MediaErrorExpanded", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "MediaErrorCollapsed", true);
            }
        }
    }
}
