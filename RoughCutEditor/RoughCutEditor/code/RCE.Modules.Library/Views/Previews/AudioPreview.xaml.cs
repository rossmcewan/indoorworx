// <copyright file="AudioPreview.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: AudioPreview.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Library
{
    using System;
    using System.Windows;
    using Infrastructure;
    using Infrastructure.Models;

    /// <summary>
    /// Preview control for <see cref="AudioAsset"/>.
    /// </summary>
    public partial class AudioPreview : AssetPreview
    {
        /// <summary>
        /// The <see cref="DependencyProperty"/> to have the Asset of the preview.
        /// </summary>
        private static readonly DependencyProperty AssetProperty =
            DependencyProperty.Register("Asset", typeof(Asset), typeof(AudioPreview), null);

        /// <summary>
        /// The <see cref="DependencyProperty"/> to display the Add Asset button of the preview.
        /// </summary>
        private static readonly DependencyProperty DisplayAddAssetButtonProperty =
            DependencyProperty.Register("DisplayAddAssetButton", typeof(bool), typeof(AudioPreview), new PropertyMetadata(false, OnDisplayAddAssetButtonPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioPreview"/> class.
        /// </summary>
        public AudioPreview()
        {
            InitializeComponent();
            this.MouseEnter += (sender, e) => this.ShowInterface.Begin();
            this.MouseLeave += (sender, e) => this.HideInterface.Begin();
            this.Player.StartMediaPlay += this.Player_StartPlay;
            this.Player.MetadataClick += (sender, e) => this.OnPlayerMetadataClick(sender, e);
        }

        /// <summary>
        /// Occurs when metadata click event occures in <see cref="Player"/> control.
        /// </summary>
        public event EventHandler MetadataClick;

        /// <summary>
        /// Occurs when user clicks on Play button of the <see cref="Player"/>.
        /// </summary>
        public event EventHandler Playing;

        /// <summary>
        /// Occurs when user clicks on Stop button of the <see cref="Player"/>.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The <see cref="Asset"/>.</value>
        public override Asset Asset
        {
            get { return this.GetValue(AssetProperty) as Asset; }
            set { this.SetValue(AssetProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the add asset button should be displayed.
        /// </summary>
        /// <value>A true if the button should be displayed;otherwise false.</value>
        public bool DisplayAddAssetButton
        {
            get { return (bool)this.GetValue(DisplayAddAssetButtonProperty); }
            set { this.SetValue(DisplayAddAssetButtonProperty, value); }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            if (this.Player.MediaElement != null)
            {
                this.AssetContainer.Children.Remove(this.Player.MediaElement);
                this.Player.StopMedia();
                this.OnStopping();
            }
        }

        /// <summary>
        /// Scales the current preview control to the specified size.
        /// </summary>
        /// <param name="size">The size to which the preview control is to be scaled.</param>
        public override void Scale(Size size)
        {
            AudioGrid.Width = size.Width;
            AudioGrid.Height = size.Height;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the dependency property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object  associated with the dependecy property.</param>
        /// <param name="e">The event args instance containing the event data.</param>
        private static void OnDisplayAddAssetButtonPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AudioPreview audioPreview = dependencyObject as AudioPreview;

            if (audioPreview != null)
            {
                audioPreview.OnDisplayAddAssetButtonChanged((bool)e.NewValue);
            }
        }

        /// <summary>
        /// Changes the visibility of the add asset button.
        /// </summary>
        /// <param name="value">A value that indicates if the button should be visible or not.</param>
        private void OnDisplayAddAssetButtonChanged(bool value)
        {
            this.AddAsset.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Handles the StartPlay event of the Player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Player_StartPlay(object sender, EventArgs e)
        {
            if (this.Player.MediaElement == null)
            {
                this.OnPlaying();

                this.Player.MediaElement = new CoreSmoothStreamingMediaElement
                {
                    Width = 0,
                    Height = 0,
                    AutoPlay = false
                };

                this.Player.SetSource(this.Asset);
                this.AssetContainer.Children.Add(this.Player.MediaElement);
            }
        }

        /// <summary>
        /// Triggers the <see cref="Playing"/> event.
        /// </summary>
        private void OnPlaying()
        {
            EventHandler playingHandler = this.Playing;
            if (playingHandler != null)
            {
                playingHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Triggers the <see cref="Stopping"/> event.
        /// </summary>
        private void OnStopping()
        {
            EventHandler stoppingHandler = this.Stopping;
            if (stoppingHandler != null)
            {
                stoppingHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [player metadata click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPlayerMetadataClick(object sender, EventArgs e)
        {
            this.OnMetadataClick();
        }

        /// <summary>
        /// Handles the Click event of the AddAsset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddAsset_Click(object sender, RoutedEventArgs e)
        {
            this.OnAddingAsset();
        }

        /// <summary>
        /// Triggers the <see cref="MetadataClick"/> event.
        /// </summary>
        private void OnMetadataClick()
        {
            EventHandler metadataClickHandler = this.MetadataClick;
            if (metadataClickHandler != null)
            {
                metadataClickHandler(this, EventArgs.Empty);
            }
        }
    }
}
