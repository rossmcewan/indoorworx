// <copyright file="SmoothStreamingVideoAsset.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SmoothStreamingVideoAsset.cs                     
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

    /// <summary>
    /// A class that represents a smooth streaming video asset.
    /// </summary>
    public class SmoothStreamingVideoAsset : VideoAsset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmoothStreamingVideoAsset"/> class.
        /// </summary>
        public SmoothStreamingVideoAsset()
        {
            this.DataStreams = new List<string>();
            this.ExternalManifests = new List<Uri>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmoothStreamingVideoAsset"/> class.
        /// </summary>
        /// <param name="id">The id of the asset.</param>
        public SmoothStreamingVideoAsset(Guid id)
            : base(id)
        {
            this.DataStreams = new List<string>();
            this.ExternalManifests = new List<Uri>();
        }

        /// <summary>
        /// Gets or sets the start position of the Video.
        /// </summary>
        /// <value>The start position of the video.</value>
        public double StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the data streams of the Video.
        /// </summary>
        /// <value>The data streams of the video.</value>
        public List<string> DataStreams { get; set; }

        /// <summary>
        /// Gets or sets the external manifests of the Video.
        /// </summary>
        /// <value>The external manifests of the video.</value>
        public List<Uri> ExternalManifests { get; set; }
    }
}
