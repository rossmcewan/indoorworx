// <copyright file="Shell.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: Shell.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Infrastructure.Models;

    /// <summary>
    /// Provides the implementation for <see cref="IShell"/>.
    /// </summary>
    public partial class Shell : UserControl, IShell
    {
        /// <summary>
        /// The current <see cref="fullScreenMode"/> mode.
        /// </summary>
        private FullScreenMode fullScreenMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shell"/> class.
        /// </summary>
        public Shell()
        {
            InitializeComponent();
            this.fullScreenMode = FullScreenMode.Player;
            Application.Current.Host.Content.FullScreenChanged += this.FullScreen_Changed;
        }

        /// <summary>
        /// Ocurrs when a keyboard command is executed (Esc or F).
        /// </summary>
        public event EventHandler<KeyMappingActionEventArgs> KeyMappingActionEvent;

        /// <summary>
        /// Event to save project.
        /// </summary>
        public event EventHandler SaveProject;

        /// <summary>
        /// Gets or sets the Model associated to the view.
        /// </summary>
        /// <value>The model associated to the view.</value>
        public ShellPresenter Model
        {
            get { return this.DataContext as ShellPresenter; }
            set { this.DataContext = value; }
        }

        /// <summary>
        /// Toggles the full screen selection of the application or the player depending on the <see cref="FullScreenMode"/> mode.
        /// </summary>
        /// <param name="mode">The mode to determine if the toggling of the fullscreen should be done in the application or in the player.</param>
        public void ToggleFullScreen(FullScreenMode mode)
        {
            if (this.fullScreenMode == mode || (!Application.Current.Host.Content.IsFullScreen && this.fullScreenMode != mode))
            {
                Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
            }
            else if (Application.Current.Host.Content.IsFullScreen && this.fullScreenMode != mode)
            {
                if (this.fullScreenMode == FullScreenMode.Application)
                {
                    this.HideRegions();
                }
                else if (this.fullScreenMode == FullScreenMode.Player)
                {
                    this.ShowRegions();
                }
            }
       
            this.fullScreenMode = mode;
        }

        /// <summary>
        /// Hides all the placeholders so the Player can fill the fullscreen.
        /// </summary>
        public void HideRegions()
        {
            ScaleTransform scaleTransform = this.PlayerPlaceholder.RenderTransform as ScaleTransform;

            if (scaleTransform != null)
            {
                scaleTransform.ScaleX = Application.Current.Host.Content.ActualWidth / PlayerPlaceholder.Width;
                scaleTransform.ScaleY = Application.Current.Host.Content.ActualHeight / PlayerPlaceholder.ActualHeight;
                this.PlayerPlaceholder.VerticalAlignment = VerticalAlignment.Top;
                this.TimelinePlaceHolder.Visibility = Visibility.Collapsed;
                this.MetadataViewPlaceholder.Visibility = Visibility.Collapsed;
                this.ExpandButton.Visibility = Visibility.Collapsed;
                this.RootLayout.ColumnDefinitions[0].Width = new GridLength(0);
                this.RootLayout.RowDefinitions[0].Height = new GridLength(0);
            }
        }

        /// <summary>
        /// Shows all the placeholders.
        /// </summary>
        public void ShowRegions()
        {
             ScaleTransform scaleTransform = this.PlayerPlaceholder.RenderTransform as ScaleTransform;

             if (scaleTransform != null)
             {
                 scaleTransform.ScaleX = 1;
                 scaleTransform.ScaleY = 1;
                 this.PlayerPlaceholder.VerticalAlignment = VerticalAlignment.Center;
                 this.RootLayout.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Auto);
                 this.RootLayout.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                 this.TimelinePlaceHolder.Visibility = Visibility.Visible;
                 this.ExpandButton.Visibility = Visibility.Visible;
                 this.MetadataViewPlaceholder.Visibility = Visibility.Visible;
             }
        }

        /// <summary>
        /// Occurs when the expand button is being clicked. Changes the state of the application by showing or hiding the player.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="RoutedEventArgs"/> args.</param>
        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            bool expand = this.PlayerPlaceholder.Visibility == Visibility.Collapsed;

            if (expand)
            {
                VisualStateManager.GoToState(this, "ExpandedFormState", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "CollapsedFormState", true);
            }
        }

        /// <summary>
        /// Occurs when a key is being pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="KeyEventArgs"/> args.</param>
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F9:
                // case Key.Space:
                    this.OnKeyMappingAction(KeyMappingAction.Toggle);
                   
                // e.Handled = true;
                    break;

                case Key.F12:
                    this.OnKeyMappingAction(KeyMappingAction.PlayTimeline);
                    break;
                
                case Key.Escape:
                    this.OnKeyMappingAction(KeyMappingAction.PausePlayer);
                    break;
                
                case Key.S:
                    if (ModifierKeys.Control == Keyboard.Modifiers)
                    {
                        this.OnSaveProject();
                    }

                    break;
            }
        }

        /// <summary>
        /// Ocurrs when the application fullscreen setting change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <seealso cref="EventArgs"/> args.</param>
        private void FullScreen_Changed(object sender, EventArgs e)
        {
            if (this.fullScreenMode == FullScreenMode.Player)
            {
                if (Application.Current.Host.Content.IsFullScreen)
                {
                    this.HideRegions();
                }
                else
                {
                    this.ShowRegions();
                }
            }
            else if (this.fullScreenMode == FullScreenMode.Application)
            {
                this.ShowRegions();
            }
        }
        
        /// <summary>
        /// Raises the <see cref="KeyMappingActionEvent"/> event.
        /// </summary>
        /// <param name="keyAction"><see cref="RCE.Infrastructure.Models.KeyMappingAction"/> value.</param>
        private void OnKeyMappingAction(KeyMappingAction keyAction)
        {
            EventHandler<KeyMappingActionEventArgs> kayActionHandler = this.KeyMappingActionEvent;
            if (kayActionHandler != null)
            {
                kayActionHandler(this, new KeyMappingActionEventArgs(keyAction));
            }
        }

        /// <summary>
        /// Raises the <see cref="SaveProject"/> event.
        /// </summary>
        private void OnSaveProject()
        {
            EventHandler saveHandler = this.SaveProject;
            if (saveHandler != null)
            {
                saveHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handle the Click event of the New button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Uri source = Application.Current.Host.Source;

            string uri = source.AbsoluteUri.Substring(0, source.AbsoluteUri.IndexOf("ClientBin", StringComparison.OrdinalIgnoreCase));

            Uri navigateUri = new Uri(String.Format(CultureInfo.InvariantCulture, uri, "Default.aspx"));

            HtmlPage.Window.Navigate(navigateUri);
        }
    }
}
