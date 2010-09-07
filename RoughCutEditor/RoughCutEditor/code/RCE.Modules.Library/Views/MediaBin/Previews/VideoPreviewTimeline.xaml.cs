﻿// <copyright file="VideoPreviewTimeline.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: VideoPreviewTimeline.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.MediaBin
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using Infrastructure;
    using Infrastructure.Models;
    using Library;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Web.Media.SmoothStreaming;
    using Services.Contracts;
    using SMPTETimecode;

    /// <summary>
    /// Control to display a video file.
    /// </summary>
    public partial class VideoPreviewTimeline : AssetPreview
    {
        /// <summary>
        /// The Seperator used between the position and duration of the video.
        /// </summary>
        private const string DurationSeparator = " | ";

        /// <summary>
        /// Total margin value in x coordinate.
        /// </summary>
        private const double MarginX = 24;

        /// <summary>
        /// Total margin value in y coordinate.
        /// </summary>
        private const double MarginY = 40;

        /// <summary>
        /// The <see cref="IThumbnailService"/> service used to get the asset thumbnail frame.
        /// </summary>
        private readonly IThumbnailService thumbnailService;

        /// <summary>
        /// DependencyProperty for Asset.
        /// </summary>
        private static readonly DependencyProperty AssetProperty =
                DependencyProperty.RegisterAttached("Asset", typeof(Asset), typeof(VideoPreviewTimeline), new PropertyMetadata(AssetChanged));

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// Flag to maintain if the storyboard have been loaded.
        /// </summary>
        private bool interfaceLoaded;

        /// <summary>
        /// True if the <see cref="VideoPreview"/> is loaded.
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// Indicates if the media element is buffering or not.
        /// </summary>
        private bool isBuffering;

        /// <summary>
        /// Current scale size(Current Preview size).
        /// </summary>
        private Size scaleSize;

        /// <summary>
        /// If the asset is in fullscreen or not.
        /// </summary>
        private bool isFullScreen;

        /// <summary>
        /// The Video Mark In / Mark Out associated with the current asset.
        /// </summary>
        private VideoAssetInOut videoInOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPreviewTimeline"/> class.
        /// </summary>
        public VideoPreviewTimeline()
        {
            InitializeComponent();

            this.MouseEnter += this.VideoPreview_MouseEnter;
            this.MouseLeave += this.VideoPreview_MouseLeave;
            this.Loaded += this.VideoAssetControl_Loaded;
            this.Timeline.MovingPlayHead += this.Timeline_MovingPlayHead;
            this.Timeline.PositionChange += this.Timeline_PositionChange;
            this.Timeline.Resized += this.Timeline_Resized;
            this.Player.StartMediaPlay += this.Player_StartPlay;
            this.Player.ExpandToFullScreen += this.Player_ExpandToFullScreen;
            this.Player.MediaPositionChanged += this.Player_MediaPositionChanged;
            this.Player.LiveSeekCompleted += this.Player_LiveSeekCompleted;
            this.Player.SetSubMarkClipClicked += this.Player_SetSubMarkClipClicked;
            this.Player.PlaySubClipClicked += this.Player_PlaySubClipClicked;
            this.Player.FrameRateParsed += this.Player_FrameRateParsed;
            this.Player.MediaErrorExpandedChanged += this.Player_MediaErrorExpandedChanged;
            this.Player.GoToTimeCodeClicked += this.PlayerGoToTimeCodeClicked;
            this.MetadataPanel.MetadataSelected += this.MetadataPanel_MetadataSelected;

            this.configurationService = ServiceLocator.Current.GetInstance(typeof(IConfigurationService)) as IConfigurationService;
            this.thumbnailService = ServiceLocator.Current.GetInstance(typeof(IThumbnailService)) as IThumbnailService;
        }

        /// <summary>
        /// Occurs when user clicks on FullScreen button of the <see cref="Player"/>.
        /// </summary>
        public event EventHandler TogglingFullScreen;

        /// <summary>
        /// Occurs when user clicks on Stop button of the <see cref="Player"/>.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Occurs when user clicks on Play button of the <see cref="Player"/>.
        /// </summary>
        public event EventHandler Playing;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is full screen.
        /// </summary>
        /// <value>
        /// A <seealso cref="bool"/> value.<c>true</c> if this instance is full screen; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullScreen
        {
            get
            {
                return this.isFullScreen;
            }

            set
            {
                this.isFullScreen = value;
                this.TogglePlayerControlsVisibility();
                this.ToggleTimelineVisibility();
                this.ToggleMetadataPanelVisibility();
                this.ToggleMouseWheelEventSubscription();
            }
        }

        /// <summary>
        /// Gets the Mark In position of the asset.
        /// </summary>
        /// <value>The Mark In position.</value>
        public TimeCode InPosition
        {
            get
            {
                return this.Timeline.InPosition;
            }
        }

        /// <summary>
        /// Gets the Mark Out position of the asset.
        /// </summary>
        /// <value>The oMark Out position.</value>
        public TimeCode OutPosition
        {
            get
            {
                return this.Timeline.OutPosition;
            }
        }

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
        public override Asset Asset
        {
            get { return GetAsset(this); }
            set { SetAsset(this, value); }
        }

        /// <summary>
        /// Gets the VideoAssetInOut associdated with the current asset.
        /// </summary>
        /// <value>The current video asset sub clip.</value>
        public VideoAssetInOut VideoInOut
        {
            get
            {
                if (this.videoInOut == null)
                {
                    this.videoInOut = new VideoAssetInOut(this.VideoAsset);
                    this.UpdateVideInOutProperties();
                }

                return this.videoInOut;
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
        /// <param name="obj">The DependencyObject.</param>
        /// <returns>Returns the value of the Asset property.</returns>
        public static Asset GetAsset(DependencyObject obj)
        {
            return obj.GetValue(AssetProperty) as Asset;
        }

        /// <summary>
        /// Sets the asset.
        /// </summary>
        /// <param name="obj">The DependencyObject.</param>
        /// <param name="value">The <see cref="Asset"/>.</param>
        public static void SetAsset(DependencyObject obj, Asset value)
        {
            obj.SetValue(AssetProperty, value);
        }

        /// <summary>
        /// Stop the currently playing media element.
        /// </summary>
        public override void Stop()
        {
            if (this.Player.MediaElement != null)
            {
                this.AssetContainer.Children.Remove(this.Player.MediaElement);
                this.Player.MediaElement.MediaOpened -= this.MediaElement_MediaOpened;
                this.Player.MediaElement.CurrentStateChanged -= this.MediaElement_CurrentStateChanged;
                this.Player.MediaElement.SeekCompleted -= this.MediaElement_SeekCompleted;
                this.Player.StopMedia();
                this.FramePreviewImage.Visibility = Visibility.Visible;
                this.ClearPreviewState();
                this.UpdateMediaError(false);
                this.OnStopping();
            }
        }

        /// <summary>
        /// Scales the current <see cref="VideoPreview"/> to the specified size.
        /// </summary>
        /// <param name="size">The size to scale preview.</param>
        public override void Scale(Size size)
        {
            this.scaleSize = size;

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
        /// Updates the smpte frame rate.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        public void UpdateSmpteFrameRate(SmpteFrameRate frameRate)
        {
        }

        /// <summary>
        /// Assets the changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/>.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void AssetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoPreviewTimeline preview = d as VideoPreviewTimeline;

            if (preview != null && preview.thumbnailService != null)
            {
                Uri frameUri = new Uri(preview.thumbnailService.GetThumbnailSource(preview.Asset), UriKind.RelativeOrAbsolute);
                preview.FramePreviewImage.Source = new BitmapImage(frameUri);

                if (Infrastructure.DragDrop.DragDropManager.GetIsDragSource(d) && e.OldValue != e.NewValue)
                {
                    Infrastructure.DragDrop.DragDropManager.SetDragData(d, preview.VideoInOut);
                }
            }
        }

        /// <summary>
        /// Handles the Loaded event of the VideoAssetControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void VideoAssetControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.isLoaded)
            {
                Size aspectSize = this.GetSizeMaintainingAspectRatio(this.VideoGrid.ActualWidth, this.VideoGrid.ActualHeight);
                this.FramePreviewImage.Width = aspectSize.Width;
                this.FramePreviewImage.Height = aspectSize.Height;

                this.FramePreviewImage.Visibility = Visibility.Visible;

                this.Timeline.SetDuration(this.VideoAsset.Duration);

                if (this.scaleSize.Width != 0 && this.scaleSize.Height != 0)
                {
                    this.Scale(this.scaleSize);
                }

                this.Player.IsLive = this.Asset.ResourceType == ResourceType.LiveSmoothStream;

                this.isLoaded = true;
                this.UpdateVideInOutProperties();
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
        /// Toggles the visibility of the timeline grid.
        /// </summary>
        private void ToggleTimelineVisibility()
        {
            if (this.IsFullScreen)
            {
                this.Timeline.Visibility = Visibility.Visible;

                if (this.Timeline.HasMarkIn)
                {
                    this.MarkInGrid.Visibility = Visibility.Visible;
                }

                if (this.Timeline.HasMarkOut)
                {
                    this.MarkOutGrid.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.Timeline.Visibility = Visibility.Collapsed;
                this.MarkInGrid.Visibility = Visibility.Collapsed;
                this.MarkOutGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Toggles the visibility of the metadata panel.
        /// </summary>
        private void ToggleMetadataPanelVisibility()
        {
            this.MetadataPanel.Visibility = this.IsFullScreen ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Toggles the visibility of the Player controls.
        /// </summary>
        private void TogglePlayerControlsVisibility()
        {
            this.Player.HasSubClipControls = this.IsFullScreen;
            this.Player.HasGoToButton = this.IsFullScreen;
        }

        /// <summary>
        /// Toggles the subscription to the MouseWheel event.
        /// </summary>
        private void ToggleMouseWheelEventSubscription()
        {
            if (this.IsFullScreen)
            {
                Application.Current.RootVisual.MouseWheel += this.RootVisual_MouseWheel;
            }
            else
            {
                Application.Current.RootVisual.MouseWheel -= this.RootVisual_MouseWheel;
            }
        }

        /// <summary>
        /// Called when [mouse wheel] event occures.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void RootVisual_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.Timeline.ZoomTimeline(e.Delta);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the MouseEnter event of the VideoPreview control.
        /// Begin the storyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void VideoPreview_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Timeline.SubClipPlayheadContentControl.Focus();

            if (!this.interfaceLoaded)
            {
                this.interfaceLoaded = true;
                this.ShowInterface.Begin();
                if (Application.Current.RootVisual != null)
                {
                    Application.Current.RootVisual.KeyDown += this.RootVisual_KeyDown;
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the RootVisual control( I and O).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void RootVisual_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Timeline.Visibility == Visibility.Visible)
            {
                if (this.Timeline.CanHandleKeyStroke(e.OriginalSource))
                {
                    switch (e.Key)
                    {
                        case Key.I:
                            this.SetMarkIn(this.Timeline.CurrentPosition);
                            break;
                        case Key.O:
                            this.SetMarkOut(this.Timeline.CurrentPosition);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a WriteableBitmap of the current media element.
        /// </summary>
        /// <returns>The WriteableBitmap created.</returns>
        private WriteableBitmap CreateWriteableBitmap()
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap((int)this.Player.MediaElement.ActualWidth, (int)this.Player.MediaElement.ActualHeight);
            writeableBitmap.Render(this.Player.MediaElement, null);
            writeableBitmap.Invalidate();

            return writeableBitmap;
        }

        /// <summary>
        /// Updates the In/Out position.
        /// </summary>
        private void UpdateVideInOutProperties()
        {
            this.VideoInOut.InPosition = (this.InPosition.TotalSeconds >= this.Timeline.StartOffset.TotalSeconds) ? this.InPosition.TotalSeconds - this.Timeline.StartOffset.TotalSeconds : this.InPosition.TotalSeconds;
            this.VideoInOut.OutPosition = (this.OutPosition.TotalSeconds >= this.Timeline.StartOffset.TotalSeconds) ? this.OutPosition.TotalSeconds - this.Timeline.StartOffset.TotalSeconds : this.OutPosition.TotalSeconds;
        }

        /// <summary>
        /// Sets the mark in value based on the given position.
        /// </summary>
        /// <param name="currentPosition">The current position of the mark in.</param>
        private void SetMarkIn(TimeCode currentPosition)
        {
            if (this.Player.MediaElement != null && this.Timeline.SetMarkIn(currentPosition))
            {
                this.MarkInText.Text = currentPosition.ToString();
                this.MarkInPreview.Source = this.CreateWriteableBitmap();
                this.MarkInGrid.Visibility = Visibility.Visible;
                this.AdjustPreviews();
                this.UpdateVideInOutProperties();
            }
        }

        /// <summary>
        /// Sets the mark out value based on the given position.
        /// </summary>
        /// <param name="currentPosition">The current position of the mark out.</param>
        private void SetMarkOut(TimeCode currentPosition)
        {
            if (this.Player.MediaElement != null && this.Timeline.SetMarkOut(currentPosition))
            {
                this.MarkOutText.Text = currentPosition.ToString();
                this.MarkOutPreview.Source = this.CreateWriteableBitmap();
                this.MarkOutGrid.Visibility = Visibility.Visible;
                this.AdjustPreviews();
                this.UpdateVideInOutProperties();
            }
        }

        /// <summary>
        /// Handles the MouseLeave event of the VideoPreview control.
        /// Stop the storyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void VideoPreview_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.interfaceLoaded)
            {
                this.interfaceLoaded = false;
                this.HideInterface.Begin();
                if (Application.Current.RootVisual != null)
                {
                    Application.Current.RootVisual.KeyDown -= this.RootVisual_KeyDown;
                }
            }
        }

        /// <summary>
        /// Called when [toggling full screen].
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
        /// Returns the best fit size for the asset for the given size.
        /// </summary>
        /// <param name="width">Max possible width.</param>
        /// <param name="height">Max possible height.</param>
        /// <returns>Returns the best fit size for the asset.</returns>
        private Size GetSizeMaintainingAspectRatio(double width, double height)
        {
            width -= MarginX;

            height -= MarginY;

            if (this.IsFullScreen)
            {
                height -= this.Timeline.ActualHeight;
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

        private void ClearPreviewState()
        {
            this.CurrentPositionLabel.Text = String.Empty;
            this.RemoveMarkIn();
            this.RemoveMarkOut();
            this.Timeline.SetAvailableTime(TimeCode.FromAbsoluteTime(0, this.VideoAsset.FrameRate));
            this.EndBuffer();
        }

        /// <summary>
        /// Handles the StartPlay event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Player_StartPlay(object sender, EventArgs e)
        {
            this.Timeline.SubClipPlayheadContentControl.Focus();
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
                this.Player.MediaElement.CurrentStateChanged += this.MediaElement_CurrentStateChanged;
                this.Player.MediaElement.SeekCompleted += this.MediaElement_SeekCompleted;

                this.Player.SetSource(this.Asset);

                this.FramePreviewImage.Visibility = Visibility.Collapsed;
                this.AssetContainer.Children.Add(this.Player.MediaElement);
                this.Player.SetSmpteFrameRate(this.VideoAsset.FrameRate);
            }
            else
            {
                if (this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Closed || this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Playing)
                {
                    this.UpdateMediaError(false);
                    this.Player.RetrySource(this.Asset);
                }

                // this.Player.Position = TimeSpan.FromSeconds(this.Timeline.CurrentPosition.TotalSeconds);
            }
        }

        /// <summary>
        /// Handles the CurrentStateChanged event of the media element.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.Player != null && this.Player.MediaElement != null)
            {
                if (this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Buffering)
                {
                    this.StartBuffer();
                }
                else if (this.isBuffering)
                {
                    this.EndBuffer();
                }
            }
        }

        /// <summary>
        /// Starts the animation of buffering in the player.
        /// </summary>
        private void StartBuffer()
        {
            this.isBuffering = true;
            this.BufferBar.Visibility = Visibility.Visible;
            this.Spinner.BeginAnimation();
        }

        /// <summary>
        /// End the animation of buffering in the player.
        /// </summary>
        private void EndBuffer()
        {
            this.isBuffering = false;
            this.BufferBar.Visibility = Visibility.Collapsed;
            this.Spinner.StopAnimation();
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
                if (this.Asset.IsAdaptiveAsset)
                {
                    double mediaElementDuration;

                    try
                    {
                        mediaElementDuration = this.Player.MediaElement.LivePosition - this.Timeline.StartOffset.TotalSeconds;
                    }
                    catch (NullReferenceException)
                    {
                        mediaElementDuration = this.Player.MediaElement.EndPosition.TotalSeconds;
                    }

                    if (this.Timeline.Duration.TotalSeconds < mediaElementDuration)
                    {
                        TimeCode duration;

                        if ((this.Timeline.Duration.TotalSeconds == 0 && mediaElementDuration != 0) ||
                            (mediaElementDuration - this.Timeline.Duration.TotalSeconds > 300))
                        {
                            duration = TimeCode.FromSeconds(mediaElementDuration, this.VideoAsset.FrameRate);
                        }
                        else
                        {
                            double range = this.Player.MediaElement.LivePosition - this.Timeline.StartOffset.TotalSeconds;
                            double seconds = (this.Player.MediaElement.LivePosition + (range * 0.15)) - this.Timeline.StartOffset.TotalSeconds;
                            duration = TimeCode.FromSeconds(seconds, this.VideoAsset.FrameRate);
                        }

                        this.SetDuration(duration);
                    }

                    this.UpdateAvailableTime(false);
                }

                if (!this.Player.MediaElement.IsSeeking && (this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Playing || this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Paused))
                {
                    TimeCode currentPosition = TimeCode.FromSeconds(this.Player.Position.TotalSeconds, this.VideoAsset.FrameRate);

                    if (this.Player.IsPlaySubClipMode && currentPosition >= this.OutPosition)
                    {
                        currentPosition = this.OutPosition;
                        this.Player.IsPlaySubClipMode = false;
                        this.Player.PauseMedia();
                    }
                        
                    this.CurrentPositionLabel.Text = currentPosition.ToString() + DurationSeparator;

                    currentPosition = this.Timeline.GetPositionWithoutStartOffset(currentPosition);

                    this.Timeline.SetPlayHeadPosition(currentPosition);
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
            this.Player.Position = TimeSpan.FromSeconds(this.Timeline.CurrentPosition.TotalSeconds);
            this.SetDuration(this.Player.MediaElement.Duration);

            if (this.Asset.IsAdaptiveAsset)
            {
                this.Timeline.SetStartTimeCode(TimeCode.FromSeconds(this.Player.MediaElement.StartPosition.TotalSeconds, this.VideoAsset.FrameRate));
            }

            this.UpdateAvailableTime(true);

            SmoothStreamingVideoAsset smoothStreamingVideoAsset = this.Asset as SmoothStreamingVideoAsset;

            if (smoothStreamingVideoAsset != null && smoothStreamingVideoAsset.StartPosition != this.Player.MediaElement.StartPosition.TotalSeconds)
            {
                smoothStreamingVideoAsset.StartPosition = this.Player.MediaElement.StartPosition.TotalSeconds;
            }

            this.MetadataPanel.SetStartOffset(this.Timeline.StartOffset);
            this.MetadataPanel.SetInStreamData(this.Player.InStreamData);

            this.UpdateVideInOutProperties();
        }

        /// <summary>
        /// Handles the LiveSeekCompleted event of the Player.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Player_LiveSeekCompleted(object sender, DataEventArgs<bool> e)
        {
            if (e.Data)
            {
                if (this.Timeline.Duration.TotalSeconds < this.Player.MediaElement.Duration.TotalSeconds)
                {
                    this.SetDuration(this.Player.MediaElement.Duration);
                }

                this.UpdateAvailableTime(false);
                TimeCode timeCode = TimeCode.FromSeconds(this.Player.Position.TotalSeconds, this.VideoAsset.FrameRate);

                this.CurrentPositionLabel.Text = timeCode.ToString() + DurationSeparator;

                // if (timeCode > this.Timeline.StartOffset)
                // {
                //    timeCode = timeCode - this.Timeline.StartOffset;
                // }
                this.Timeline.SetPlayHeadPosition(timeCode);
            }
        }

        private void UpdateAvailableTime(bool processVod)
        {
            if (this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Playing || this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Paused || this.Player.MediaElement.CurrentState == SmoothStreamingMediaElementState.Opening)
            {
                if (this.VideoAsset.ResourceType == ResourceType.LiveSmoothStream && this.Player.IsLive)
                {
                    double liveBuffer = (double)(this.Player.MediaElement.LiveBufferSize / 1000);

                    TimeSpan position = TimeSpan.FromSeconds(this.Player.MediaElement.LivePosition - liveBuffer);

                    TimeCode timeCode = TimeCode.FromSeconds(position.TotalSeconds, this.VideoAsset.FrameRate);

                    timeCode = this.Timeline.GetPositionWithoutStartOffset(timeCode);

                    this.Timeline.SetAvailableTime(timeCode);
                }
                else if (processVod)
                {
                    this.Timeline.SetAvailableTime(TimeCode.FromSeconds(this.Player.MediaElement.Duration.TotalSeconds, this.VideoAsset.FrameRate));
                }
            }
        }

        /// <summary>
        /// Sets the duration to the timeline and the video asset.
        /// </summary>
        /// <param name="duration">The duration being set.</param>
        private void SetDuration(TimeCode duration)
        {
            this.Timeline.SetDuration(duration);
            this.VideoAsset.Duration = duration;
            this.UpdateVideInOutProperties();
        }

        /// <summary>
        /// Sets the duration to the timeline and the video asset.
        /// </summary>
        /// <param name="duration">The duration being set.</param>
        private void SetDuration(TimeSpan duration)
        {
            TimeCode currentDuration = TimeCode.FromSeconds(duration.TotalSeconds, this.VideoAsset.FrameRate);
            this.SetDuration(currentDuration);
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
                this.VideoAsset.Duration = TimeCode.FromSeconds(this.VideoAsset.Duration.TotalSeconds, e.Data);
                this.VideoAsset.FrameRate = e.Data;
                this.Timeline.SetDuration(TimeCode.FromSeconds(this.Timeline.Duration.TotalSeconds, e.Data));
                this.UpdateVideInOutProperties();
            }
        }

        /// <summary>
        /// Handles the PositionChange event of the Timeline control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Timeline_PositionChange(object sender, PositionChangeEventArgs e)
        {
            if (this.Player.MediaElement != null)
            {
                this.Player.MediaElement.Scrubbing = true;

                if (!this.Player.MediaElement.IsSeeking)
                {
                    this.StartBuffer();
                }

                TimeSpan position = TimeSpan.FromSeconds(e.NewPosition.TotalSeconds);

                if ((this.VideoAsset.ResourceType == ResourceType.LiveSmoothStream && this.Player.IsLive) || this.Player.MediaElement.StartPosition.TotalSeconds > 0)
                {
                    // Prevent Seeking
                    if (position.TotalSeconds < this.Player.MediaElement.StartPosition.TotalSeconds)
                    {
                        position = this.Player.MediaElement.StartPosition;
                        this.Timeline.SetPlayHeadPosition(this.Timeline.GetPositionWithoutStartOffset(TimeCode.FromSeconds(position.TotalSeconds, this.VideoAsset.FrameRate)));
                    }
                }

                if (this.VideoAsset.ResourceType == ResourceType.LiveSmoothStream && this.Player.IsLive && this.Player.MediaElement.LivePosition < position.TotalSeconds)
                {
                    position = TimeSpan.FromSeconds(this.Player.MediaElement.LivePosition);

                    TimeCode timeCode = TimeCode.FromSeconds(position.TotalSeconds, this.VideoAsset.FrameRate);

                    timeCode = this.Timeline.GetPositionWithoutStartOffset(timeCode);

                    this.Timeline.SetPlayHeadPosition(timeCode);
                }

                if (this.Player.IsPlaySubClipMode && (this.InPosition.TotalSeconds > position.TotalSeconds || this.OutPosition.TotalSeconds < position.TotalSeconds))
                {
                    this.Player.IsPlaySubClipMode = false;
                }

                this.Player.Position = position;

                this.CurrentPositionLabel.Text = TimeCode.FromSeconds(position.TotalSeconds, this.VideoAsset.FrameRate) + DurationSeparator;
            }
        }

        /// <summary>
        /// Handles the MovingPlayhead event of the Timeline control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Timeline_MovingPlayHead(object sender, EventArgs e)
        {
            if (this.Player.MediaElement != null)
            {
                this.Player.PauseMedia();
            }
        }

        /// <summary>
        /// Handles the Resized event of the Timeline control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Timeline_Resized(object sender, EventArgs e)
        {
            if (this.IsFullScreen)
            {
                this.Resize();
            }
        }

        /// <summary>
        /// Adjusts the Previews size based on the player size.
        /// </summary>
        private void AdjustPreviews()
        {
            if (this.Player.MediaElement != null)
            {
                // if (this.Timeline.HasMarkIn || this.Timeline.HasMarkOut)
                // {
                //    this.Player.MediaElement.Width = this.VideoGrid.Width / 2;
                // }
                var height = this.Player.MediaElement.Height;

                if (this.Timeline.HasMarkIn)
                {
                    var previewHeight = height - this.MarkInGrid.RowDefinitions[0].Height.Value - this.MarkInGrid.RowDefinitions[2].Height.Value;

                    this.MarkInPreview.MaxHeight = previewHeight;
                }

                if (this.Timeline.HasMarkOut)
                {
                    var previewHeight = height - this.MarkOutGrid.RowDefinitions[0].Height.Value - this.MarkOutGrid.RowDefinitions[2].Height.Value;

                    this.MarkOutPreview.MaxHeight = previewHeight;
                }
            }
        }

        /// <summary>
        /// Called when stop button of <see cref="Player"/> control is clicked.
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
        /// Called when play button of <see cref="Player"/> control is clicked.
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
        /// Resizes and adjusts the previes.
        /// </summary>
        private void Resize()
        {
            this.Scale(new Size(this.VideoGrid.Width, this.VideoGrid.Height));

            this.AdjustPreviews();
        }

        /// <summary>
        /// Removes the current mark out.
        /// </summary>
        private void RemoveMarkIn()
        {
            this.Timeline.RemoveMarkIn();
            this.MarkInGrid.Visibility = Visibility.Collapsed;
            this.MarkInPreview.Source = null;
            this.Resize();
            this.UpdateVideInOutProperties();
        }

        /// <summary>
        /// Removes the current mark out.
        /// </summary>
        private void RemoveMarkOut()
        {
            this.Timeline.RemoveMarkOut();
            this.MarkOutGrid.Visibility = Visibility.Collapsed;
            this.MarkOutPreview.Source = null;
            this.Resize();
            this.UpdateVideInOutProperties();
        }

        /// <summary>
        /// Handles the Click event of the MarkInDeleteButton. Removes the Mark in.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args istance containing the event data.</param>
        private void MarkInDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.RemoveMarkIn();
        }

        /// <summary>
        /// Handles the Click event of the MarkOutDeleteButton. Removes the Mark out.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args istance containing the event data.</param>
        private void MarkOutDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.RemoveMarkOut();
        }

        /// <summary>
        /// Handles the MetadataSelected event. Sets the mark in based on the metadata information.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance of the event data.</param>
        private void MetadataPanel_MetadataSelected(object sender, DataEventArgs<EventData> e)
        {
            if (this.Player.MediaElement != null)
            {
                this.Player.PauseMedia();
                this.RemoveMarkIn();
                this.RemoveMarkOut();
                this.Timeline.ResetZoom();

                if (!this.Player.MediaElement.IsSeeking)
                {
                    this.StartBuffer();
                }

                double? currentOffset = this.configurationService.GetParameterValueAsDouble("MarkInOffsetInSeconds");

                TimeCode timeCode = TimeCode.FromTicks(e.Data.Time.Ticks, this.VideoAsset.FrameRate);

                if (e.Data.IsRelativeTime)
                {
                    timeCode += this.Timeline.StartOffset;
                }

                if (timeCode.TotalSeconds >= currentOffset)
                {
                    timeCode = timeCode.Subtract(TimeCode.FromSeconds(currentOffset.GetValueOrDefault(), this.VideoAsset.FrameRate));
                }

                EventHandler<SeekCompletedEventArgs> markInHandler = null;
                markInHandler = (s, args) =>
                              {
                                  if (args.Success)
                                  {
                                      this.CurrentPositionLabel.Text = timeCode + DurationSeparator;
                                      System.Threading.Thread.Sleep(50);

                                      TimeCode playheadTimecode = this.Timeline.GetPositionWithoutStartOffset(timeCode);

                                      this.Timeline.SetPlayHeadPosition(playheadTimecode);

                                      this.SetMarkIn(timeCode);
                                  }

                                  this.Player.MediaElement.SeekCompleted -= markInHandler;
                              };

                this.Player.MediaElement.SeekCompleted += markInHandler;

                this.Player.Position = TimeSpan.FromSeconds(timeCode.TotalSeconds);
            }
        }

        /// <summary>
        /// Handles the SeekCompleted event of the MediaElement. Stops the buffering animation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void MediaElement_SeekCompleted(object sender, SeekCompletedEventArgs e)
        {
            if (this.Player.MediaElement != null && !this.Player.MediaElement.IsSeeking)
            {
                this.Player.MediaElement.Scrubbing = false;

                this.EndBuffer();
            }
        }

        /// <summary>
        /// Handles the SetSubMarkClipClicked event of the Player. Sets the Mark In / Out.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void Player_SetSubMarkClipClicked(object sender, DataEventArgs<ScrubShiftType> e)
        {
            switch (e.Data)
            {
                case ScrubShiftType.In:
                    this.SetMarkIn(this.Timeline.CurrentPosition);
                break;

                case ScrubShiftType.Out:
                    this.SetMarkOut(this.Timeline.CurrentPosition);
                break;
            }
        }

        /// <summary>
        /// Handles the PlaySubClipClicked event of the Player. Enables the play subclip feature.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void Player_PlaySubClipClicked(object sender, EventArgs e)
        {
            if (this.Player.MediaElement != null && this.Timeline.HasMarkIn)
            {
                this.GoToSubClipPosition(this.InPosition);
            }
            else
            {
                this.Player.IsPlaySubClipMode = false;
            }
        }

        /// <summary>
        /// Handles the MediaErrorExpandedChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void Player_MediaErrorExpandedChanged(object sender, RoutedEventArgs e)
        {
            this.ClearPreviewState();
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

        /// <summary>
        /// Goes to the given subclip position.
        /// </summary>
        /// <param name="position">The Mark In or Mark Out position.</param>
        private void GoToSubClipPosition(TimeCode position)
        {
            TimeCode timeCode = this.Timeline.GetPositionWithoutStartOffset(position);
            this.Timeline.SetPlayHeadPosition(timeCode);
            this.Player.Position = TimeSpan.FromSeconds(position.TotalSeconds);
        }

        /// <summary>
        /// Handles the Clicked event on the Mark In. Goes to the mark in position.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args containing event data.</param>
        private void MarkIn_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (this.Player.MediaElement != null && this.Timeline.HasMarkIn)
            {
                this.GoToSubClipPosition(this.InPosition);
            }
        }

        /// <summary>
        /// Handles the Clicked event on the Mark Out. Goes to the mark out position.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args containing event data.</param>
        private void MarkOut_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (this.Player.MediaElement != null && this.Timeline.HasMarkOut)
            {
                this.GoToSubClipPosition(this.OutPosition);
            }
        }

        private void PlayerGoToTimeCodeClicked(object sender, EventArgs e)
        {
            if (this.GoToPanel.Visibility == Visibility.Collapsed)
            {
                this.GoToPanel.Visibility = Visibility.Visible;
                this.GoToTextBox.Focus();
            }
            else
            {
                this.CloseGoToPanel();
            }
        }

        private void CloseGoToPanel()
        {
            this.GoToPanel.Visibility = Visibility.Collapsed;
            this.GoToTextBox.Text = string.Empty;
            VisualStateManager.GoToState(this.GoToTextBox, "Valid", false);
            ToolTipService.SetToolTip(this.GoToTextBox, null);
            this.Timeline.SubClipPlayheadContentControl.Focus();
        }

        private void GoToTimeCodeClose_Click(object sender, RoutedEventArgs e)
        {
            this.CloseGoToPanel();
        }

        private void GoToTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Player.MediaElement != null)
            {
                if (e.Key == Key.Enter)
                {
                    if (!TimeCode.ValidateSmpte12MTimecode(this.GoToTextBox.Text))
                    {
                        VisualStateManager.GoToState(this.GoToTextBox, "InvalidUnfocused", false);
                        ToolTipService.SetToolTip(this.GoToTextBox, "Invalid timecode");
                        return;
                    }

                    TimeCode position = new TimeCode(this.GoToTextBox.Text, this.VideoAsset.FrameRate);

                    if (position < this.Timeline.StartOffset || position > (this.Timeline.StartOffset + this.Timeline.Duration))
                    {
                        VisualStateManager.GoToState(this.GoToTextBox, "InvalidUnfocused", false);
                        ToolTipService.SetToolTip(this.GoToTextBox, "Invalid timecode");
                        return;
                    }

                    this.CloseGoToPanel();

                    if (!this.Player.MediaElement.IsSeeking)
                    {
                        this.StartBuffer();
                    }

                    TimeCode positionWithoutOffset = this.Timeline.GetPositionWithoutStartOffset(position);

                    this.Player.Position = TimeSpan.FromSeconds(position.TotalSeconds);

                    this.Timeline.SetPlayHeadPosition(positionWithoutOffset);
                }
            }
        }
    }
}
