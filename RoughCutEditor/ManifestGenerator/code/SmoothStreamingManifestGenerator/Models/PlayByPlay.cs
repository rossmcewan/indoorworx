// <copyright file="PlayByPlay.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayByPlay.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace SmoothStreamingManifestGenerator.Models
{
    using System;

    public class PlayByPlay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayByPlay"/> class.
        /// </summary>
        public PlayByPlay(Guid id, string text, long time)
        {
            this.ID = id;
            this.Text = text;
            this.Time = time;
        }

        /// <summary>
        /// Gets the PlayByPlay id.
        /// </summary>
        /// <value>The unique identifier for the PlayByPlay.</value>
        public Guid ID { get; private set; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The PlayByPlay text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the position of the ad opportunity.
        /// </summary>
        /// <value>The absolute time in ticks.</value>
        public long Time { get; private set; }
    }
}