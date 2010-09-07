// <copyright file="RefreshElementsEventArgs.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: RefreshElementsEventArgs.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Infrastructure.Events
{
    using System;

    /// <summary>
    /// Payload for <see cref="RefreshElementsEvent"/>.
    /// </summary>
    public class RefreshElementsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshElementsEventArgs"/> class.
        /// </summary>
        /// <param name="refreshedWidth">The new width.</param>
        public RefreshElementsEventArgs(double refreshedWidth)
        {
            this.RefreshedWidth = refreshedWidth;
        }

        /// <summary>
        /// Gets the new width of the element.
        /// </summary>
        /// <value>The new width.</value>
        public double RefreshedWidth { get; private set; }
    }
}