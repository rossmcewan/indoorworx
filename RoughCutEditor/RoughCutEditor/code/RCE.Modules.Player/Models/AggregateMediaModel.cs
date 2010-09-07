// <copyright file="AggregateMediaModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: AggregateMediaModel.cs                     
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
    using System.Linq;
    using System.Windows.Threading;
    using RCE.Infrastructure.Events;
    using RCE.Infrastructure.Models;
    using RCE.Modules.Player.Models;

    /// <summary>
    /// Class which plays a group of assets in serial mode.
    /// </summary>
    public class AggregateMediaModel : IAggregateMediaModel
    {
        /// <summary>
        /// The <see cref="DispatcherTimer"/> to change the raise the <see cref="PositionUpdated"/> event
        /// and swith to diffrent <see cref="MediaData"/> when current media ends.
        /// </summary>
        private readonly DispatcherTimer timer;

        /// <summary>
        /// List of all the media datas.
        /// </summary>
        private readonly List<MediaData> mediaData;

        /// <summary>
        /// Current playing media.
        /// </summary>
        private IList<int> currentMedia;
        
        // private int currentMedia;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateMediaModel"/> class.
        /// </summary>
        public AggregateMediaModel()
        {
            this.mediaData = new List<MediaData>();
            this.timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 25) };
            this.timer.Tick += this.OnFrameRendered;
            this.timer.Start();
            this.currentMedia = new List<int>();
        }

        /// <summary>
        /// Occurs when [position updated].
        /// </summary>
        public event EventHandler<PositionPayloadEventArgs> PositionUpdated;

        /// <summary>
        /// Occurs when [finished playing].
        /// </summary>
        public event EventHandler FinishedPlaying;

        /// <summary>
        /// Occurs when [buffer start].
        /// </summary>
        public event EventHandler BufferStart;

        /// <summary>
        /// Occurs when [buffer end].
        /// </summary>
        public event EventHandler BufferEnd;

        /// <summary>
        /// Occurs when [download start].
        /// </summary>
        public event EventHandler<AssetDownloadProgressEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration
        {
            get
            {
                if (this.mediaData.Count == 0)
                {
                    return new TimeSpan(0);
                }

                TimeSpan time = TimeSpan.FromSeconds(0);

                for (int i = 0; i < this.mediaData.Count; i++)
                {
                    MediaData media = this.mediaData[i];
                    time = time.Add(media.Out.Subtract(media.In));
                }

                return time;
            }
        }

        /// <summary>
        /// Gets the current asset.
        /// </summary>
        /// <value>The current asset.</value>
        public Asset CurrentAsset
        {
            get
            {
                if (this.currentMedia.Count > 0 && this.mediaData.Any())
                {
                    return this.mediaData[this.currentMedia[0]].TimelineElement.Asset;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public TimeSpan Position
        {
            get
            {
                if (this.mediaData.Count == 0)
                {
                    return new TimeSpan(0);
                }

                TimeSpan currentTime = TimeSpan.FromSeconds(0);

                foreach (int i in this.currentMedia)
                {
                    MediaData m = this.mediaData[i];

                    double position;

                    if (m is BlankMediaData)
                    {
                        position = m.TimelineElement.Position.TotalSeconds - m.Out.TotalSeconds + m.Position.TotalSeconds;
                    }
                    else
                    {
                        position = m.TimelineElement.Position.TotalSeconds + m.Position.TotalSeconds;
                    }

                    TimeSpan time = TimeSpan.FromSeconds(position - m.In.TotalSeconds);

                    if (time > currentTime)
                    {
                        currentTime = time;
                    }
                }

                return currentTime;
            }

            set
            {
                // determine current media
                if (this.mediaData.Count == 0)
                {
                    return;
                }

                IList<int> lastCurrentMedia = this.currentMedia;
                this.currentMedia = this.NextCurrentMedia(value);

                double position = 0;

                if (this.currentMedia.Count > 0)
                {
                    for (int i = 0; i < this.currentMedia.Count; i++)
                    {
                        MediaData m = this.mediaData[this.currentMedia[i]];

                        if (m is BlankMediaData)
                        {
                            position = TimeSpan.FromSeconds(m.TimelineElement.Position.TotalSeconds).TotalSeconds - (m.Out.TotalSeconds - m.In.TotalSeconds) + m.In.TotalSeconds;
                        }
                        else
                        {
                            position = m.TimelineElement.Position.TotalSeconds - m.In.TotalSeconds;
                        }

                        if (lastCurrentMedia.Count == this.currentMedia.Count && lastCurrentMedia.Except(this.currentMedia).Count() == 0)
                        {
                            if (!m.CurrentlyShowing)
                            {
                                m.Show();
                            }

                            m.Position = value.Subtract(TimeSpan.FromSeconds(position));
                        }
                    }
                }
                else
                {
                    foreach (int i in lastCurrentMedia)
                    {
                        if (i > 0 && i < this.mediaData.Count)
                        {
                            this.mediaData[i].Hide();
                        }
                    }

                    return;
                }

                if (lastCurrentMedia.Count != this.currentMedia.Count || lastCurrentMedia.Except(this.currentMedia).Count() > 0)
                {
                    bool isMuted = false;
                    for (int i = 0; i < lastCurrentMedia.Count; i++)
                    {
                        if (lastCurrentMedia[i] > 0 && lastCurrentMedia[i] < this.mediaData.Count)
                        {
                            this.mediaData[lastCurrentMedia[i]].Hide();
                            isMuted = this.mediaData[lastCurrentMedia[i]].IsMuted;
                        }
                    }

                    for (int i = 0; i < this.currentMedia.Count; i++)
                    {
                        this.mediaData[this.currentMedia[i]].IsMuted = isMuted;
                        this.mediaData[this.currentMedia[i]].Show();
                        this.mediaData[this.currentMedia[i]].Position = value.Subtract(TimeSpan.FromSeconds(position));

                        if (this.IsPlaying)
                        {
                            this.mediaData[this.currentMedia[i]].Play();
                        }
                    }
                }

                // if (FrameRendered != null) FrameRendered(this, null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        /// It should be <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Sets a value indicating whether media model is muted.
        /// </summary>
        /// <value>True if model is in muted state; otherwise false.</value>
        public bool Mute
        { 
            set
            {
                if (!this.currentMedia.Any() || !this.mediaData.Any())
                {
                    return;
                }

                foreach (int i in this.currentMedia)
                {
                    this.mediaData[i].IsMuted = value;
                }
            }
        }

        /// <summary>
        /// Sets a value indicating whether the current <see cref="MediaData"/> is visible.
        /// </summary>
        /// <value>It should be true if the current <see cref="MediaData"/> is to shown; otherwise false.</value>
        public bool IsVisible
        {
            set
            {
                if (!this.currentMedia.Any() || !this.mediaData.Any())
                {
                    return;
                }

                foreach (int i in this.currentMedia)
                {
                    if (value)
                    {
                        this.mediaData[i].Show();
                    }
                    else
                    {
                        this.mediaData[i].Hide();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the media data.
        /// </summary>
        /// <value>The media data.</value>
        protected IList<MediaData> MediaData
        {
            get { return this.mediaData; }
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        public void Play()
        {
            if (this.IsPlaying)
            {
                return;
            }

            if (this.currentMedia.Count == 0 || this.mediaData.Count == 0)
            {
                return;
            }

            if (!this.timer.IsEnabled)
            {
                this.timer.Start();
            }

            this.IsPlaying = true;

            foreach (int i in this.currentMedia)
            {
                this.mediaData[i].Show();
                this.mediaData[i].Play();
            }
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            this.IsPlaying = false;

            if (this.currentMedia.Count == 0 || this.mediaData.Count == 0)
            {
                return;
            }

            foreach (int i in this.currentMedia)
            {
                this.mediaData[i].Pause();

                this.OnPositionUpdated(this.Position);
            }
        }

        public void FastRewind()
        {
            if (this.currentMedia.Count == 0 || this.mediaData.Count == 0)
            {
                return;
            }

            foreach (int i in this.currentMedia)
            {
                this.mediaData[i].FastRewind();
            }
        }

        public void FastForward()
        {
            if (this.currentMedia.Count == 0 || this.mediaData.Count == 0)
            {
                return;
            }

            foreach (int i in this.currentMedia)
            {
                this.mediaData[i].FastForward();
            }
        }

        /// <summary>
        /// Adds the element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        public MediaData AddElement(TimelineElement element)
        {
            MediaData media = null;

            if (element.Asset is VideoAsset || element.Asset is AudioAsset)
            {
                media = new PlayableMediaData(element);
                media.BufferStart += this.Media_BufferStart;
                media.BufferEnd += this.Media_BufferEnd;
                media.DownloadProgressChanged += this.Media_DownloadProgressChanged;
            }
            else if (element.Asset is ImageAsset)
            {
                media = new ImageMediaData(element);
            }
            else if (element.Asset is TitleAsset)
            {
                media = new TitleMediaData(element);
            }

            if (media != null)
            {
                this.mediaData.Add(media);
            }

            return media;
        }

        /// <summary>
        /// Adds the blank.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        public MediaData AddBlank(TimelineElement element)
        {
            MediaData media = new BlankMediaData(element);
            this.mediaData.Add(media);
            return media;
        }

        /// <summary>
        /// Removes the element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        public MediaData RemoveElement(TimelineElement element)
        {
            MediaData media = this.FindMediaByElement(element);

            this.Remove(media);

            return media;
        }

        /// <summary>
        /// Removes the blank element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        public MediaData RemoveBlankElement(TimelineElement element)
        {
            MediaData media = this.FindBlankMediaByElement(element);

            this.Remove(media);
            return media;
        }

        /// <summary>
        /// Reorders the elements and sets it's In/Out position in the timeline model.
        /// </summary>
        /// <param name="elements">The elements.</param>
        public void ReorderElements(IList<TimelineElement> elements)
        {
            int position = 0;
            double endPosition = 0;

            foreach (TimelineElement element in elements)
            {
                MediaData blankMediaData = this.FindBlankMediaByElement(element);
                MediaData media = this.FindMediaByElement(element);

                if (blankMediaData != null && media != null)
                {
                    this.mediaData.Remove(blankMediaData);
                    blankMediaData.Out = TimeSpan.FromSeconds(media.TimelineElement.Position.TotalSeconds - endPosition);
                    this.mediaData.Insert(position, blankMediaData);
                    position++;
                    this.mediaData.Remove(media);
                    this.mediaData.Insert(position, media);
                    position++;
                    endPosition = media.TimelineElement.Position.TotalSeconds + media.TimelineElement.Duration.TotalSeconds;
                }
            }
        }

        /// <summary>
        /// Resets the current.
        /// </summary>
        public void ResetCurrent()
        {
            // this.currentMedia = -1;
        }

        /// <summary>
        /// Finds the <see cref="MediaData"/> by element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        public MediaData FindMediaByElement(TimelineElement element)
        {
            return this.mediaData.Where(x => x.TimelineElement == element && !(x is BlankMediaData)).
                FirstOrDefault();
        }

        /// <summary>
        /// Removes the specified media.
        /// </summary>
        /// <param name="media">The media.</param>
        private void Remove(MediaData media)
        {
            if (media != null)
            {
                this.mediaData.Remove(media);
            }
        }

        /// <summary>
        /// Finds the <see cref="MediaData"/> by element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        private MediaData FindBlankMediaByElement(TimelineElement element)
        {
            return this.mediaData.Where(x => x.TimelineElement == element && x is BlankMediaData).
                FirstOrDefault();
        }

        /// <summary>
        /// Raises the <see cref="PositionUpdated"/> event and sets 
        /// the current mediadata when the current mediadata reaches to end.
        /// the current mediadata when the current mediadata reaches to end.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnFrameRendered(object sender, EventArgs e)
        {
            this.timer.Stop();

            if (!this.IsPlaying)
            {
                return;
            }

            TimeSpan currentPosition = this.Position;

            TimeSpan off = TimeSpan.FromSeconds(0);

            TimeSpan outPosition = TimeSpan.FromSeconds(0);

            for (int i = 0; i < this.currentMedia.Count; i++)
            {
                if (this.currentMedia[i] >= 0 && this.currentMedia[i] < this.mediaData.Count)
                {
                    MediaData m = this.mediaData[this.currentMedia[i]];

                    if (m.Position.TotalSeconds >= m.Out.TotalSeconds || !m.Playing)
                    {
                        off = m.Position.Subtract(m.Out);
                        outPosition = TimeSpan.FromSeconds(m.TimelineElement.Position.TotalSeconds + m.Out.TotalSeconds);

                        if (off.Ticks < 0)
                        {
                            off = TimeSpan.FromSeconds(0);
                        }

                        m.Hide();
                    }
                }
            }

           TimeSpan newPosition = currentPosition + off;

           IList<int> nextCurrentMedia = this.NextCurrentMedia(newPosition);

           if (nextCurrentMedia.Count != this.currentMedia.Count || nextCurrentMedia.Except(this.currentMedia).Count() > 0)
           {
               if (newPosition > this.Position)
               {
                   this.Position = newPosition;
               }

               foreach (int i in this.currentMedia)
               {
                   MediaData m = this.mediaData[i];
                   m.Show();
                   m.Play();
               }
           }

            if (this.currentMedia.Count == 0)
            {
                this.IsPlaying = false;
                this.OnFinishedPlaying();
                this.Position = outPosition;
            }

            this.OnPositionUpdated(this.Position);

            this.timer.Start();

            // if (FrameRendered != null) FrameRendered(this, null);
        }

        /// <summary>
        /// Called when position of the current <see cref="MediaData"/> is updated.
        /// </summary>
        /// <param name="position">The position.</param>
        private void OnPositionUpdated(TimeSpan position)
        {
            EventHandler<PositionPayloadEventArgs> positionUpdatedHandler = this.PositionUpdated;
            if (positionUpdatedHandler != null)
            {
                positionUpdatedHandler(this, new PositionPayloadEventArgs(position));
            }
        }

        /// <summary>
        /// Called when current <see cref="MediaData"/> reahces to the end.
        /// </summary>
        private void OnFinishedPlaying()
        {
            EventHandler finishedPlayingHandler = this.FinishedPlaying;
            
            if (finishedPlayingHandler != null)
            {
                finishedPlayingHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [buffer start].
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void OnBufferStart(object sender)
        {
            EventHandler bufferStart = this.BufferStart;
            if (bufferStart != null)
            {
                bufferStart(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [buffer end].
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void OnBufferEnd(object sender)
        {
            EventHandler bufferEnd = this.BufferEnd;
            if (bufferEnd != null)
            {
                bufferEnd(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the BufferStart event of the <see cref="MediaData"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Media_BufferStart(object sender, EventArgs e)
        {
            this.OnBufferStart(sender);
        }

        /// <summary>
        /// Handles the BufferEnd event of the <see cref="MediaData"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Media_BufferEnd(object sender, EventArgs e)
        {
            this.OnBufferEnd(sender);
        }

        /// <summary>
        /// Called when [download start].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AssetDownloadProgressEventArgs"/> instance containing the event data.</param>
        private void OnDownloadProgressChanged(object sender, AssetDownloadProgressEventArgs e)
        {
            EventHandler<AssetDownloadProgressEventArgs> downloadProgressChanged = this.DownloadProgressChanged;
            if (downloadProgressChanged != null)
            {
                downloadProgressChanged(sender, e);
            }
        }

        /// <summary>
        /// Handles the DownloadProgressChanged event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Media_DownloadProgressChanged(object sender, AssetDownloadProgressEventArgs e)
        {
            this.OnDownloadProgressChanged(sender, e);
        }

        /// <summary>
        /// Returns a list of the next media available.
        /// </summary>
        /// <param name="nextPosition">The position being evaluated.</param>
        /// <returns>The list of media.</returns>
        private IList<int> NextCurrentMedia(TimeSpan nextPosition)
        {
            IList<int> nextCurrentMedia = new List<int>();
            for (int i = 0; i < this.mediaData.Count; i++)
            {
                MediaData m = this.mediaData[i];

                double position;
                if (m is BlankMediaData)
                {
                    position = TimeSpan.FromSeconds(m.TimelineElement.Position.TotalSeconds).TotalSeconds - (m.Out.TotalSeconds - m.In.TotalSeconds);
                }
                else
                {
                    position = m.TimelineElement.Position.TotalSeconds;
                }

                if (position <= nextPosition.TotalSeconds && (position + (m.Out.TotalSeconds - m.In.TotalSeconds)) >= nextPosition.TotalSeconds)
                {
                    nextCurrentMedia.Add(i);
                }
            }

            return nextCurrentMedia;
        }
    }
}