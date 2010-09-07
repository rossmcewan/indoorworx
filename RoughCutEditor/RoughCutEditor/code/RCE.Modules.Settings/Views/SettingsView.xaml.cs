// <copyright file="SettingsView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SettingsView.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Settings
{
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    
    /// <summary>
    /// Provides the implementation for <see cref="ISettingsView"/>.
    /// </summary>
    public partial class SettingsView : UserControl, ISettingsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsView"/> class.
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();

            // Key Commands
            if (Application.Current.RootVisual != null)
            {
                Application.Current.RootVisual.KeyUp += this.RootVisual_KeyUp;
            }

            HtmlPage.RegisterScriptableObject("Settings", this);
        }

        /// <summary>
        /// Gets or sets the <see cref="ISettingsViewPresentationModel"/> presentation model of the view.
        /// </summary>
        /// <value>A <see also="ISettingsViewPresentatinModel"/> that represents the presentation model of the view.</value>
        public ISettingsViewPresentationModel Model
        {
            get
            {
               return this.DataContext as ISettingsViewPresentationModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// An scriptable method that provides an entry point to save te current project. 
        /// </summary>
        [ScriptableMember]
        public void SaveProject()
        {
            this.Model.SaveProject();
        }

        /// <summary>
        /// Handles the KeyUp event of the RootVisual control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void RootVisual_KeyUp(object sender, KeyEventArgs e)
        {
            // Activate the view if Ctrl + 7 is pressed.
            if (e.Key == System.Windows.Input.Key.D7 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.Model.Activate();
            }
        }
    }
}
