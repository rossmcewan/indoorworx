// <copyright file="INotificationViewPresenter.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: INotificationViewPresenter.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Services.Views
{
    /// <summary>
    /// Defines the interface for the presenter associated with the notification view.
    /// </summary>
    public interface INotificationViewPresenter
    {
        /// <summary>
        /// Gets the <see cref="INotificationView"/> view.
        /// </summary>
        /// <value>A <seealso cref="INotificationView"/> that represents the current view.</value>
        INotificationView View { get; }
    }
}