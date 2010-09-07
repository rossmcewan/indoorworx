// <copyright file="CoreSmoothStreamingMediaElement.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: CoreSmoothStreamingMediaElement.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using Microsoft.Web.Media.SmoothStreaming;

    /// <summary>
    /// Custom SmoothStresamingMediaElement that expose common functionality.
    /// </summary>
    [CLSCompliant(false)]
    public class CoreSmoothStreamingMediaElement : SmoothStreamingMediaElement
    {
        /// <summary>
        /// The command used to store the seek opportunities.
        /// </summary>
        private readonly SeekCommand seekCommand;

        /// <summary>
        /// Stores the token cookies.
        /// </summary>
        private static CookieContainer cookies;

        /// <summary>
        /// Stores the slow motion playback rates.
        /// </summary>
        private List<double> slowMotionPlaybackRates;

        /// <summary>
        /// Stores the rewind playback rates.
        /// </summary>
        private List<double> rewindPlaybackRates;

        /// <summary>
        /// Stores the fast forward playback rates.
        /// </summary>
        private List<double> fastForwardPlaybackRates;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreSmoothStreamingMediaElement"/> class.
        /// </summary>
        public CoreSmoothStreamingMediaElement()
        {
            this.ConfigPath = "Config.xml";
            this.seekCommand = new SeekCommand();
            
            CookiesChanged += this.CoreSmoothStreamingMediaElement_CookiesChanged;
            this.SeekCompleted += this.CoreSmoothStreamingMediaElement_SeekCompleted;
            this.MediaFailed += this.CoreSmoothStreamingMediaElement_MediaFailed;
        }

        /// <summary>
        /// Occurs when the cookies change.
        /// </summary>
        public static event EventHandler CookiesChanged;

        /// <summary>
        /// Occurs when the playback rate change.
        /// </summary>
        public event RoutedEventHandler PlaybackRateChanged;

        /// <summary>
        /// Gets or sets the <seealso cref="CookieContainer"/>.
        /// </summary>
        /// <value>The cookie container that stores the toke cookies.</value>
        public static CookieContainer Cookies
        {
            get
            {
                if (cookies == null)
                {
                    cookies = new CookieContainer();
                }

                return cookies;
            }

            set
            {
                cookies = value;
                OnCookiesChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the media element is currently seeking or not.
        /// </summary>
        /// <value>A true if the media element is seeking;otherwise false.</value>
        public bool IsSeeking
        {
            get { return this.seekCommand.IsSeeking; }
        }

        /// <summary>
        /// Gets or sets the media element position.
        /// </summary>
        /// <value>The position of the media element.</value>
        public new TimeSpan Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                if (this.seekCommand.IsSeeking)
                {
                    this.seekCommand.Position = value;
                    return;
                }

                if (!PipMode)
                {
                    TimeSpan maxPosition = IsLive ? TimeSpan.FromSeconds(LivePosition) : EndPosition;
                    value = GetPositionInRange(value, StartPosition, maxPosition);
                }

                // workaround, handle case when set position and CurrentState = Paused, 
                // if set IsSeeking = true a SeekCompleted event is never raised, so the
                // SSME control will ignore future Position and Play commands
                if (this.SmoothStreamingSource != null)
                {
                    this.seekCommand.IsSeeking = true;
                }

                // else
                // {
                    // if (CurrentState == SmoothStreamingMediaElementState.Playing)
                    // {
                    //    this.seekCommand.IsSeeking = true;
                    // }
                // }
                base.Position = value;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. 
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.CookieContainer = Cookies;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Plays the media element.
        /// </summary>
        public override void Play()
        {
            // workaround for SSME issue, the SSME can have odd behavior when calling Play 
            // from the SeekComplete event handle when it's currently in the Playing state
            if (CurrentState == SmoothStreamingMediaElementState.Playing)
            {
                return;
            }

            // the SSME control has a problem if call Play when it's currently 
            // seeking, set a flag so can call Play when seeking completes
            if (this.seekCommand.IsSeeking)
            {
                this.seekCommand.Play = true;
            }
            else
            {
                try
                {
                    // workaround for SSME issue, can through null exception when call Play
                    base.Play();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Starts to seek to live.
        /// </summary>
        /// <returns>A true if the seek was started;otherwise false.</returns>
        public new bool StartSeekToLive()
        {
            if (this.seekCommand.IsSeeking)
            {
                this.seekCommand.StartSeekToLive = true;
                return false;
            }

            this.seekCommand.IsSeeking = true;
            return base.StartSeekToLive();
        }

        /// <summary>
        /// Updates the current playback rate. This is used to support Slow Motion.
        /// </summary>
        /// <returns>A true if the operation was succesfull;otherwise false.</returns>
        public bool OnSlowMotion()
        {
            bool playbackRateChanged = false;
            int newPlaybackRateIndex = 0;

            if (this.SmoothStreamingSource != null && (this.CurrentState == SmoothStreamingMediaElementState.Paused || this.CurrentState == SmoothStreamingMediaElementState.Playing || this.CurrentState == SmoothStreamingMediaElementState.Buffering))
            {
                double playbackRate = this.PlaybackRate;

                if (this.slowMotionPlaybackRates == null)
                {
                    this.slowMotionPlaybackRates = new List<double>();
                    IList<double> supportedPlaybackRates = this.SupportedPlaybackRates;

                    for (int i = 0; i < supportedPlaybackRates.Count; i++)
                    {
                        if (supportedPlaybackRates[i] > 0.0 && supportedPlaybackRates[i] < 1.0)
                        {
                            this.slowMotionPlaybackRates.Add(supportedPlaybackRates[i]);
                        }
                    }

                    this.slowMotionPlaybackRates.Add(1.0);
                }

                if (playbackRate <= 0.0 || playbackRate >= 1.0)
                {
                    newPlaybackRateIndex = 0;
                }
                else
                {
                    for (int i = 0; i < this.slowMotionPlaybackRates.Count; i++)
                    {
                        if (this.slowMotionPlaybackRates[i] == playbackRate)
                        {
                            newPlaybackRateIndex = (i + 1) % this.slowMotionPlaybackRates.Count;
                            break;
                        }
                    }
                }

                this.SetPlaybackRate(this.slowMotionPlaybackRates[newPlaybackRateIndex]);

                playbackRateChanged = true;
            }

            return playbackRateChanged;
        }

        /// <summary>
        /// Updates the current playback rate. This is used to support Rewind.
        /// </summary>
        /// <returns>A true if the operation was succesfull;otherwise false.</returns>
        public bool OnRewind()
        {
            bool playbackRateChanged = false;
            int newPlaybackRateIndex = 0;

            if (this.SmoothStreamingSource != null && (this.CurrentState == SmoothStreamingMediaElementState.Paused || this.CurrentState == SmoothStreamingMediaElementState.Playing || this.CurrentState == SmoothStreamingMediaElementState.Buffering))
            {
                double playbackRate = this.PlaybackRate;

                if (this.rewindPlaybackRates == null)
                {
                    this.rewindPlaybackRates = new List<double>();
                    IList<double> supportedPlaybackRates = this.SupportedPlaybackRates;

                    for (int i = 0; i < supportedPlaybackRates.Count; i++)
                    {
                        if (supportedPlaybackRates[i] < 0.0)
                        {
                            this.rewindPlaybackRates.Add(supportedPlaybackRates[i]);
                        }
                    }

                    this.rewindPlaybackRates.Add(1.0);
                }

                if (playbackRate >= 0.0)
                {
                    newPlaybackRateIndex = 0;
                }
                else
                {
                    for (int i = 0; i < this.rewindPlaybackRates.Count; i++)
                    {
                        if (this.rewindPlaybackRates[i] == playbackRate)
                        {
                            newPlaybackRateIndex = (i + 1) % this.rewindPlaybackRates.Count;
                            break;
                        }
                    }
                }

                double newPlayBackRate = this.rewindPlaybackRates[newPlaybackRateIndex];

                this.SetPlaybackRate(newPlayBackRate);

                playbackRateChanged = newPlayBackRate != 1.0;
            }

            return playbackRateChanged;
        }

        /// <summary>
        /// Updates the current playback rate. This is used to support FastForward.
        /// </summary>
        /// <returns>A true if the operation was succesfull;otherwise false.</returns>
        public bool OnFastForward()
        {
            bool playbackRateChanged = false;
            int newPlaybackRateIndex = 0;

            if (this.SmoothStreamingSource != null && (this.CurrentState == SmoothStreamingMediaElementState.Paused || this.CurrentState == SmoothStreamingMediaElementState.Playing || this.CurrentState == SmoothStreamingMediaElementState.Buffering))
            {
                double playbackRate = this.PlaybackRate;

                if (this.fastForwardPlaybackRates == null)
                {
                    this.fastForwardPlaybackRates = new List<double>();
                    IList<double> supportedPlaybackRates = this.SupportedPlaybackRates;

                    for (int i = 0; i < supportedPlaybackRates.Count; i++)
                    {
                        if (supportedPlaybackRates[i] > 1.0)
                        {
                            this.fastForwardPlaybackRates.Add(supportedPlaybackRates[i]);
                        }
                    }

                    this.fastForwardPlaybackRates.Add(1.0);
                }

                if (playbackRate <= 1.0)
                {
                    newPlaybackRateIndex = 0;
                }
                else
                {
                    for (int i = 0; i < this.fastForwardPlaybackRates.Count; i++)
                    {
                        if (this.fastForwardPlaybackRates[i] == playbackRate)
                        {
                            newPlaybackRateIndex = (i + 1) % this.fastForwardPlaybackRates.Count;
                            break;
                        }
                    }
                }

                double newPlayBackRate = this.fastForwardPlaybackRates[newPlaybackRateIndex];

                this.SetPlaybackRate(newPlayBackRate);

                playbackRateChanged = newPlayBackRate != 1.0;
            }

            return playbackRateChanged;
        }

        /// <summary>
        /// Selects the tracks for stream.
        /// </summary>
        /// <param name="key">The key to look for within the custom attributes of the track info.</param>
        /// <param name="value">The wanted value.</param>
        /// <param name="minBitrate">The min bitrate of the selected tracks.</param>
        /// <param name="maxBitrate">The max value of the selected tracks.</param>
        public void SelectTracks(string key, string value, ulong minBitrate, ulong maxBitrate)
        {
            StreamInfo videoStream = this.GetStreamInfoForStreamType("video");

            if (videoStream != null)
            {
                bool attributeAvailable = false;
                IList<TrackInfo> tracks = new List<TrackInfo>();

                foreach (TrackInfo trackInfo in videoStream.AvailableTracks)
                {
                    string keyValue;

                    trackInfo.CustomAttributes.TryGetValue(key, out keyValue);

                    if (!string.IsNullOrEmpty(keyValue) && keyValue.ToUpper(CultureInfo.InvariantCulture) == value.ToUpper(CultureInfo.InvariantCulture))
                    {
                        attributeAvailable = true;
                        if (trackInfo.Bitrate >= minBitrate && trackInfo.Bitrate <= maxBitrate)
                        {
                            tracks.Add(trackInfo);
                        }
                    }
                }

                if (!attributeAvailable)
                {
                    tracks = videoStream.AvailableTracks.Where(x => x.Bitrate >= minBitrate && x.Bitrate <= maxBitrate).ToList();
                }

                this.SelectTracksForStream(videoStream, tracks, true);
            }
        }

        /// <summary>
        /// Selects the max available bitrate tracks of a video stream.
        /// </summary>
        /// <param name="key">The key to look for within the custom attributes of the track info.</param>
        /// <param name="value">The wanted value.</param>
        public void SelectMaxAvailableBitrateTracks(string key, string value)
        {
            StreamInfo videoStream = this.GetStreamInfoForStreamType("video");

            if (videoStream != null)
            {
                ulong maxBitrate = videoStream.AvailableTracks.Max(x => x.Bitrate);

                this.SelectTracks(key, value, maxBitrate, maxBitrate);
            }
        }

        /// <summary>
        /// Ensure that the given position is within the expected range.
        /// </summary>
        /// <param name="position">The position being checked.</param>
        /// <param name="minPosition">The min position.</param>
        /// <param name="maxPosition">The max position.</param>
        /// <returns>The position after the validation.</returns>
        private static TimeSpan GetPositionInRange(TimeSpan position, TimeSpan minPosition, TimeSpan maxPosition)
        {
            if (position < minPosition)
            {
                position = minPosition;
            }

            if (position > maxPosition)
            {
                position = maxPosition;
            }

            return position;
        }

        /// <summary>
        /// Raises the CookieChanged event.
        /// </summary>
        private static void OnCookiesChanged()
        {
            EventHandler handler = CookiesChanged;
            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the SeekCompleted event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void CoreSmoothStreamingMediaElement_SeekCompleted(object sender, SeekCompletedEventArgs e)
        {
            this.seekCommand.IsSeeking = false;

            // see if should play
            if (this.seekCommand.Play)
            {
                this.Play();
                this.seekCommand.Play = false;
            }

            // see if should seek to a new position
            if (this.seekCommand.Position.HasValue)
            {
                this.Position = this.seekCommand.Position.Value;
                this.seekCommand.Position = null;
            }

            // see if playback value changed
            if (this.PlaybackRate != this.seekCommand.LastPlaybackRate)
            {
                // store last value
                this.seekCommand.LastPlaybackRate = this.PlaybackRate;
                this.OnPlaybackRateChanged();
            }

            if (this.seekCommand.StartSeekToLive)
            {
                this.StartSeekToLive();
                this.seekCommand.StartSeekToLive = false;
            }
        }

        private void OnPlaybackRateChanged()
        {
            RoutedEventHandler handler = this.PlaybackRateChanged;
            if (handler != null)
            {
                handler(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Handles the MediaFailed event.
        /// </summary>
        /// <param name="sender">The source of the vent.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void CoreSmoothStreamingMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.seekCommand.IsSeeking = false;
        }

        /// <summary>
        /// Handles the CookiesChanged event. Sets the token cookies into the cookie container.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void CoreSmoothStreamingMediaElement_CookiesChanged(object sender, EventArgs e)
        {
            this.CookieContainer = Cookies;
        }
    }
}