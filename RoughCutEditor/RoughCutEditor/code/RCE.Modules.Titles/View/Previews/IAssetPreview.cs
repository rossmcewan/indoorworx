// <copyright file="IAssetPreview.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: IAssetPreview.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles
{
    using Infrastructure.Models;

    /// <summary>
    /// Interface for the Title preview.
    /// </summary>
    public interface IAssetPreview
    {
        /// <summary>
        /// Gets or sets the title template.
        /// </summary>
        /// <value>The <see cref="TitleTemplate"/>.</value>
        TitleTemplate TitleTemplate { get; set; }
    }
}
