// <copyright file="IPlayerViewPresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: IPlayerViewPresenter.cs                     
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
    using System.Windows.Media.Imaging;
    using Infrastructure.DragDrop;
    using Infrastructure.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Models;

    /// <summary>
    /// Interface for the player view presenter.
    /// </summary>
    public interface IPlayerViewPresenter
    {
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The <see cref="IPlayerView"/>.</value>
        IPlayerView View { get; set; }

        /// <summary>
        /// Gets or sets the player mode.
        /// </summary>
        /// <value>The <see cref="PlayerMode"/>.</value>
        PlayerMode PlayerMode { get; set; }

        /// <summary>
        /// Gets the command executed on drop elements.
        /// </summary>
        /// <value>The delegate command used to drop the elements.</value>
        DelegateCommand<DropPayload> DropCommand { get; }

        /// <summary>
        /// Gets the command executed on fast rewind.
        /// </summary>
        /// <value>The delegate command used to start/stop fast rewind.</value>
        DelegateCommand<object> FastRewindCommand { get; }

        /// <summary>
        /// Gets the command executed on fast forward.
        /// </summary>
        /// <value>The delegate command used to start/stop fast forward.</value>
        DelegateCommand<object> FastForwardCommand { get; }

        /// <summary>
        /// Gets the MediaData associated to the visual element at current position.
        /// </summary>
        /// <returns>The MediaData of the current position.</returns>
        MediaData GetMediaDataAtCurrentPosition();

        /// <summary>
        /// Publishes the ThumbnailEvent.
        /// </summary>
        /// <param name="bitmap">The bitmap being published.</param>
        void SetThumbnail(WriteableBitmap bitmap);
    }
}