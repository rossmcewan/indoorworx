// <copyright file="IPreview.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: IPreview.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline
{
    /// <summary>
    /// Interface for preview elements(Image/Audio/Video/Title).
    /// </summary>
    public interface IPreview
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        double Height { get; set; }

        /// <summary>
        /// Sets the current element as selected element.
        /// </summary>
        /// <param name="selected">If set to <c>true</c> [selected].</param>
        void SetSelected(bool selected);

        /// <summary>
        /// Shows the download progress bar.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <param name="offset">The offset.</param>
        void ShowDownloadProgressBar(double progress, double offset);
    }
}