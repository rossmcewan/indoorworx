// <copyright file="VideoAsset.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: VideoAsset.cs                     
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
    using Services.Contracts;
    using SMPTETimecode;
    using System.Collections.Generic;

    /// <summary>
    /// A class that represents a video asset.
    /// </summary>
    public class VideoAsset : Asset
    {
        /// <summary>
        /// The video asset duration.
        /// </summary>
        private TimeCode duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoAsset"/> class.
        /// </summary>
        public VideoAsset()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoAsset"/> class.
        /// </summary>
        /// <param name="id">The id of the asset.</param>
        public VideoAsset(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the duration of the Video.
        /// </summary>
        /// <value>The duration of the video.</value>
        public virtual TimeCode Duration
        {
            get
            {
                return this.duration;
            }

            set 
            { 
                this.duration = value;
                this.OnPropertyChanged("Duration");
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SmpteFrameRate"/> of the Video.
        /// </summary>
        /// <value>The frame rate of the video.</value>
        public virtual SmpteFrameRate FrameRate { get; set; }

        /// <summary>
        /// Gets or sets the height of the Video.
        /// </summary>
        /// <value>The height of the video.</value>
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the Video.
        /// </summary>
        /// <value>The width of the video.</value>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the source of the Video Thumbnail.
        /// </summary>
        /// <value>The source of the thumbnail.</value>
        public string ThumbnailSource { get; set; }

        //private List<Telemetry> baseTelemetryData = new List<Telemetry>()
        //        {
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(0), Watts = 0 },
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(1), Watts = 303},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(2), Watts = 331},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(3), Watts = 282},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(4), Watts = 256},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(5), Watts = 257},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(6), Watts = 258},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(7), Watts = 232},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(8), Watts = 224},
        //            new Telemetry() { TimePosition = TimeSpan.FromMinutes(9), Watts = 224}
        //        };

        private ICollection<Telemetry> baseTelemetry;
        public ICollection<Telemetry> BaseTelemetry
        {
            get { return this.baseTelemetry; }
            set { this.baseTelemetry = value; }
        }

        private ICollection<Telemetry> telemetry = new List<Telemetry>();
        public ICollection<Telemetry> Telemetry
        {
            set
            {
                this.telemetry = value;
                OnPropertyChanged("Telemetry");
            }
            get
            {
                return this.telemetry;
            }
        }

        public void UpdateTelemetry(TimeCode inPosition, TimeCode outPosition)
        {
            this.Telemetry = this.baseTelemetry.Where(x => x.TimePosition.TotalSeconds >= inPosition.TotalSeconds && x.TimePosition.TotalSeconds <= outPosition.TotalSeconds).ToList();
        }
        
        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns> A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.</returns>
        public override string ToString()
        {
            return "Video";
        }
    }
}