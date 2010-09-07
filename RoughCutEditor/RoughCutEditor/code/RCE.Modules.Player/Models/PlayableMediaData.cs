// <copyright file="PlayableMediaData.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayableMediaData.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player.Models
{
    using System;
    using System.Windows;
    using Microsoft.Web.Media.SmoothStreaming;
    using RCE.Infrastructure;
    using RCE.Infrastructure.Models;
    using SMPTETimecode;

    /// <summary>
    /// Used to play the video\audio asset in the player.
    /// </summary>
    public class PlayableMediaData : MediaData
    {
        /// <summary>
        /// The <see cref="TimelineElement"/> for the <see cref="PlayableMediaData"/>
        /// to have the position of the mediadata in the timeline.
        /// </summary>
        private readonly TimelineElement timelineElement;

        /// <summary>
        /// To play the asset(Video\Autio).
        /// </summary>
        private readonly CoreSmoothStreamingMediaElement mediaElement;

        /// <summary>
        /// Start position from where <see cref="PlayableMediaData"/> will start playing.
        /// </summary>
        private TimeSpan inPosition;

        /// <summary>
        /// End position from where <see cref="PlayableMediaData"/> will stop playing.
        /// </summary>
        private TimeSpan outPosition;

        /// <summary>
        /// Indicates if the media is buffering.
        /// </summary>
        private bool isBufferStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayableMediaData"/> class.
        /// </summary>
        /// <param name="timelineElement">The timeline element.</param>
        public PlayableMediaData(TimelineElement timelineElement)
        {
            this.timelineElement = timelineElement;
            this.timelineElement.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "InPosition")
                {
                    if (this.mediaElement != null && (this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Paused || this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Playing))
                    {
                        this.In = TimeSpan.FromSeconds(this.timelineElement.InPosition.TotalSeconds);
                    }
                }

                if (e.PropertyName == "OutPosition")
                {
                    this.Out = TimeSpan.FromSeconds(this.timelineElement.OutPosition.TotalSeconds);
                }

                if (e.PropertyName == "Volume")
                {
                    if (this.mediaElement != null)
                    {
                        this.mediaElement.Volume = timelineElement.Volume;
                        this.mediaElement.IsMuted = this.timelineElement.Volume == 0;
                    }
                }
            };

           this.mediaElement = new CoreSmoothStreamingMediaElement
                                    {
                                        Opacity = 0,
                                        Volume = timelineElement.Volume,
                                        IsMuted = this.timelineElement.Volume == 0,
                                        AutoPlay = false
                                    };

           this.mediaElement.CurrentStateChanged += this.MediaElement_CurrentStateChanged;
           this.mediaElement.ManifestReady += this.MediaElement_ManifestReady;
           this.mediaElement.MediaOpened += this.MediaElement_MediaOpened;
           this.mediaElement.DownloadProgressChanged += this.MediaElement_DownloadProgressChanged;
           this.mediaElement.SeekCompleted += this.MediaElement_SeekCompleted;

           VideoAsset videoAsset = timelineElement.Asset as VideoAsset;
           AudioAsset audioAsset = timelineElement.Asset as AudioAsset;

           if (videoAsset != null)
           {
               this.Duration = TimeSpan.FromSeconds(videoAsset.Duration.TotalSeconds);
           }
           else if (audioAsset != null)
           {
               this.Duration = TimeSpan.FromSeconds(audioAsset.Duration);
               this.mediaElement.Width = 0;
               this.mediaElement.Height = 0;
           }

           if (this.timelineElement.Asset.IsAdaptiveAsset)
           {
               this.mediaElement.SmoothStreamingSource = this.timelineElement.Asset.Source;
           }
           else
           {
               this.mediaElement.Source = this.timelineElement.Asset.Source;
           }
            
           this.Out = TimeSpan.FromSeconds(this.timelineElement.OutPosition.TotalSeconds);
        }

        /// <summary>
        /// Gets the control corresponding to the <see cref="PlayableMediaData"/> which
        /// is used to play the <see cref="PlayableMediaData"/>.
        /// </summary>
        /// <value>The user control.</value>
        public override object Media
        {
            get { return this.mediaElement; }
        }

        /// <summary>
        /// Gets the timeline element for the <see cref="PlayableMediaData"/>.
        /// </summary>
        /// <value>The timeline element.</value>
        public override TimelineElement TimelineElement
        {
            get { return this.timelineElement; }
        }

        /// <summary>
        /// Gets the duration of the <see cref="PlayableMediaData"/>.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets or sets start position of the asset of the <see cref="PlayableMediaData"/>.
        /// </summary>
        /// <value>
        /// Start position from where <see cref="PlayableMediaData"/> will start playing.
        /// </value>
        public override TimeSpan In
        {
            get
            {
                return this.inPosition;
            }

            set
            {
                this.Position = value;
                this.inPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the stop position of the asset of the <see cref="PlayableMediaData"/>.
        /// </summary>
        /// <value>
        /// End position from where <see cref="PlayableMediaData"/> will stop playing.
        /// </value>
        public override TimeSpan Out
        {
            get
            {
                return this.outPosition;
            }

            set
            {
                if (this.Duration == value)
                {
                    value -= TimeSpan.FromSeconds(0.0);
                }

                this.outPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="PlayableMediaData"/>.
        /// </summary>
        /// <value>The position.</value>
        public override TimeSpan Position
        {
            get
            {
                if (this.mediaElement.StartPosition.TotalSeconds > 0)
                {
                    if (this.mediaElement.Position.TotalSeconds >= this.mediaElement.StartPosition.TotalSeconds)
                    {
                        return this.mediaElement.Position - this.mediaElement.StartPosition;
                    }

                    if (this.mediaElement.Position.TotalSeconds < this.mediaElement.StartPosition.TotalSeconds)
                    {
                        return this.mediaElement.StartPosition - this.mediaElement.StartPosition;
                    }
                }

                return this.mediaElement.Position;
            }

            set
            {
                this.DoActualSeek(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mediadata is muted.
        /// </summary>
        /// <value>
        /// Value would be <c>true</c> if the <see cref="PlayableMediaData"/> is muted; otherwise, <c>false</c>.
        /// </value>
        public override bool IsMuted 
        {
            get
            {
                return this.mediaElement.IsMuted;
            }

            set
            {
                this.mediaElement.IsMuted = value;
            }
        }

        /// <summary>
        /// Shows this <see cref="MediaData"/>.
        /// </summary>
        public override void Show()
        {
            base.Show();

            if (!(this.timelineElement.Asset is AudioAsset))
            {
                this.mediaElement.Opacity = 1;
            }
        }

        /// <summary>
        /// Plays this <see cref="PlayableMediaData"/>.
        /// </summary>
        public override void Play()
        {
            this.mediaElement.Play();
            this.Playing = true;
        }

        /// <summary>
        /// Pauses this <see cref="PlayableMediaData"/>.
        /// </summary>
        public override void Pause()
        {
            this.mediaElement.Pause();
            this.Playing = false;
        }

        public override void FastRewind()
        {
            this.mediaElement.OnRewind();
        }

        public override void FastForward()
        {
            this.mediaElement.OnFastForward();
        }

        /// <summary>
        /// Stops this <see cref="PlayableMediaData"/>.
        /// </summary>
        public override void Stop()
        {
            this.mediaElement.SetPlaybackRate(1.0);
            this.mediaElement.Stop();
            this.Playing = false;
        }

        /// <summary>
        /// Hides this <see cref="PlayableMediaData"/>.
        /// </summary>
        public override void Hide()
        {
            base.Hide();
            this.mediaElement.Opacity = 0;
        }

        /// <summary>
        /// Handles the DownloadProgressChanged event of the MediaElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MediaElement_DownloadProgressChanged(object sender, RoutedEventArgs e)
        {
            this.OnDownloadProgressChanged(this.mediaElement.DownloadProgress, this.mediaElement.DownloadProgressOffset);
        }

        /// <summary>
        /// Handles the CurrentStateChanged event of the media element.
        /// </summary>
        /// <param name="sender">The <see cref="PlayableMediaData"/>.</param>
        /// <param name="e"><see cref="RoutedEventArgs"/> argument.</param>
        private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Stopped || this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Paused)
            {
                this.Playing = false;
            }

            if (this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Buffering)
            {
                this.isBufferStarted = true;
                this.OnBufferStart();
            }
            else if (this.isBufferStarted)
            {
                this.OnBufferEnd();
                this.isBufferStarted = false;
            }
        }

        /// <summary>
        /// Handles the MediaOpened event of the MediaElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            // HACK: For non-smooth streaming content.
            // this.Position = TimeSpan.FromSeconds(this.timelineElement.InPosition.TotalSeconds);
            // this.Position = TimeSpan.FromSeconds(0);
            this.In = TimeSpan.FromSeconds(this.timelineElement.InPosition.TotalSeconds);

            VideoAsset videoAsset = this.timelineElement.Asset as VideoAsset;
            AudioAsset audioAsset = this.timelineElement.Asset as AudioAsset;

            if (videoAsset != null)
            {
                if (TimeSpan.FromSeconds(videoAsset.Duration.TotalSeconds).TotalSeconds != this.mediaElement.Duration.TotalSeconds)
                {
                    videoAsset.Duration = TimeCode.FromSeconds(this.mediaElement.Duration.TotalSeconds, videoAsset.FrameRate);
                    this.Duration = TimeSpan.FromSeconds(videoAsset.Duration.TotalSeconds + this.timelineElement.InPosition.TotalSeconds);
                }
            }

            if (audioAsset != null)
            {
                if (audioAsset.Duration != this.mediaElement.Duration.TotalSeconds)
                {
                    audioAsset.Duration = this.mediaElement.Duration.TotalSeconds;
                    this.Duration = TimeSpan.FromSeconds(audioAsset.Duration + this.timelineElement.InPosition.TotalSeconds);
                }
            }

            SmoothStreamingVideoAsset smoothStreamingVideoAsset = this.timelineElement.Asset as SmoothStreamingVideoAsset;

            if (smoothStreamingVideoAsset != null && smoothStreamingVideoAsset.StartPosition != this.mediaElement.StartPosition.TotalSeconds)
            {
                smoothStreamingVideoAsset.StartPosition = this.mediaElement.StartPosition.TotalSeconds;
            }

            // if (this.mediaElement.StartPosition.TotalSeconds > this.timelineElement.InPosition.TotalSeconds)
            // {
            //    this.timelineElement.InPosition = TimeCode.FromSeconds(this.mediaElement.StartPosition.TotalSeconds, this.timelineElement.InPosition.FrameRate);
            //    this.timelineElement.OutPosition += TimeCode.FromSeconds(this.mediaElement.StartPosition.TotalSeconds, this.timelineElement.InPosition.FrameRate);
            // }
        }

        /// <summary>
        /// Handles the ManifestDonwloaded event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void MediaElement_ManifestReady(object sender, EventArgs e)
        {
            this.mediaElement.SelectTracks("cameraAngle", "camera1", 0, 3450000);
        }

        /// <summary>
        /// Handles the SeekCompleted event of the MediaElement. Stop seek animation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private void MediaElement_SeekCompleted(object sender, SeekCompletedEventArgs e)
        {
           this.OnBufferEnd();
        }

        /// <summary>
        /// Performes a seek to the given value.
        /// </summary>
        /// <param name="value">The value to seek.</param>
        private void DoActualSeek(TimeSpan value)
        {
            if (this.mediaElement != null && (this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Paused || this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Buffering || this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Playing || this.mediaElement.CurrentState == SmoothStreamingMediaElementState.Opening))
            {
                double livePosition = this.mediaElement.IsLive ? this.mediaElement.LivePosition : this.mediaElement.EndPosition.TotalSeconds;

                double time = Math.Min(livePosition, Math.Max(this.mediaElement.StartPosition.TotalSeconds, this.mediaElement.StartPosition.TotalSeconds + value.TotalSeconds));

                TimeSpan position = TimeSpan.FromSeconds(time);

                // this.mediaElement.Scrubbing = true;
                this.mediaElement.Position = position;

                if (this.mediaElement.IsSeeking)
                {
                    this.OnBufferStart();
                }

                // this.mediaElement.Scrubbing = false;
            }
        }
    }
}
