// <copyright file="SeekCommand.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SeekCommand.cs                     
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

    /// <summary>
    /// Defines a seek command operation.
    /// </summary>
    internal class SeekCommand
    {
        /// <summary>
        /// Gets or sets a value indicating whether the command is seeking or not.
        /// </summary>
        public bool IsSeeking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Play should be called or not.
        /// </summary>
        public bool Play { get; set; }

        /// <summary>
        /// Gets or sets the position of the seek.
        /// </summary>
        public TimeSpan? Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether StartToSeekLive should be called or not.
        /// </summary>
        public bool StartSeekToLive { get; set; }

        /// <summary>
        /// Gets or sets the LastPlaybackRate reported.
        /// </summary>
        public double LastPlaybackRate { get; set; }
    }
}
