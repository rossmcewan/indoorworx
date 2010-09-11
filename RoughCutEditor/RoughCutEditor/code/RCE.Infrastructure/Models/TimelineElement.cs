// <copyright file="TimelineElement.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TimelineElement.cs                     
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
    using System.Linq;
    using System.Collections.ObjectModel;
    using SMPTETimecode;
using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Specifies the properties of the elements in the timeline module.
    /// </summary>
    public class TimelineElement : Audit
    {
        /// <summary>
        /// In position of the <see cref="TimelineElement"/> relative to the asset duration.
        /// </summary>
        private TimeCode inPosition;

        /// <summary>
        /// Out position of the <see cref="TimelineElement"/> relative to the asset duration.
        /// </summary>
        private TimeCode outPosition;

        /// <summary>
        /// Volume of the timeline element.
        /// </summary>
        private double volume;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineElement"/> class.
        /// </summary>
        public TimelineElement()
        {
            this.Id = Guid.NewGuid();
            this.Comments = new ObservableCollection<Comment>();
            this.Volume = 0.5;
            this.Created = DateTime.Now;
            this.Modified = DateTime.Now;
        }

        /// <summary>
        /// Gets the unique identifier for the <see cref="TimelineElement"/>.
        /// </summary>
        /// <value>The unique identifier for the <see cref="TimelineElement"/>.</value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The <see cref="Asset"/>.</value>
        public Asset Asset { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The <see cref="TimeCode"/>.</value>
        public TimeCode Position { get; set; }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public ObservableCollection<Comment> Comments { get; private set; }

        /// <summary>
        /// Gets or sets the In position of the <see cref="TimelineElement"/> relative to the asset duration.
        /// </summary>
        /// <value>The in position.</value>
        public TimeCode InPosition
        {
            get
            {
                return this.inPosition;
            }

            set
            {
                this.inPosition = value;
                this.OnPropertyChanged("InPosition");
            }
        }

        /// <summary>
        /// Gets or sets the Out position of the <see cref="TimelineElement"/> relative to the asset duration.
        /// </summary>
        /// <value>The out position.</value>
        public TimeCode OutPosition
        {
            get
            {
                return this.outPosition;
            }

            set
            {
                this.outPosition = value;
                this.OnPropertyChanged("OutPosition");
            }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public TimeCode Duration
        {
            get
            {
                return TimeCode.FromAbsoluteTime(this.OutPosition.TotalSeconds - this.InPosition.TotalSeconds, this.Position.FrameRate);
            }
        }

        /// <summary>
        /// Gets or sets the provider URI.
        /// </summary>
        /// <value>The provider URI.</value>
        public Uri ProviderUri { get; set; }

        // TODO: Replace this with Anchor properties instead of using the values

        /// <summary>
        /// Gets or sets the track anchor URI.
        /// </summary>
        /// <value>The track anchor URI.</value>
        public Uri TrackAnchorUri { get; set; }

        // TODO: Replace this with Anchor properties instead of using the values

        /// <summary>
        /// Gets or sets the source anchor URI.
        /// </summary>
        /// <value>The source anchor URI.</value>
        public Uri SourceAnchorUri { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public double Volume
        {
            get
            {
                return this.volume;
            }

            set
            {
                this.volume = value;
                this.OnPropertyChanged("Volume");
            }
        }

        /// <summary>
        /// Adds the comments.
        /// </summary>
        /// <param name="comments">The comments.</param>
        public void AddComments(ObservableCollection<Comment> comments)
        {
            this.Comments = comments;
        }

        /// <summary>
        /// Gets the memento.
        /// </summary>
        /// <returns>The <see cref="TimelineElement"/>.</returns>
        public TimelineElement GetMemento()
        {
            return new TimelineElement { Id = this.Id, InPosition = this.InPosition, OutPosition = this.OutPosition, Position = this.Position };
        }

        /// <summary>
        /// Sets the memento.
        /// </summary>
        /// <param name="element">The <see cref="TimelineElement"/>.</param>
        public void SetMemento(TimelineElement element)
        {
            this.InPosition = element.inPosition;
            this.OutPosition = element.OutPosition;
            this.Position = element.Position;
        }

        private List<Telemetry> baseTelemetryData = new List<Telemetry>()
                {
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(0), Watts = 0 },
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(1), Watts = 303},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(2), Watts = 331},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(3), Watts = 282},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(4), Watts = 256},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(5), Watts = 257},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(6), Watts = 258},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(7), Watts = 232},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(8), Watts = 224},
                    new Telemetry() { TimePosition = TimeSpan.FromMinutes(9), Watts = 224}
                };

        public void UpdateTelemetryForTimecodes()
        {
            this.telemetry = this.baseTelemetryData.Where(x => x.TimePosition.TotalSeconds >= InPosition.TotalSeconds && x.TimePosition.TotalSeconds <= OutPosition.TotalSeconds).ToList();
            OnPropertyChanged("Telemetry");
        }

        private ICollection<Telemetry> telemetry = new List<Telemetry>();
        public ICollection<Telemetry> Telemetry
        {
            get
            {
                return this.telemetry;
            }
        }
    }
}