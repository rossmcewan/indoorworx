// <copyright file="TitlesView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TitlesView.xaml.cs                     
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Infrastructure.Models;

    /// <summary>
    /// The view for the Titles module.
    /// </summary>
    public partial class TitlesView : UserControl, ITitlesView
    {
        /// <summary>
        /// Indicates when the popup used for drag and drop is captured.
        /// </summary>
        private bool popupCaptured;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitlesView"/> class.
        /// </summary>
        public TitlesView()
        {
            InitializeComponent();

            // Key Commands
            if (Application.Current.RootVisual != null)
            {
                Application.Current.RootVisual.KeyUp += this.RootVisual_KeyUp;
            }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The <see cref="ITitlesViewPresentationModel"/>.</value>
        public ITitlesViewPresentationModel Model
        {
            get
            {
                return this.DataContext as ITitlesViewPresentationModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Handles the key events related to the title view.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="KeyEventArgs"/> used to determine the key pressed.</param>
        private void RootVisual_KeyUp(object sender, KeyEventArgs e)
        {
            // Activate the view if Ctrl + 4 is pressed.
            if (e.Key == System.Windows.Input.Key.D4 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.Model.Activate();
            }

            if (this.Model.IsActive)
            {
                switch (e.Key)
                {
                    case Key.A:
                        if (e.OriginalSource is ListBoxItem)
                        {
                            this.AddTitleToTimeline();
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Notifies the model that a new title should be added to the timeline.
        /// </summary>
        private void AddTitleToTimeline()
        {
            TitleTemplate selectedTitleTemplate = this.TitleList.SelectedItem as TitleTemplate;

            if (selectedTitleTemplate != null)
            {
                this.Model.AddTitleAssetToTimeline(selectedTitleTemplate);
            }
        }
    }
}
