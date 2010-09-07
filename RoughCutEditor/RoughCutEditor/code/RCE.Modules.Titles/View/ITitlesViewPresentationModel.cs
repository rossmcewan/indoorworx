// <copyright file="ITitlesViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ITitlesViewPresentationModel.cs                     
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.Models;

    /// <summary>
    /// Presention Model Interface for the Titles view.
    /// </summary>
    public interface ITitlesViewPresentationModel : IHeaderInfoProvider<string>, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the title templates.
        /// </summary>
        /// <value>The <see cref="ObservableCollection{T}"/> of title templates.</value>
        ObservableCollection<TitleTemplate> TitleTemplates { get; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The <see cref="ITitlesView"/>.</value>
        ITitlesView View { get; set; }

        /// <summary>
        /// Gets or sets the main title of the titles.
        /// </summary>
        /// <value>The main title.</value>
        string MainTitle { get; set; }

        /// <summary>
        /// Gets or sets the sub title of the titles.
        /// </summary>
        /// <value>The sub title.</value>
        string SubTitle { get; set; }

        /// <summary>
        /// Gets the header on icon.
        /// </summary>
        /// <value>The header icon on.</value>
        string HeaderIconOn { get; }

        /// <summary>
        /// Gets the header off icon.
        /// </summary>
        /// <value>The header icon off.</value>
        string HeaderIconOff { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>A <c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        bool IsActive { get; }

        /// <summary>
        /// Adds the title asset to timeline.
        /// </summary>
        /// <param name="titleTemplate">The <see cref="TitleTemplate"/>.</param>
        void AddTitleAssetToTimeline(TitleTemplate titleTemplate);

        /// <summary>
        /// Activates this media Titles view.
        /// </summary>
        void Activate();
    }
}