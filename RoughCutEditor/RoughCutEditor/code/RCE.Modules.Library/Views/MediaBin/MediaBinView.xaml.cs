// <copyright file="MediaBinView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MediaBinView.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.MediaBin
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using Library;
    using RCE.Infrastructure;
    using RCE.Infrastructure.Controls;
    using RCE.Infrastructure.Models;
    using Services.Contracts;
    using SMPTETimecode;

    /// <summary>
    /// The view for the MediaBin module.
    /// </summary>
    public partial class MediaBinView : UserControl, IMediaBinView
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
        /// True if the popup is captured.
        /// </summary>
        private bool popupCaptured;

        /// <summary>
        /// Ticks for the last click of the mouse.
        /// </summary>
        private long lastClickTicks;

        /// <summary>
        /// Currently copied asset.
        /// </summary>
        private Asset clipBoardAsset;

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
        /// Initializes a new instance of the <see cref="MediaBinView"/> class.
        /// </summary>
        public MediaBinView()
        {
            InitializeComponent();
            this.currentAssetSize = new Size(this.assetMinWidth * 2, this.assetMinHeight * 2);

            HtmlPage.RegisterScriptableObject("MediaBin", this);

            // Key Commands
            // Register to MouseWheel event.
            if (Application.Current.RootVisual != null)
            {
                Application.Current.RootVisual.KeyUp += this.RootVisual_KeyUp;
                Application.Current.RootVisual.MouseWheel += this.OnMouseWheel;
            }

            // Initilaize values related to the scaling assets.
            this.assetMinHeight = this.previewSize.Height;
            this.assetMinWidth = this.previewSize.Width;
            this.previewAspectRatio = this.previewSize.Width / this.previewSize.Height;
            this.currentAssetSize = new Size(this.previewSize.Width, this.previewSize.Height);
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public IMediaBinViewPresentationModel Model
        {
            get
            {
                return this.DataContext as IMediaBinViewPresentationModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Gets the size of the preview element for full screen mode.
        /// </summary>
        /// <value>The size of the get asset full screen.</value>
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
        /// Gets the maximum possible height of the preview control.
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
        /// Gets the max possible width of preview.
        /// </summary>
        /// <value>The max possible width of the preview.</value>
        private double MaxPreviewWidth
        {
            get
            {
                return this.AssetsList.ActualWidth - ListRightOffset > this.assetMinWidth ? this.AssetsList.ActualWidth - ListRightOffset : this.assetMinWidth;
            }
        }

        /// <summary>
        /// Gets the width of the current preview corresponding to the slider value.
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
        /// Gets the height of the current preview corresponding to the slider value.
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
        /// Sets the current smpte frame rate in video preview.
        /// </summary>
        /// <param name="frameRate">The frame rate.</param>
        public void SetCurrentSmpteFrameRate(SmpteFrameRate frameRate)
        {
           // IList<VideoPreview> videoPreviews = this.AssetsList.GetChildControls<VideoPreview>();
            IList<VideoPreviewTimeline> videoPreviews = this.AssetsList.GetChildControls<VideoPreviewTimeline>();

            foreach (VideoPreviewTimeline preview in videoPreviews)
            {
                preview.UpdateSmpteFrameRate(frameRate);
            }
        }

        /// <summary>
        /// It addes the metadataFields fields in the list view of the assets.
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
        /// Gets the media bin.
        /// </summary>
        /// <returns>The array of provider uri of the all the assets.</returns>
        [ScriptableMember]
        public string[] GetMediaBin()
        {
            return this.Model.GetMediaBin();
        }

        /// <summary>
        /// Shows the messagebox and gets the delete asset confirmation.
        /// </summary>
        public void GetDeleteAssetConfirmation()
        {
            HtmlPage.Window.Dispatcher.BeginInvoke(() => this.DisplayAssetDeleteConfirmation());
        }

        /// <summary>
        /// Called when [mouse wheel] event occures.
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
                    VideoPreviewTimeline preview = this.currentAsset as VideoPreviewTimeline;

                    if (preview == null || !preview.IsFullScreen)
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
        }

        /// <summary>
        /// Handles the Playing event of the Asset control.
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
        /// Handles the Stopping event of the Asset control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Asset_Stopping(object sender, EventArgs e)
        {
            this.currentAsset = null;
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the UserControl control as well as the double click.
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
        /// Handles the KeyDown event of the RootVisual control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void RootVisual_KeyUp(object sender, KeyEventArgs e)
        {
            // Activate the view if Ctrl + 2 is pressed.
            if (e.Key == System.Windows.Input.Key.D2 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.Model.Activate();
            }

            ContentControl contentControl = e.OriginalSource as ContentControl;

            bool handleKey = e.OriginalSource is ListBoxItem ||
                             (contentControl != null &&
                              contentControl.Name.ToUpper(CultureInfo.CurrentCulture).StartsWith("SUBCLIPPLAYHEAD"));

            if (this.Model.IsActive)
            {
                switch (e.Key)
                {
                        // Delete asset.
                    case System.Windows.Input.Key.Delete:
                        if (handleKey)
                        {
                            //// This is required else the IE browser crashes. This is the bug in Silverlight v2.0.
                            //// Refer http://silverlight.net/forums/p/61357/152971.aspx for details

                            this.Model.DeleteAssetCommand.Execute(null);
                            e.Handled = true;
                        }

                        break;
                    case System.Windows.Input.Key.X:
                        if (Keyboard.Modifiers == ModifierKeys.Control && handleKey)
                        {
                            this.CopyAssetToClipboard();
                            e.Handled = true;
                        }

                        break;
                    case System.Windows.Input.Key.V:
                        if (Keyboard.Modifiers == ModifierKeys.Control && handleKey)
                        {
                            this.CopyAssetFromClipboardToSelectedFolder();
                            e.Handled = true;
                        }

                        break;
                    case System.Windows.Input.Key.A:
                        if (handleKey)
                        {
                            this.AddAssetToTimeline();
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Adds the asset to timeline.
        /// </summary>
        private void AddAssetToTimeline()
        {
            Asset selectedAsset = this.AssetsList.SelectedItem as Asset;
            if (selectedAsset != null && !(selectedAsset is FolderAsset))
            {
                if (selectedAsset is VideoAsset)
                {
                    // VideoPreview preview = this.AssetsList.GetChildControls<VideoPreview>().Where(x => x.Asset == selectedAsset).Single();
                    VideoPreviewTimeline preview = this.AssetsList.GetChildControls<VideoPreviewTimeline>().Where(x => x.Asset == selectedAsset).Single();
                    VideoAsset videoAsset = selectedAsset as VideoAsset;
                    VideoAssetInOut videoAssetInOut = new VideoAssetInOut(videoAsset)
                    {
                        InPosition = preview.InPosition.TotalSeconds,
                        OutPosition = preview.OutPosition.TotalSeconds
                    };
                    this.Model.AddAssetToTimeline(videoAssetInOut);
                }
                else
                {
                    this.Model.AddAssetToTimeline(selectedAsset);
                }
            }
        }

        /// <summary>
        /// Displays the asset delete confirmation messagebox.
        /// </summary>
        private void DisplayAssetDeleteConfirmation()
        {
            //// This is required else the IE browser crashes. This is the bug in Silverlight v2.0.
            //// Refer http://silverlight.net/forums/p/61357/152971.aspx for details
            
            MessageBoxResult result = MessageBox.Show(
                                                        "Are you sure you want to delete all the instance of this asset from the current project and the timeline?",
                                                        "Delete Media Asset",
                                                        MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == result)
            {
                this.Model.DeleteCurrentAsset();
            }
        }

        /// <summary>
        /// Copies the selected asset to clipBoardAsset if the asset is not <see cref="FolderAsset"/>.
        /// </summary>
        private void CopyAssetToClipboard()
        {
            Asset selectedAsset = this.AssetsList.SelectedItem as Asset;

            // Check if the selected asset is playing then first stop it.
            // Also, need to check that the user copies ONLY Audio, Video or Image asset.
            if (selectedAsset != null && !(selectedAsset is FolderAsset))
            {
                this.clipBoardAsset = selectedAsset;
            }
        }

        /// <summary>
        /// Copies the asset from clipboard to selected folder.
        /// </summary>
        private void CopyAssetFromClipboardToSelectedFolder()
        {
            if (this.clipBoardAsset != null)
            {
                Asset selectedFolderAsset = this.AssetsList.SelectedItem as Asset;

                // Check that the user pastes the selected asset (Audio, Video or Image asset) to a folder.
                // If a folder is selected then copy the asset in the selected folder
                // else copy it in the current folder.
                if (selectedFolderAsset != null && selectedFolderAsset is FolderAsset)
                {
                    this.Model.AddAssetToFolder(this.clipBoardAsset, selectedFolderAsset.Id);
                    this.Model.RefreshCurrentAssets();
                }
                else
                {
                    this.Model.CopyAssetFromClipboardToCurrentFolder(this.clipBoardAsset);
                }

                this.clipBoardAsset = null;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the Slider control.
        /// Resize all the items in the AssetListBox according to the value of the slider.
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
        /// Scale all the assets in the listbox according to the slider value.
        /// </summary>
        private void UpdateZoom()
        {
            if (this.ZoomSlider != null)
            {
                IList<AssetPreview> previews = this.AssetsList.GetChildControls<AssetPreview>();

                foreach (AssetPreview preview in previews)
                {
                    // VideoPreview videoPreview = preview as VideoPreview;
                    VideoPreviewTimeline videoPreview = preview as VideoPreviewTimeline;

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
        /// Sets the preview size maintaining aspect ratio.
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
        /// Sets the scroller offset to the given asset.
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
        /// Handles the Changed event of the FullScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FullScreen_Changed(object sender, EventArgs e)
        {
            // VideoPreview preview = sender as VideoPreview;
            VideoPreviewTimeline preview = sender as VideoPreviewTimeline;

            if (preview != null)
            {
                // Check if there is any other VideoPreview which is in full screen mode. If yes then bring it to normal mode.
                // this.AssetsList.GetChildControls<VideoPreview>().
                //    Where(x => x.IsFullScreen && x != preview).ToList().
                //    ForEach(y =>
                //                {
                //                    y.Scale(this.currentAssetSize);
                //                    y.IsFullScreen = false;
                //                });
                this.AssetsList.GetChildControls<VideoPreviewTimeline>().
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
            // IList<VideoPreview> videoPreviews = this.AssetsList.GetChildControls<VideoPreview>();
            IList<VideoPreviewTimeline> videoPreviews = this.AssetsList.GetChildControls<VideoPreviewTimeline>();

            foreach (VideoPreviewTimeline preview in videoPreviews)
            {
                if (preview.IsFullScreen)
                {
                    preview.Scale(this.GetAssetFullScreenSize);
                }
            }

            this.lstSizeChanging = true;

            this.SetPreviewSizeMaintainingAspectRatio();
            this.UpdateZoom();

            this.AssetsList.UpdateLayout();

            this.lstSizeChanging = false;
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
                // calculate and set the slider value from the initialized values.
                if (this.AssetsList.ActualWidth - this.assetMinWidth != 0)
                {
                    this.ZoomSlider.Value = (this.currentAssetSize.Width - this.assetMinWidth) / (this.AssetsList.ActualWidth - this.assetMinWidth);
                }

                this.ZoomSlider.ValueChanged += this.Slider_ValueChanged;
                this.AssetsList.SizeChanged += this.AssetsList_SizeChanged;

                Binding playSelectedAssetCommand = new Binding("PlaySelectedAssetCommand") { Source = this.DataContext };
                ((BindingHelper)this.Resources["PlaySelectedAssetCommand"]).SetBinding(BindingHelper.ValueProperty, playSelectedAssetCommand);

                this.isLoaded = true;
            }

            Binding dropCommand = new Binding("DropCommand") { Source = this.DataContext };
            ((BindingHelper)this.Resources["DropItemCommand"]).SetBinding(BindingHelper.ValueProperty, dropCommand);
        }

        /// <summary>
        /// Handles the KeyUp event of the TextBox control.
        /// If the key enterd is Enter then searches the value and shows the result.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Model.SearchCommand.Execute(this.SearchTextBox.Text);
            }
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
        /// Handles the TextChanged event of the FolderName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void FolderName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Model.FolderTitle = this.FolderName.Text;
            this.Model.AddFolderCommand.RaiseCanExecuteChanged();
        }
    }
}
