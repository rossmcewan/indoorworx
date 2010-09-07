// <copyright file="VideoAssetInOut.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: VideoAssetInOut.cs                     
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
    using Services.Contracts;
    using SMPTETimecode;

    /// <summary>
    /// Specifies the In and Out scrub position of the <see cref="VideoAsset"/>.
    /// </summary>
    public class VideoAssetInOut : VideoAsset
    {
        /// <summary>
        /// The in position of the scrubbed <see cref="VideoAsset"/>.
        /// </summary>
        private double inPosition = -1;

        /// <summary>
        /// The out position of the scrubbed <see cref="VideoAsset"/>.
        /// </summary>
        private double outPosition = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoAssetInOut"/> class.
        /// </summary>
        /// <param name="videoAsset">The video asset.</param>
        public VideoAssetInOut(VideoAsset videoAsset)
        {
            this.VideoAsset = videoAsset;
            this.Id = videoAsset.Id;
            this.ProviderUri = videoAsset.ProviderUri;
            this.Title = videoAsset.Title;
            this.Source = videoAsset.Source;
            this.Height = videoAsset.Height;
            this.Width = videoAsset.Width;
            this.ResourceType = videoAsset.ResourceType;
            this.ThumbnailSource = videoAsset.ThumbnailSource;
            this.Metadata = videoAsset.Metadata;
        }

        /// <summary>
        /// Gets the video asset associated with the Video In Out asset.
        /// </summary>
        /// <value>The video asset associated with the Video In Out asset.</value>
        public VideoAsset VideoAsset { get; private set; }

        /// <summary>
        /// Gets or sets the value of InPosition in seconds from the begining of the video.
        /// </summary>
        /// <value>InPosition value in second.</value>
        public double InPosition
        {
            get
            {
                return this.inPosition;
            }

            set
            {
                this.inPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of OutPosition in seconds from the begining of the video.
        /// </summary>
        /// <value>OutPosition value in second.</value>
        public double OutPosition
        {
            get
            {
                return this.outPosition;
            }

            set
            {
                this.outPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the duration of the Video.
        /// </summary>
        /// <value>The duration of the video.</value>
        public override SMPTETimecode.TimeCode Duration
        {
            get
            {
                return this.VideoAsset.Duration;
            }

            set
            {
                this.VideoAsset.Duration = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SmpteFrameRate"/> of the Video.
        /// </summary>
        /// <value>The frame rate of the video.</value>
        public override SmpteFrameRate FrameRate
        {
            get
            {
                return this.VideoAsset.FrameRate;
            }

            set
            {
                this.VideoAsset.FrameRate = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "VideoInOut";
        }
    }
}
