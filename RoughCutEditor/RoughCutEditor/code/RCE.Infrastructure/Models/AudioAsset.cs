// <copyright file="AudioAsset.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: AudioAsset.cs                     
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

    /// <summary>
    /// A class that represents an audio asset.
    /// </summary>
    public class AudioAsset : Asset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioAsset"/> class.
        /// </summary>
        public AudioAsset()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the duration of the Audio.
        /// </summary>
        /// <value>The duration of the audio.</value>
        public double Duration { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns> A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.</returns>
        public override string ToString()
        {
            return "Audio";
        }
    }
}