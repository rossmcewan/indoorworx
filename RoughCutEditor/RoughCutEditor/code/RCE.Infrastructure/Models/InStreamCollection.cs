// <copyright file="InStreamCollection.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: InStreamCollection.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Infrastructure.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Web.Media.SmoothStreaming;

    public class InStreamCollection : ObservableCollection<XElement>, IDisposable
    {
        /// <summary>
        /// Contains the list of events.
        /// </summary>
        private readonly Dictionary<Guid, XElement> events;

        /// <summary>
        /// The smooth streaming media element.
        /// </summary>
        private CoreSmoothStreamingMediaElement mediaElement;

        /// <summary>
        /// Indicates whether the collection was disposed or not.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Contains the lists of data streams to parse.
        /// </summary>
        private List<string> dataStreams;

        /// <summary>
        /// Initializes a new instance of the <see cref="InStreamCollection"/> class.
        /// </summary>
        public InStreamCollection()
        {
            this.events = new Dictionary<Guid, XElement>();
            this.dataStreams = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether all data streams should be parsed or not.
        /// </summary>
        /// <value>A true if all data streams should be parsed;otherwise false.</value>
        public bool UseAllDataStreams { get; set; }

        /// <summary>
        /// Gets the list of data streams to parse.
        /// </summary>
        /// <value>The data streams to parse.</value>
        public List<string> DataStreams
        {
            get { return this.dataStreams; }
        }

        /// <summary>
        /// Gets or sets the media ellement associated with the collection.
        /// </summary>
        /// <value>The media element.</value>
        [CLSCompliant(false)]
        public CoreSmoothStreamingMediaElement MediaElement
        {
            get
            {
                return this.mediaElement;
            }

            set
            {
                if (value != null)
                {
                    if (this.mediaElement != null)
                    {
                        this.mediaElement.MediaOpened -= this.MediaElement_MediaOpened;
                        this.mediaElement.TimelineEventReached -= this.MediaElement_TimelineEventReached;
                    }

                    this.mediaElement = value;
                    this.mediaElement.MediaOpened += this.MediaElement_MediaOpened;
                    this.mediaElement.TimelineEventReached += this.MediaElement_TimelineEventReached;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates if Dispose is being called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (this.mediaElement != null)
                {
                    this.mediaElement.MediaOpened -= this.MediaElement_MediaOpened;
                    this.mediaElement.TimelineEventReached -= this.MediaElement_TimelineEventReached;
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// Handles the MediaOpened event of the current <see cref="CoreSmoothStreamingMediaElement"/>.
        /// Parses TimelineEvents of the the Sparse streams.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void MediaElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.mediaElement.AvailableStreams != null)
            {
                foreach (StreamInfo streamInfo in this.mediaElement.AvailableStreams)
                {
                    if (streamInfo.IsSparseStream)
                    {
                        foreach (TrackInfo trackInfo in streamInfo.AvailableTracks)
                        {
                            if (trackInfo.TrackData != null && this.ShouldProcessTrack(trackInfo))
                            {
                                foreach (TimelineEvent timelineEvent in trackInfo.TrackData)
                                {
                                    this.ParseTimelineEvent(timelineEvent);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verify if a track should be processed or not.
        /// </summary>
        /// <param name="track">The track being evaluated.</param>
        /// <returns>A true if the track should be processed;otherwise false.</returns>
        private bool ShouldProcessTrack(TrackInfo track)
        {
            if (this.UseAllDataStreams)
            {
                return true;
            }

            return this.DataStreams.Any(streamName => streamName.ToUpper(CultureInfo.InvariantCulture) == track.ParentStream.Name.ToUpper(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Handles the TimelineEventReached event of the current <see cref="CoreSmoothStreamingMediaElement"/>.
        /// Parses the timeline event just reached.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void MediaElement_TimelineEventReached(object sender, TimelineEventArgs e)
        {
            lock (this)
            {
                if (e.Track.ParentStream.IsSparseStream && this.ShouldProcessTrack(e.Track))
                {
                    this.ParseTimelineEvent(e.Event);
                }
            }
        }

        /// <summary>
        /// Parses the <paramref name="timelineEvent"/>. Extracts the event data.
        /// </summary>
        /// <param name="timelineEvent">The timeline event being parsed.</param>
        private void ParseTimelineEvent(TimelineEvent timelineEvent)
        {
            string eventData = System.Text.Encoding.UTF8.GetString(timelineEvent.EventData, 0, timelineEvent.EventData.Length);

            if (!string.IsNullOrEmpty(eventData))
            {
                XElement element = XElement.Parse(eventData);

                if (element.Name == "InStreamEnvelope")
                {
                    Guid id = element.Attribute("Id").GetValueAsGuid();

                    if (id != Guid.Empty && !this.events.ContainsKey(id))
                    {
                        this.events.Add(id, element);
                        string action = element.Attribute("Action").GetValue().ToUpper(CultureInfo.InvariantCulture);

                        switch (action)
                        {
                            case "ADD":
                                this.AddEvent(element);
                                break;

                            case "REMOVE":
                                this.RemoveEvent(element);
                                break;

                            case "REPLACE":
                                this.RemoveEvent(element);

                                this.AddEvent(element);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the event to the collection.
        /// </summary>
        /// <param name="element">The event being added.</param>
        private void AddEvent(XContainer element)
        {
            foreach (XElement child in element.Elements())
            {
                if (!this.Contains(child))
                {
                    this.Add(child);
                }
            }
        }

        /// <summary>
        /// Remove sthe event from the collection.
        /// </summary>
        /// <param name="element">The event being removed.</param>
        private void RemoveEvent(XElement element)
        {
            int count = this.Count;
            Guid targetId = element.Attribute("TargetID").GetValueAsGuid();

            if (targetId != Guid.Empty)
            {
                XElement elementToRemove;

                if (this.events.TryGetValue(targetId, out elementToRemove))
                {
                    foreach (XElement child in elementToRemove.Elements())
                    {
                        this.Remove(child);
                    }
                }

                if (count == this.Count)
                {
                    foreach (XElement eventData in this.events.Values)
                    {
                        Guid eventId = eventData.Attribute("TargetID").GetValueAsGuid();

                        if (eventId == targetId)
                        {
                            foreach (XElement child in eventData.Elements())
                            {
                                this.Remove(child);
                            }
                        }
                    }
                }
            }
        }
    }
}
