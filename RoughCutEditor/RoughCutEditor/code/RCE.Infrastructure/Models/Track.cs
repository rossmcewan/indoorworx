// <copyright file="Track.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Track.cs                     
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
    using System.Linq;

    /// <summary>
    /// Collection of elemetns in the timeline(Visual/Audio/Title).
    /// </summary>
    public class Track : Audit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        public Track()
        {
            this.Id = Guid.NewGuid();
            this.Shots = new List<TimelineElement>();
            this.Created = DateTime.Now;
            this.Modified = DateTime.Now;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The unique identifier for the <see cref="Track"/>.</value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the shots.
        /// </summary>
        /// <value>The collection of <see cref="TimelineElement"/>.</value>
        public List<TimelineElement> Shots { get; private set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        /// <value>The track number.</value>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the type of the track.
        /// </summary>
        /// <value>The type of the track.</value>
        public TrackType TrackType { get; set; }

        /// <summary>
        /// Gets or sets the provider URI.
        /// </summary>
        /// <value>The provider URI.</value>
        public Uri ProviderUri { get; set; }

        /// <summary>
        /// Gets the copy of the shots in <see cref="Track"/> which can use used 
        /// for ctrl + z functionality.
        /// </summary>
        /// <returns>The list of <see cref="TimelineElement"/>.</returns>
        public IList<TimelineElement> GetMemento()
        {
            IList<TimelineElement> memento = new List<TimelineElement>();

            foreach (TimelineElement element in this.Shots)
            {
                memento.Add(element.GetMemento());
            }

            return memento;
        }

        /// <summary>
        /// Sets the memento.
        /// </summary>
        /// <param name="memento">The list of <see cref="TimelineElement"/>.</param>
        public void SetMemento(IList<TimelineElement> memento)
        {
            foreach (TimelineElement element in this.Shots)
            {
                TimelineElement mementoElement = memento.Where(x => x.Id == element.Id).FirstOrDefault();

                if (mementoElement != null)
                {
                    element.SetMemento(mementoElement);
                }
            }
        }
    }
}