// <copyright file="ITitlesView.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ITitlesView.cs                     
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
    /// <summary>
    /// View of the Titles module.
    /// </summary>
    public interface ITitlesView
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The <see cref="ITitlesViewPresentationModel"/>.</value>
        ITitlesViewPresentationModel Model { get; set; }
    }
}