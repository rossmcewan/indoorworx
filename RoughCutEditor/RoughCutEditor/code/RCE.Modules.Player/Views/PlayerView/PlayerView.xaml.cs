// <copyright file="PlayerView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayerView.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using Microsoft.Web.Media.SmoothStreaming;
    using RCE.Infrastructure;
    using RCE.Infrastructure.Models;
    using RCE.Modules.Player.Models;
    using RCE.Services.Contracts;
    using SMPTETimecode;
    using Comment = RCE.Infrastructure.Models.Comment;
    using Key = System.Windows.Input.Key;

    /// <summary>
    /// The view for the player control.
    /// </summary>
    public partial class PlayerView : UserControl, IPlayerView
    {
        /// <summary>
        /// <see cref="DispatcherTimer"/> to handle the forward/rewind of the media.
        /// </summary>
        private readonly DispatcherTimer timer;

        /// <summary>
        /// <see cref="DispatcherTimer"/> to set the current position of the playing media.
        /// </summary>
        private readonly DispatcherTimer timeCodeTimer;

        /// <summary>
        /// Flag indicating if the curent media is playing.
        /// </summary>
        private bool isPlaying;

        /// <summary>
        /// Flag indicating if the media should be repeated in the player.
        /// </summary>
        private bool loopPlayback;

        /// <summary>
        /// Indicates the visibility of the comments.
        /// </summary>
        private Visibility commentsVisibility;

        /// <summary>
        /// Value(+1/-1) indicating forward/rewind is going on.
        /// </summary>
        private int currentSkipDirection;

        /// <summary>
        /// Current Smpte Frame rate.
        /// </summary>
        private SmpteFrameRate currentSmpteFrameRate;

        /// <summary>
        /// Current key pressed.
        /// </summary>
        private Key? keyPressed;
        
        /// <summary>
        /// Indicates if the Player media element is in buffering mode.
        /// </summary>
        private bool isBuffering;

        /// <summary>
        /// Indicates if the player is muted.
        /// </summary>
        private bool isMuted = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerView"/> class.
        /// </summary>
        public PlayerView()
        {
            InitializeComponent();
            
            HtmlPage.RegisterScriptableObject("Player", this);
            this.timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.timer.Tick += this.Timer_Tick;
            this.timeCodeTimer = new DispatcherTimer { Interval = new TimeSpan(500) };
            this.timeCodeTimer.Tick += this.TimeCodeTimer_Tick;
            this.Player.CurrentStateChanged += this.Player_CurrentStateChanged;
            this.Player.MediaOpened += this.Player_MediaOpened;
            this.Player.ManifestReady += this.Player_ManifestReady;
            this.Loaded += this.PlayerView_Loaded;

            this.commentsVisibility = Visibility.Visible;

            // Key Commands
            if (Application.Current.RootVisual != null)
            {
                Application.Current.RootVisual.KeyDown += this.RootVisual_KeyDown;
            }
        }

        /// <summary>
        /// Occurs when [full screen changed].
        /// </summary>
        public event EventHandler<FullScreenModeEventArgs> FullScreenChanged;

        /// <summary>
        /// Occurs when [play clicked].
        /// </summary>
        public event EventHandler PlayClicked;

        /// <summary>
        /// Occurs when [pause clicked].
        /// </summary>
        public event EventHandler PauseClicked;

        /// <summary>
        /// Occurs when [move to start clicked].
        /// </summary>
        public event EventHandler MoveToStartClicked;

        /// <summary>
        /// Occurs when [move to end clicked].
        /// </summary>
        public event EventHandler MoveToEndClicked;

        /// <summary>
        /// Occurs when [frame rewind started].
        /// </summary>
        public event EventHandler FrameRewindStarted;

        /// <summary>
        /// Occurs when [frame rewind ended].
        /// </summary>
        public event EventHandler FrameRewindEnded;

        /// <summary>
        /// Occurs when [frame forward started].
        /// </summary>
        public event EventHandler FrameForwardStarted;

        /// <summary>
        /// Occurs when [frame forward ended].
        /// </summary>
        public event EventHandler FrameForwardEnded;

        /// <summary>
        /// Occurs when [loop playback clicked].
        /// </summary>
        public event EventHandler LoopPlaybackClicked;

        /// <summary>
        /// Occurs when [add comment clicked].
        /// </summary>
        public event EventHandler AddCommentClicked;

        /// <summary>
        /// Occurs when [metadata clicked].
        /// </summary>
        public event EventHandler MetadataClicked;

        /// <summary>
        /// Handles the mute buton click event.
        /// </summary>
        public event EventHandler MuteClicked;

        /// <summary>
        /// Handles the slow motion click event.
        /// </summary>
        public event EventHandler SlowMotionClicked;

        /// <summary>
        /// Handles the pick thumbnail click event.
        /// </summary>
        public event EventHandler PickThumbnailClicked;

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public IPlayerViewPresenter Model
        {
            get
            {
                return this.DataContext as IPlayerViewPresenter;
            }

            set
            {
                this.DataContext = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the player is muted.
        /// </summary>
        /// <value>True if the player is muted otherwise false.</value>
        public bool IsMuted
        {
            get
            {
               return this.isMuted;
            }

            set
            {
                this.isMuted = value;

                if (this.HasSource)
                {
                    this.Player.IsMuted = this.isMuted;    
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Player"/> has source.
        /// </summary>
        /// <value> It would be 
        /// <c>true</c> if the player has source; otherwise, <c>false</c>.
        /// </value>
        private bool HasSource
        {
            get { return this.Player.Source != null || this.Player.SmoothStreamingSource != null; }
        }

        /// <summary>
        /// Toggles between play/pause.
        /// </summary>
        [ScriptableMember]
        public void TogglePlay()
        {
            if (this.isPlaying)
            {
                this.Pause();
            }
            else
            {
                this.Play();
            }
        }

        /// <summary>
        /// Sets the source of the <see cref="Player"/> in case of Audio/Video asset
        /// and set the <see cref="PreviewImage"/> source in case of image asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void SetSource(Asset asset)
        {
            this.Stop();

            // Set the size of the media element maintaining the aspect ratio.
            VideoAsset videoAsset = asset as VideoAsset;
            
            if (videoAsset != null)
            {
                Size size = this.CalculatePlayerSize(videoAsset.Width.GetValueOrDefault(), videoAsset.Height.GetValueOrDefault());

                this.Player.Width = size.Width;
                this.Player.Height = size.Height;
            }

            if (asset is ImageAsset)
            {
                this.Player.Opacity = 0;
                this.PreviewImage.Source = new BitmapImage(asset.Source);
                this.PreviewImage.Opacity = 1;
            }
            else
            {
                this.PreviewImage.Opacity = 0;

                if (asset.IsAdaptiveAsset)
                {
                    this.Player.SmoothStreamingSource = asset.Source;
                }
                else
                {
                    this.Player.Source = asset.Source;
                }

                this.Player.Opacity = 1;
            }
        }

        /// <summary>
        /// Hides the image control if the Audio/Video asset is selected for playing.
        /// </summary>
        public void HidePreviewImage()
        {
            this.PreviewImage.Source = null;
            this.PreviewImage.Opacity = 0;
        }

        /// <summary>
        /// Sets the current smpte frame rate.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        public void SetCurrentSmpteFrameRate(SmpteFrameRate frameRate)
        {
            this.currentSmpteFrameRate = frameRate;
        }

        /// <summary>
        /// Adds the <see cref="FrameworkElement"/> for the given <see cref="MediaData"/> to the <see cref="PlayerContainerGrid"/>.
        /// </summary>
        /// <param name="mediaData">The media data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void AddElement(MediaData mediaData, int width, int height)
        {
            FrameworkElement element = mediaData.Media as FrameworkElement;

            if (element != null)
            {
                // Get the best fit size maintaining the aspect ratio.
                Size size = this.CalculatePlayerSize(width, height);

                element.Width = size.Width;
                element.Height = size.Height;

                this.PlayerContainerGrid.Children.Add(element);
                Grid.SetRow(element, 0);
                this.UpdateLayout();
            }
        }

        /// <summary>
        /// Removes the <see cref="FrameworkElement"/> corresponding to the given <see cref="MediaData"/>.
        /// </summary>
        /// <param name="mediaData">The media data.</param>
        public void RemoveElement(MediaData mediaData)
        {
            FrameworkElement element = mediaData.Media as FrameworkElement;

            if (element != null && this.PlayerContainerGrid.Children.Contains(element))
            {
                this.PlayerContainerGrid.Children.Remove(element);
            }
        }

        /// <summary>
        /// Resize the player and all other Media/Image elements in the grid
        /// to the  best fit size that maintains the aspect ratio.
        /// </summary>
        /// <param name="selectedAspectRatio">Aspect Ratio enum value.</param>
        public void SetAspectRatio(AspectRatio selectedAspectRatio)
        {
            Size aspectSize = UtilityHelper.GetSelectedAspectRatio(selectedAspectRatio);

            // Change height of the player to maintain the aspect ratio selected in the setting module.
            // Player width would be constant.
            this.Player.Width = this.PlayerContainerGrid.ActualWidth;
            this.Player.Height = this.PlayerContainerGrid.ActualWidth * aspectSize.Height / aspectSize.Width;
            this.CommentViewer.Height = this.Player.Width * aspectSize.Height / aspectSize.Width;

            // Resize all the elements in the grid.
            foreach (FrameworkElement element in this.PlayerContainerGrid.Children)
            {
                if (element is MediaElement || element is Image)
                {
                    aspectSize = this.CalculatePlayerSize(element.Width, element.Height);
                    element.Height = aspectSize.Height;
                    element.Width = aspectSize.Width;
                    element.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Sets the text of the <see cref="Time"/> to the given time.
        /// </summary>
        /// <param name="position">The position.</param>
        public void SetCurrentTime(TimeSpan position)
        {
            this.Time.Text = new TimeCode(position.TotalSeconds, this.currentSmpteFrameRate).ToString();
        }

        /// <summary>
        /// Sets the position of the <see cref="MediaElement"/> to the start of the media.
        /// </summary>
        public void MoveToStart()
        {
            if (this.HasSource)
            {
                this.Player.Position = TimeSpan.FromSeconds(0);
                this.SetCurrentTime(this.Player.Position);
            }
        }

        /// <summary>
        /// Sets the media element to the end of the media.
        /// </summary>
        public void MoveToEnd()
        {
            if (this.HasSource)
            {
                if (this.Player.NaturalDuration.HasTimeSpan)
                {
                    this.Player.Position = this.Player.NaturalDuration.TimeSpan; // .Subtract(new Duration(TimeSpan.FromSeconds(0.1))).TimeSpan;
                    this.SetCurrentTime(this.Player.Position);
                }
            }
        }

        /// <summary>
        /// Starts the rewind forward.
        /// </summary>
        /// <param name="skipDirection">The skip direction.</param>
        public void StartFrameRewindForward(int skipDirection)
        {
            if (this.HasSource)
            {
                this.Pause();
                this.currentSkipDirection = skipDirection;
                this.timer.Start();
            }
        }

        /// <summary>
        /// Ends the rewind forward.
        /// </summary>
        public void EndFrameRewindForward()
        {
            if (this.HasSource)
            {
                this.currentSkipDirection = 0;
                this.timer.Stop();
            }
        }

        /// <summary>
        /// Toggles the play/pause visibility.
        /// </summary>
        /// <param name="playing">If set to <c>true</c> then hide play button and show pause button
        /// otherwise vice versa.</param>
        public void TogglePlayVisibility(bool playing)
        {
            if (playing)
            {
                this.PauseButton.Visibility = Visibility.Visible;
                this.PlayButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.PlayButton.Visibility = Visibility.Visible;
                this.PauseButton.Visibility = Visibility.Collapsed;
            }

            this.keyPressed = null;
        }

        /// <summary>
        /// Toggles the loop playback of the player.
        /// </summary>
        public void ToggleLoopPlayback()
        {
           this.loopPlayback = !this.loopPlayback;
        }

        /// <summary>
        /// Shows the given comment in the comment preview.
        /// </summary>
        /// <param name="comments">The comments.</param>
        public void ShowComments(List<Comment> comments)
        {
            this.CommentViewer.ShowComments(comments);
            this.CommentViewer.Visibility = this.commentsVisibility;
        }

        /// <summary>
        /// Hides the current visible comment.
        /// </summary>
        public void HideComments()
        {
            this.CommentViewer.ShowComments(null);
            this.CommentViewer.Visibility = this.commentsVisibility;
        }

        /// <summary>
        /// Stops this media element.
        /// </summary>
        public void Stop()
        {
            this.Time.Text = "00:00:00:00";
            this.isPlaying = false;
            this.TogglePlayVisibility(this.isPlaying);
            this.Player.Opacity = 0;
            this.SlowMotionButton.IsChecked = false;

            if (this.HasSource)
            {
                this.timeCodeTimer.Stop();
                this.timer.Stop();
                this.Player.Stop();
            }
        }

        /// <summary>
        /// Starts the animation of buffering in the player.
        /// </summary>
        public void StartBuffer()
        {
            this.BufferBar.Visibility = Visibility.Visible;
            this.Spinner.BeginAnimation();
        }

        /// <summary>
        /// End the animation of buffering in the player.
        /// </summary>
        public void EndBuffer()
        {
            this.BufferBar.Visibility = Visibility.Collapsed;
            this.Spinner.StopAnimation();
        }

        /// <summary>
        /// Starts the animation of buffering in the player.
        /// </summary>
        public void StartThumbnailBuffer()
        {
            this.ThumbnailBufferBar.Visibility = Visibility.Visible;
            this.ThumbnailSpinner.BeginAnimation();
        }

        /// <summary>
        /// End the animation of buffering in the player.
        /// </summary>
        public void EndThumbnailBuffer()
        {
            this.ThumbnailBufferBar.Visibility = Visibility.Collapsed;
            this.ThumbnailSpinner.StopAnimation();
        }

        /// <summary>
        /// Pauses the player.
        /// </summary>
        public void PausePlayer()
        {
            if (this.isPlaying)
            {
                this.TogglePlay();
            }
        }

        /// <summary>
        /// Picks a thumbnail of the current media element.
        /// </summary>
        /// <param name="mediaData">The media data o the current element.</param>
        public void PickThumbnail(MediaData mediaData)
        {
            if (mediaData != null)
            {
                SmoothStreamingMediaElement mediaElement = mediaData.Media as SmoothStreamingMediaElement;

                if (mediaElement != null)
                {
                    this.StartThumbnailBuffer();
                    bool thumbnailSeekCompleted = false;
                    TimeSpan currentPosition = mediaElement.Position;
            
                    CoreSmoothStreamingMediaElement thumbnailMediaElement = new CoreSmoothStreamingMediaElement
                                                                          {
                                                                              Width = mediaElement.ActualWidth,
                                                                              Height = mediaElement.ActualHeight,
                                                                              IsMuted = true,
                                                                              AutoPlay = false,
                                                                              Volume = 0,
                                                                          };

                    DispatcherTimer thubmnailTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 5) };
                   
                    thubmnailTimer.Tick += (sender, e) =>
                    {
                        if (thumbnailSeekCompleted)
                        {
                            thumbnailSeekCompleted = false;
                            thubmnailTimer.Stop();
                            WriteableBitmap writeableBitmap = new WriteableBitmap(thumbnailMediaElement, null);

                            // writeableBitmap.Render(mediaElement, null);
                            writeableBitmap.Invalidate();
                            this.Model.SetThumbnail(writeableBitmap);
                            this.PlayerContainerGrid.Children.Remove(thumbnailMediaElement);
                            thumbnailMediaElement = null;
                            thubmnailTimer = null;
                            this.EndThumbnailBuffer();
                        }
                    };

                    thubmnailTimer.Start();

                    this.PlayerContainerGrid.Children.Add(thumbnailMediaElement);
                    
                    thumbnailMediaElement.ManifestReady += (sender, e) => ((CoreSmoothStreamingMediaElement)sender).SelectMaxAvailableBitrateTracks("cameraAngle", "camera1");
                    
                    thumbnailMediaElement.MediaOpened += (sender, e) =>
                                                       {
                                                           ((SmoothStreamingMediaElement)sender).Position = currentPosition;
                                                           ((CoreSmoothStreamingMediaElement)sender).Visibility = Visibility.Collapsed;
                                                       };

                    thumbnailMediaElement.SeekCompleted += (sender, e) => thumbnailSeekCompleted = true;
                    thumbnailMediaElement.SmoothStreamingSource = mediaElement.SmoothStreamingSource;
                }
            }
        }

        /// <summary>
        /// Toggles the motion of the current media element.
        /// </summary>
        /// <param name="mediaData">The media data o the current element.</param>
        public void ToggleSlowMotion(MediaData mediaData)
        {
            bool result = false;

            if (mediaData != null)
            {
                CoreSmoothStreamingMediaElement mediaElement = mediaData.Media as CoreSmoothStreamingMediaElement;

                if (mediaElement != null)
                {
                    result = mediaElement.OnSlowMotion();
                }
            }
            else
            {
                if (this.HasSource)
                {
                   result = this.Player.OnSlowMotion();
                }
            }

            if (!result)
            {
                this.SlowMotionButton.IsChecked = false;
            }
        }

        /// <summary>
        /// Pauses this player media player if it has the source.
        /// </summary>
        private void Pause()
        {
            if (this.HasSource)
            {
                this.timeCodeTimer.Stop();
                this.timer.Stop();
                this.Player.Pause();
                this.isPlaying = false;
                this.TogglePlayVisibility(this.isPlaying);
            }
        }

        /// <summary>
        /// Plays the player media element if it has the source.
        /// </summary>
        private void Play()
        {
            if (this.HasSource)
            {
                this.Player.Play();
                this.timeCodeTimer.Start();
                this.isPlaying = true;
                this.TogglePlayVisibility(this.isPlaying);
                this.Player.IsMuted = this.IsMuted;
            }
        }

        /// <summary>
        /// Handles the Click event of the Play control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.OnPlayClicked();
        }

        /// <summary>
        /// Handles the Click event of the Pause control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            this.OnPauseClicked();
        }

        /// <summary>
        /// Handles the Click event of the MoveToStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MoveToStart_Click(object sender, RoutedEventArgs e)
        {
            this.OnMoveToStartClicked();
        }

        /// <summary>
        /// Handles the Click event of the MoveToEnd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MoveToEnd_Click(object sender, RoutedEventArgs e)
        {
            this.OnMoveToEndClicked();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the FrameRewind control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void FrameRewind_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnFrameRewindStarted();
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the FrameRewind control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void FrameRewind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnFrameRewindEnded();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the FrameForward control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void FrameForward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnFrameForwardStarted();
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the FrameForward control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void FrameForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnFrameForwardEnded();
        }

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// It handles the forward/rewind of the media.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.currentSkipDirection == 0)
            {
                return;
            }

            bool add = this.currentSkipDirection > 0;
            long newSkipDirection = 1;

            TimeCode currentTimeCode = TimeCode.FromTimeSpan(this.Player.Position, this.currentSmpteFrameRate);
            long currentTotalFrames = currentTimeCode.TotalFrames;

            TimeCode frameTimeCode = TimeCode.FromFrames(newSkipDirection, this.currentSmpteFrameRate);

            currentTimeCode = add ? currentTimeCode.Add(frameTimeCode) : currentTimeCode.Subtract(frameTimeCode);
            newSkipDirection++;

            while (currentTimeCode.TotalFrames == currentTotalFrames)
            {
                frameTimeCode = TimeCode.FromFrames(newSkipDirection, this.currentSmpteFrameRate);
                currentTimeCode = add ? currentTimeCode.Add(frameTimeCode) : currentTimeCode.Subtract(frameTimeCode);
                newSkipDirection++;
            }

            this.Player.Position = TimeSpan.FromSeconds(Math.Max(0, currentTimeCode.TotalSeconds));
            this.SetCurrentTime(this.Player.Position);
        }

        /// <summary>
        /// Handles the Tick event of the TimeCodeTimer control.
        /// It sets the the position of the media.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TimeCodeTimer_Tick(object sender, EventArgs e)
        {
            if (this.isPlaying)
            {
                this.SetCurrentTime(this.Player.Position);
            }
        }

        /// <summary>
        /// Handles the Click event of the Comment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            this.OnAddCommentClicked();
        }

        /// <summary>
        /// Handles the Click event of the FullScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
           this.OnFullScreenChanged(FullScreenMode.Player);
        }

        /// <summary>
        /// Handles the Click event of the ApplicationFullScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ApplicationFullScreen_Click(object sender, RoutedEventArgs e)
        {
            this.OnFullScreenChanged(FullScreenMode.Application);
        }

        /// <summary>
        /// Handles the Click event of the MediaRepeat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MediaRepeat_Click(object sender, RoutedEventArgs e)
        {
            this.OnLoopPlaybackClicked();
        }

        /// <summary>
        /// Handles the MediaEnded event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.HasSource)
            {
                if (this.loopPlayback)
                {
                    this.Player.Position = TimeSpan.FromSeconds(0);
                    this.Play();
                }
                else
                {
                    this.Pause();
                }
            }
        }

        /// <summary>
        /// Called when [full screen changed].
        /// </summary>
        /// <param name="mode">The <see cref="FullScreenMode"/>.</param>
        private void OnFullScreenChanged(FullScreenMode mode)
        {
            EventHandler<FullScreenModeEventArgs> fullScreenChangedHandler = this.FullScreenChanged;
            if (fullScreenChangedHandler != null)
            {
                fullScreenChangedHandler(this, new FullScreenModeEventArgs(mode));
            }
        }

        /// <summary>
        /// Called when [play clicked].
        /// </summary>
        private void OnPlayClicked()
        {
            EventHandler playClickedHandler = this.PlayClicked;
            if (playClickedHandler != null)
            {
                playClickedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [pause clicked].
        /// </summary>
        private void OnPauseClicked()
        {
            EventHandler pauseClickedHandler = this.PauseClicked;
            if (pauseClickedHandler != null)
            {
                pauseClickedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [move to start clicked].
        /// </summary>
        private void OnMoveToStartClicked()
        {
            EventHandler moveToStartClickedHandler = this.MoveToStartClicked;
            if (moveToStartClickedHandler != null)
            {
                moveToStartClickedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [move to end clicked].
        /// </summary>
        private void OnMoveToEndClicked()
        {
            EventHandler moveToEndClickedHandler = this.MoveToEndClicked;
            if (moveToEndClickedHandler != null)
            {
                moveToEndClickedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when frame rewind starts.
        /// </summary>
        private void OnFrameRewindStarted()
        {
            EventHandler rewindStartedHandler = this.FrameRewindStarted;
            if (rewindStartedHandler != null)
            {
                rewindStartedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [rewind ended].
        /// </summary>
        private void OnFrameRewindEnded()
        {
            EventHandler rewindEndedHandler = this.FrameRewindEnded;
            if (rewindEndedHandler != null)
            {
                rewindEndedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when FrameForwarding starts.
        /// </summary>
        private void OnFrameForwardStarted()
        {
            EventHandler forwardStartedHandler = this.FrameForwardStarted;
            if (forwardStartedHandler != null)
            {
                forwardStartedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [frame forward ended].
        /// </summary>
        private void OnFrameForwardEnded()
        {
            EventHandler forwardEndedHandler = this.FrameForwardEnded;
            if (forwardEndedHandler != null)
            {
                forwardEndedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [loop playback clicked].
        /// </summary>
        private void OnLoopPlaybackClicked()
        {
            EventHandler loopPlaybackClickedHandler = this.LoopPlaybackClicked;
            if (loopPlaybackClickedHandler != null)
            {
                loopPlaybackClickedHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [add comment clicked].
        /// </summary>
        private void OnAddCommentClicked()
        {
            EventHandler addCommentClicked = this.AddCommentClicked;
            if (addCommentClicked != null)
            {
                this.AddCommentClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [metadata clicked].
        /// </summary>
        private void OnMetadataClicked()
        {
            EventHandler metadataClicked = this.MetadataClicked;
            if (metadataClicked != null)
            {
                this.MetadataClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Triggers the MuteClicked event.
        /// </summary>
        private void OnMuteClicked()
        {
            EventHandler muteClicked = this.MuteClicked;
            if (muteClicked != null)
            {
                this.MuteClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Calculates the best fit size in the player region, maintaining aspect ratio of the asset.
        /// </summary>
        /// <param name="width">Width of the asset.</param>
        /// <param name="height">Height of the asset.</param>
        /// <returns>Best fit size in the player region.</returns>
        private Size CalculatePlayerSize(double width, double height)
        {
            return UtilityHelper.GetBestFitSizeMaintainingAspectRatio(
                                                                    this.PlayerContainerGrid.ActualWidth,
                                                                    this.Player.Height,
                                                                    width,
                                                                    height);
        }

        /// <summary>
        /// Handles the Click event of the Metadata control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Metadata_Click(object sender, RoutedEventArgs e)
        {
            this.OnMetadataClicked();
        }

        /// <summary>
        /// Handles the key down commands.
        /// </summary>
        /// <param name="sender">Sender objects.</param>
        /// <param name="e">KeyEventArgs argument.</param>
        private void RootVisual_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TextBox) && !(e.OriginalSource is ListBoxItem))
            {
                switch (e.Key)
                {
                    case Key.H:
                        if (this.keyPressed == Key.H)
                        {
                            this.OnFrameRewindEnded();
                            this.keyPressed = null;
                        }
                        else
                        {
                            this.OnFrameRewindStarted();
                            this.keyPressed = Key.H;                            
                        }
                        
                        break;
                    case Key.L:
                        if (this.keyPressed == Key.L)
                        {
                            this.OnFrameForwardEnded();
                            this.keyPressed = null;
                        }
                        else
                        {
                            this.OnFrameForwardStarted();
                            this.keyPressed = Key.L;
                        }

                        break;
                    case Key.Home:
                        this.OnMoveToStartClicked();
                        break;
                    case Key.End:
                        this.OnMoveToEndClicked();
                        break;
                    case Key.Q:
                        this.OnLoopPlaybackClicked();
                        break;
                    case Key.C:
                        this.OnAddCommentClicked();
                        break;
                    case Key.F:
                    case Key.Enter:
                        this.OnFullScreenChanged(FullScreenMode.Player);
                        e.Handled = true;
                        break;
                    case Key.M:
                        this.OnMetadataClicked();
                        break;
                    case Key.Z:
                        if (Keyboard.Modifiers == ModifierKeys.None)
                        {
                            this.OnMuteClicked();
                        }

                        break;
                } 
            }
        }

        /// <summary>
        /// Handle the mute button click event.
        /// </summary>
        /// <param name="sender">Sender object(Mute Button).</param>
        /// <param name="e">Event argument.</param>
        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            this.OnMuteClicked();
        }

        /// <summary>
        /// Handles the StateChanged event of the current media element.
        /// Show / Hide the buffer control if it starts/stop buffering.
        /// </summary>
        /// <param name="sender">The <see cref="MediaElement"/>.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>.</param>
        private void Player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.Player.CurrentState == SmoothStreamingMediaElementState.Buffering)
            {
                this.isBuffering = true;
                this.StartBuffer();
            }
            else if (this.isBuffering)
            {
                this.isBuffering = false;
                this.EndBuffer();
            }
        }

        /// <summary>
        /// Handles the MediaOpened event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Play or pause the media.
            if (this.isPlaying)
            {
                this.Player.Play();
            }
            else
            {
                this.Player.Pause();
            }
        }

        /// <summary>
        /// Handles the click of the ShowHideComments button. Toggle the visibility of the comments.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowHideComments_Click(object sender, RoutedEventArgs e)
        {
            this.commentsVisibility = this.commentsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            this.CommentViewer.Visibility = this.commentsVisibility;
        }

        /// <summary>
        /// Handles the Loaded event of the view. Set the default aspect ratio to Wide.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void PlayerView_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetAspectRatio(AspectRatio.Wide);
        }

        /// <summary>
        /// Handles the Thumbnail_Click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Thumbnail_Click(object sender, RoutedEventArgs e)
        {
            this.OnPickThumbnailClicked();
        }

        /// <summary>
        /// Handles the ManifestDonwloaded event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void Player_ManifestReady(object sender, EventArgs e)
        {
            this.Player.SelectTracks("cameraAngle", "camera1", 0, 3450000);
        }

        /// <summary>
        /// Handles the click of the SlowMotion button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void SlowMotion_Click(object sender, RoutedEventArgs e)
        {
            this.OnSlowMotionClicked();
        }

        /// <summary>
        /// Raises the SlowMotionClicked event.
        /// </summary>
        private void OnSlowMotionClicked()
        {
            EventHandler handler = this.SlowMotionClicked;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the PickThumbnailClicked event.
        /// </summary>
        private void OnPickThumbnailClicked()
        {
            EventHandler handler = this.PickThumbnailClicked;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
