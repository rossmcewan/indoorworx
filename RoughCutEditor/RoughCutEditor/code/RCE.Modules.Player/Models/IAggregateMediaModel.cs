// <copyright file="IAggregateMediaModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: IAggregateMediaModel.cs                     
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
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Models;

    /// <summary>
    /// Interface definition for <see cref="AggregateMediaModel"/>.
    /// </summary>
    public interface IAggregateMediaModel
    {
        /// <summary>
        /// Occurs when [buffer start].
        /// </summary>
        event EventHandler BufferStart;

        /// <summary>
        /// Occurs when [buffer end].
        /// </summary>
        event EventHandler BufferEnd;

        /// <summary>
        /// Occurs when download progress of media change.
        /// </summary>
        event EventHandler<AssetDownloadProgressEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Occurs when media element position changes.
        /// </summary>
        event EventHandler<PositionPayloadEventArgs> PositionUpdated;

        /// <summary>
        /// Occurs when [finished playing].
        /// </summary>
        event EventHandler FinishedPlaying;

        /// <summary>
        /// Gets or sets the position of the media element.
        /// </summary>
        /// <value>The position.</value>
        TimeSpan Position { get; set; }

        /// <summary>
        /// Sets a value indicating whether media model is muted.
        /// </summary>
        /// <value>True if the aggregatemodel is to be in mute state;otherwise false.</value>
        bool Mute { set; }

        /// <summary>
        /// Sets a value indicating whether the cuurent <see cref="MediaData"/> is visible.
        /// </summary>
        /// <value>It should be true if the current <see cref="MediaData"/> is to shown; otherwise false.</value>
        bool IsVisible { set; }

        /// <summary>
        /// Gets the duration of the current model.
        /// </summary>
        /// <value>The duration.</value>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        /// It shold be <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets the current asset which is playing in the player.
        /// </summary>
        /// <value>The <see cref="Asset"/>.</value>
        Asset CurrentAsset { get; }

        /// <summary>
        /// Plays this Aggregate model.
        /// </summary>
        void Play();

        /// <summary>
        /// Pauses this Aggregate model.
        /// </summary>
        void Pause();

        /// <summary>
        /// Adds the element to the aggregate model.
        /// </summary>
        /// <param name="element">The <see cref="TimelineElement"/>.</param>
        /// <returns>The <see cref="MediaData"/> which holds the control for the given element.</returns>
        MediaData AddElement(TimelineElement element);

        /// <summary>
        /// Adds the blank element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/> which holds the control for the given element.</returns>
        MediaData AddBlank(TimelineElement element);

        /// <summary>
        /// Removes the element from the collection..
        /// </summary>
        /// <param name="element">The <see cref="TimelineElement"/>.</param>
        /// <returns>Returns the removed <see cref="MediaData"/>.</returns>
        MediaData RemoveElement(TimelineElement element);

        /// <summary>
        /// Removes the blank element.
        /// </summary>
        /// <param name="element">The <see cref="TimelineElement"/>.</param>
        /// <returns>Returns the removed <see cref="MediaData"/>.</returns>
        MediaData RemoveBlankElement(TimelineElement element);

        /// <summary>
        /// Reorders the elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        void ReorderElements(IList<TimelineElement> elements);

        /// <summary>
        /// Resets the current.
        /// </summary>
        void ResetCurrent();

        /// <summary>
        /// Finds the <see cref="MediaData"/> by element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="MediaData"/>.</returns>
        MediaData FindMediaByElement(TimelineElement element);

        void FastForward();

        void FastRewind();
    }
}