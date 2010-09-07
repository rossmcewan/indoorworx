// <copyright file="LibraryView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: LibraryView.xaml.cs                     
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.Models;
    using RCE.Infrastructure.Controls;

    /// <summary>
    /// View for the Asset library.
    /// </summary>
    public partial class LibraryView : UserControl, ILibraryView
    {
        // Todo: Get this value from the config file.

        /// <summary>
        /// Value by which the slider should move.
        /// </summary>
        private const double ShiftScaleMargin = 0.05;

        /// <summary>
        /// Margin in x coordinate for full screen mode.
        /// </summary>
        private const double ListRightOffset = 35;

        /// <summary>
        /// Margin in y coordinate for full screen mode.
        /// </summary>
        private const double ListBottomOffset = 20;

        /// <summary>
        /// Minimum possible asset height of the asset in the list.
        /// </summary>
        private readonly double assetMinHeight;

        /// <summary>
        /// Minimum possible asset width of the asset in the list.
        /// </summary>
        private readonly double assetMinWidth;

        /// <summary>
        /// AspectRatio of the items in the asset listbox.
        /// </summary>
        private readonly double previewAspectRatio;

        /// <summary>
        /// Size of the preview controls.
        /// </summary>
        private readonly Size previewSize = new Size(170, 145);

        /// <summary>
        /// Current selected asset.
        /// </summary>
        private AssetPreview currentAsset;

        /// <summary>
        /// Ticks for the last click of the mouse.
        /// </summary>
        private long lastClickTicks;

        /// <summary>
        /// Size of the listbox items(Varies on slider movement).
        /// </summary>
        private Size currentAssetSize;

        /// <summary>
        /// True is if the control is in list box size change event.
        /// </summary>
        private bool lstSizeChanging;

        /// <summary>
        /// True if the control has been loaded.
        /// </summary>
        private bool isLoaded;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryView"/> class.
        /// </summary>
        public LibraryView()
        {
            InitializeComponent();

            // Initilaize values related to the scaling assets.
            this.assetMinHeight = this.previewSize.Height;
            this.assetMinWidth = this.previewSize.Width;
            this.previewAspectRatio = this.previewSize.Width / this.previewSize.Height;
            this.currentAssetSize = new Size(this.previewSize.Width, this.previewSize.Height);

            // Key Commands
            // Register to MouseWheel event.
            if (Application.Current.RootVisual != null)
            {
                Application.Current.RootVisual.KeyUp += this.RootVisual_KeyUp;
                Application.Current.RootVisual.MouseWheel += this.OnMouseWheel;  
            }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public ILibraryViewPresentationModel Model
        {
            get
            {
                return this.DataContext as ILibraryViewPresentationModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Gets the maximum possible height of the preview.
        /// </summary>
        /// <value>The max height of the preview.</value>
        private double MaxPreviewHeight
        {
            get
            {
                return this.AssetsList.ActualHeight - ListBottomOffset > this.assetMinHeight ? this.AssetsList.ActualHeight - ListBottomOffset : this.assetMinHeight;
            }
        }

        /// <summary>
        /// Gets the maximum possible width of the preview.
        /// </summary>
        /// <value>The max width of the preview.</value>
        private double MaxPreviewWidth
        {
            get
            {
                return this.AssetsList.ActualWidth - ListRightOffset > this.assetMinWidth ? this.AssetsList.ActualWidth - ListRightOffset : this.assetMinWidth;
            }
        }

        /// <summary>
        /// Gets the width of the current preview.
        /// </summary>
        /// <value>The width of the current preview.</value>
        private double CurrentPreviewWidth
        {
            get
            {
                return this.assetMinWidth + ((this.MaxPreviewWidth - this.assetMinWidth) * this.ZoomSlider.Value);
            }
        }

        /// <summary>
        /// Gets the height of the current preview.
        /// </summary>
        /// <value>The height of the current preview.</value>
        private double CurrentPreviewHeight
        {
            get
            {
                return this.assetMinHeight + ((this.MaxPreviewHeight - this.assetMinHeight) * this.ZoomSlider.Value);
            }
        }

        /// <summary>
        /// Gets the maximum possible full screen size of the asset preview.
        /// </summary>
        /// <value>The maximum possible <see cref="Size"/> of the preview.</value>
        private Size GetAssetFullScreenSize
        {
            get
            {
                return new Size(
                    Math.Max(this.AssetsList.ActualWidth - ListRightOffset, this.assetMinWidth),
                    Math.Max(this.AssetsList.ActualHeight - ListBottomOffset, this.assetMinHeight));
            }
        }

        /// <summary>
        /// It addes the metadata fields in the list view of the assets.
        /// </summary>
        /// <param name="metadataFields">Name of the metadata fields.</param>
        public void AddMetadataFields(IList<string> metadataFields)
        {
            foreach (string field in metadataFields)
            {
                if (UtilityHelper.IsMetadataFieldExist(field))
                {
                    DataGridTextColumn column = new DataGridTextColumn { Header = field };

                    Binding binding;
                    if (field == "Duration")
                    {
                        binding = new Binding(field)
                                      {
                                          Converter = new Infrastructure.Converters.DurationConverter()
                                      };
                    }
                    else
                    {
                        binding = new Binding(field.Replace(" ", string.Empty));
                    }

                    column.Binding = binding;
                    DetailsDataGrid.Columns.Add(column);
                }
            }
        }

        /// <summary>
        /// Shows a progress bar.
        /// </summary>
        public void ShowProgressBar()
        {
            this.ProgressBar.Visibility = Visibility.Visible;
            this.Spinner.BeginAnimation();
        }

        /// <summary>
        /// Hides the progress bar.
        /// </summary>
        public void HideProgressBar()
        {
            this.ProgressBar.Visibility = Visibility.Collapsed;
            this.Spinner.StopAnimation();
        }

        /// <summary>
        /// Called when mouse wheel is moved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs args)
        {
            if (this.Model.IsActive)
            {
                Point point = args.GetPosition(this);
                if ((point.X >= 0 && point.X <= this.ActualWidth) &&
                    (point.Y >= 0 && point.Y <= this.ActualHeight))
                {
                    double delta = args.Delta;

                    if (delta > 0)
                    {
                        if (this.ZoomSlider.Value + ShiftScaleMargin <= 1)
                        {
                            this.ZoomSlider.Value += ShiftScaleMargin;
                        }
                        else if (this.ZoomSlider.Value != 1)
                        {
                            this.ZoomSlider.Value = 1;
                        }
                    }
                    else if (delta < 0)
                    {
                        if (this.ZoomSlider.Value - ShiftScaleMargin >= 0)
                        {
                            this.ZoomSlider.Value -= ShiftScaleMargin;
                        }
                        else if (this.ZoomSlider.Value != 0)
                        {
                            this.ZoomSlider.Value = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Playing event of the Asset(Video/Audio) control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Asset_Playing(object sender, EventArgs e)
        {
            if (this.currentAsset != null)
            {
                this.currentAsset.Stop();                
            }

            this.currentAsset = (AssetPreview)sender;
        }

        /// <summary>
        /// Handles the Stopping event of the Asset(Video/Audio) control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Asset_Stopping(object sender, EventArgs e)
        {
            this.currentAsset = null;
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the <see cref="LibraryView"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now.Ticks - this.lastClickTicks) < UtilityHelper.MouseDoubleClickDurationValue)
            {
                Asset asset = this.AssetsList.SelectedItem as Asset;

                if (asset != null)
                {
                    this.Model.OnAssetSelected(asset);    
                }
                
                this.lastClickTicks = 0;
            }

            this.lastClickTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Handles the Checked event of the ThumbButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ThumbButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.AssetsList != null)
            {
                this.AssetsList.Visibility = Visibility.Visible;
            }

            if (this.DetailsDataGrid != null)
            {
                this.DetailsDataGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Handles the Checked event of the ListButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ListButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.AssetsList != null)
            {
                this.AssetsList.Visibility = Visibility.Collapsed;
            }

            if (this.DetailsDataGrid != null)
            {
                this.DetailsDataGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// It sets the scrollbar position so that the current assets can come into the visibile region.
        /// </summary>
        /// <param name="asset">The asset.</param>
        private void SetScrollerOffset(Asset asset)
        {
            int itemIndex = this.AssetsList.Items.IndexOf(asset);
            if (itemIndex >= 0)
            {
                // Get the wrap panel to get the offset of the current element.
                IList<WrapPanel> panels = this.AssetsList.GetChildControls<WrapPanel>();
                if (panels != null && panels.Count > 0)
                {
                    double offset = panels[0].GetOffsetFromTop(itemIndex);
                    IList<ScrollViewer> scrollViewers = this.AssetsList.GetChildControls<ScrollViewer>();
                    if (scrollViewers != null && scrollViewers.Count > 0)
                    {
                        scrollViewers[0].ScrollToVerticalOffset(offset);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.isLoaded)
            {
                Binding addItemCommand = new Binding("AddItemCommand") { Source = this.DataContext };
                ((BindingHelper)this.Resources["AddItemCommand"]).SetBinding(BindingHelper.ValueProperty, addItemCommand);

                Binding playSelectedAssetCommand = new Binding("PlaySelectedAssetCommand") { Source = this.DataContext };
                ((BindingHelper)this.Resources["PlaySelectedAssetCommand"]).SetBinding(BindingHelper.ValueProperty, playSelectedAssetCommand);

                // calculate and set the slider value from the initialized values.
                if (this.AssetsList.ActualWidth - this.assetMinWidth != 0)
                {
                    this.ZoomSlider.Value = (this.currentAssetSize.Width - this.assetMinWidth) / (this.AssetsList.ActualWidth - this.assetMinWidth);
                }

                this.ZoomSlider.ValueChanged += this.Slider_ValueChanged;
                this.AssetsList.SizeChanged += this.AssetsList_SizeChanged;

                this.isLoaded = true;
            }
        }

        /// <summary>
        /// Handles the Add event of the Asset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Asset_Add(object sender, EventArgs e)
        {
            AssetPreview preview = sender as AssetPreview;

            if (preview != null)
            {
                this.Model.OnAddAsset(preview.Asset);
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="double"/> instance containing the event data.</param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!this.lstSizeChanging)
            {
                this.SetPreviewSizeMaintainingAspectRatio();
                this.UpdateZoom(); 
            }
        }

        /// <summary>
        /// Scales all the assest previews of the view according to the current scale value.
        /// </summary>
        private void UpdateZoom()
        {
            if (this.ZoomSlider != null)
            {
                IList<AssetPreview> previews = this.AssetsList.GetChildControls<AssetPreview>();
                
                foreach (AssetPreview preview in previews)
                {
                    VideoPreview videoPreview = preview as VideoPreview;

                    if (videoPreview != null && videoPreview.IsFullScreen)
                    {
                        if (this.lstSizeChanging)
                        {
                            preview.Scale(this.GetAssetFullScreenSize);
                        }
                        else
                        {
                            videoPreview.IsFullScreen = false;
                            preview.Scale(this.currentAssetSize);
                        }
                    }
                    else
                    {
                        preview.Scale(this.currentAssetSize);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the preview size while maintaining aspect ratio.
        /// </summary>
        private void SetPreviewSizeMaintainingAspectRatio()
        {
            if (this.MaxPreviewHeight < this.MaxPreviewWidth)
            {
                this.currentAssetSize.Width = this.CurrentPreviewWidth;
                this.currentAssetSize.Height = this.CurrentPreviewWidth / this.previewAspectRatio > this.MaxPreviewHeight ? this.MaxPreviewHeight : this.CurrentPreviewWidth / this.previewAspectRatio;
            }
            else
            {
                this.currentAssetSize.Width = this.CurrentPreviewHeight * this.previewAspectRatio > this.MaxPreviewWidth ? this.MaxPreviewWidth : this.CurrentPreviewHeight;
                this.currentAssetSize.Height = this.CurrentPreviewHeight;
            }
        }

        /// <summary>
        /// Handles the Changed event of the FullScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FullScreen_Changed(object sender, EventArgs e)
        {
            VideoPreview preview = sender as VideoPreview;

            if (preview != null)
            {
                // Check if there is any other VideoPreview which is in full screen mode. If yes then bring it to normal mode.
                this.AssetsList.GetChildControls<VideoPreview>().
                    Where(x => x.IsFullScreen && x != preview).ToList().
                    ForEach(y =>
                                {
                                    y.Scale(this.currentAssetSize);
                                    y.IsFullScreen = false;
                                });

                if (preview.IsFullScreen)
                {
                    preview.Scale(this.GetAssetFullScreenSize);
                }
                else
                {
                    preview.Scale(this.currentAssetSize);
                    preview.IsFullScreen = false;
                }

                // Update the layout so that the row heigts of the element can be calulated.
                this.AssetsList.UpdateLayout();

                // Set the offset of the scrollbar.
                this.SetScrollerOffset(preview.Asset);
            }
        }

        /// <summary>
        /// Handles the SizeChanged event of the AssetsList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void AssetsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IList<VideoPreview> videoPreviews = this.AssetsList.GetChildControls<VideoPreview>();

            foreach (VideoPreview preview in videoPreviews)
            {
                if (preview.IsFullScreen)
                {
                    preview.Scale(this.GetAssetFullScreenSize);
                }
            }

            this.lstSizeChanging = true;

            this.SetPreviewSizeMaintainingAspectRatio();
            this.UpdateZoom();

            this.lstSizeChanging = false;
        }

        /// <summary>
        /// Handles the MetadataClick event of the Preview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Preview_MetadataClick(object sender, EventArgs e)
        {
            AssetPreview assetPreview = sender as AssetPreview;
            if (assetPreview != null)
            {
                this.Model.ShowMetadata(assetPreview.Asset);
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the RootVisual control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void RootVisual_KeyUp(object sender, KeyEventArgs e)
        {
            // Activate the view if Ctrl + 1 is pressed.
            if (e.Key == System.Windows.Input.Key.D1 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.Model.Activate();
            }
        }
    }
}
