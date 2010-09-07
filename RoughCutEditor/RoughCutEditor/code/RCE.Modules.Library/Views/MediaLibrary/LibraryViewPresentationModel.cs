// <copyright file="LibraryViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: LibraryViewPresentationModel.cs                     
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
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;

    /// <summary>
    /// Presentation model for the library view.
    /// </summary>
    public class LibraryViewPresentationModel : BaseModel, ILibraryViewPresentationModel
    {
        /// <summary>
        /// The shift value for the slider control when Zoom In/Out is clicked.
        /// </summary>
        private const double ShiftScaleMargin = 0.05;

        /// <summary>
        /// The <see cref="IEventAggregator"/> to publish/subscribe for the events.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The <see cref="IRegionManager"/> activate the library view.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The <see cref="IConfigurationService"/> to get configuration parameters.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// The error view resolver.
        /// </summary>
        private readonly Func<IErrorView> errorViewResolver;

        /// <summary>
        /// The <see cref="DelegateCommand{T}"/> to handle the add asset to mediabin.
        /// </summary>
        private readonly DelegateCommand<object> addItemCommand;

        /// <summary>
        /// The <see cref="DelegateCommand{T}"/> to handle Zoom In/Out in slider.
        /// </summary>
        private readonly DelegateCommand<string> shiftSliderScaleCommand;

        /// <summary>
        /// The <see cref="DelegateCommand{T}"/> to handle playing the selected asset.
        /// </summary>
        private readonly DelegateCommand<object> playSelectedAssetCommand;

        /// <summary>
        /// The <see cref="DelegateCommand{T}"/> to show the assets of the parent folder
        /// of the current folder.
        /// </summary>
        private readonly DelegateCommand<string> parentFolderCommand;

        /// <summary>
        /// The <see cref="DelegateCommand{T}"/> to Show/Hide the help window.
        /// </summary>
        private readonly DelegateCommand<string> helpViewCommand;

        /// <summary>
        /// It contains the list of assets for the current folder.
        /// It is used to show the assets in the view.
        /// </summary>
        private List<Asset> assets;

        /// <summary>
        /// It contains all the assets in the library.
        /// </summary>
        private List<Asset> currentAssets;

        /// <summary>
        /// Flag indicating if the user want to see the videos in the view.
        /// </summary>
        private bool showVideos;

        /// <summary>
        /// Flag indicating if the user want to see the audios in the view.
        /// </summary>
        private bool showAudio;

        /// <summary>
        /// Flag indicating if the user want to see the images in the view.
        /// </summary>
        private bool showImages;

        /// <summary>
        /// To have the value of the slider control.
        /// </summary>
        private double scale;

        /// <summary>
        /// The current folder whose assets are visible in th library view.
        /// </summary>
        private FolderAsset currentFolderAsset;

        /// <summary>
        /// Flag indicating if the help window is open.
        /// </summary>
        private bool isHelpWindowOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryViewPresentationModel"/> class.
        /// </summary>
        /// <param name="view">The <see cref="ILibraryView"/>.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="errorViewResolver">The error view resolver.</param>
        public LibraryViewPresentationModel(ILibraryView view, IEventAggregator eventAggregator, IRegionManager regionManager, IConfigurationService configurationService, Func<IErrorView> errorViewResolver)
        {
            this.eventAggregator = eventAggregator;
            this.View = view;
            this.regionManager = regionManager;
            this.configurationService = configurationService;
            this.errorViewResolver = errorViewResolver;

            this.addItemCommand = new DelegateCommand<object>(this.AddToMediaBin);
            this.shiftSliderScaleCommand = new DelegateCommand<string>(this.ShiftScale, this.CanShiftScale);
            this.parentFolderCommand = new DelegateCommand<string>(this.ShowParentFolders, this.CanShowParentFolder);
            this.helpViewCommand = new DelegateCommand<string>(this.ShowHelpView);
            this.playSelectedAssetCommand = new DelegateCommand<object>(this.PlaySelectedAsset);

            this.assets = new List<Asset>();
            this.currentAssets = new List<Asset>();

            // Todo: Get this value from the config file.
            this.Scale = 0.2;
            this.ShowImages = true;
            this.ShowVideos = true;
            this.ShowAudio = true;

            this.PropertyChanged += this.LibraryViewPresentationModel_PropertyChanged;

            this.eventAggregator.GetEvent<AssetsAvailableEvent>().Subscribe(this.OnAssetsAvailable, true);
            
            this.View.ShowProgressBar();

            // Add metadata fields.
            this.View.AddMetadataFields(this.configurationService.GetMetadataFields());
            this.View.Model = this;
        }

        /// <summary>
        /// Gets a value indicating whether library view is the active view.
        /// </summary>
        /// <value>A <seealso cref="bool"/> value.<c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive 
        {
            get
            {
                return this.regionManager.Regions[RegionNames.ToolsRegion].ActiveViews.Where(x => x == this.View).SingleOrDefault() != null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Help window is open.
        /// </summary>
        /// <value>A boolean value.<c>true</c> if help window is open; otherwise, <c>false</c>.</value>
        public bool IsHelpWindowOpen
        {
            get
            {
                return this.isHelpWindowOpen;
            }

            set
            {
                this.isHelpWindowOpen = value;
                this.OnPropertyChanged("IsHelpWindowOpen");
            }
        }

        /// <summary>
        /// Gets the UpArrowCommand command.
        /// </summary>
        /// <value>The <seealso cref="DelegateCommand{T}"/> for navigate up from a folder.</value>
        public DelegateCommand<string> UpArrowCommand
        {
            get
            {
                return this.parentFolderCommand;
            }
        }

        /// <summary>
        /// Gets the shiftSliderScaleCommand command.
        /// </summary>
        /// <value>The <seealso cref="DelegateCommand{T}"/> for shifting the slider scale value.</value>
        public DelegateCommand<string> ShiftSliderScaleCommand
        {
            get
            {
                return this.shiftSliderScaleCommand;
            }
        }

        /// <summary>
        /// Gets or sets the scale of the asset controls.
        /// </summary>
        /// <value>A <seealso cref="double"/> value that represents the current scale of the slider.</value>
        public double Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                this.scale = value;
                this.OnPropertyChanged("Scale");
            }
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The <see cref="LibraryView"/>.</value>
        public ILibraryView View { get; set; }

        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>The assets.</value>
        public List<Asset> Assets
        {
            get
            {
                return this.assets;
            }

            set
            {
                this.assets = value;
                this.OnPropertyChanged("Assets");
            }
        }

        /// <summary>
        /// Gets the header info.
        /// </summary>
        /// <value>The header info.</value>
        public string HeaderInfo
        {
            get { return Resources.Resources.HeaderInfo; }
        }

        /// <summary>
        /// Gets the header icon (on status).
        /// </summary>
        /// <value>The header icon on.</value>
        public string HeaderIconOn
        {
            get { return Resources.Resources.HeaderIconOn; }
        }

        /// <summary>
        /// Gets the header icon (off status).
        /// </summary>
        /// <value>The header icon off.</value>
        public string HeaderIconOff
        {
            get { return Resources.Resources.HeaderIconOff; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show videos].
        /// </summary>
        /// <value>A <seealso cref="bool"/> value.<c>true</c> if [show videos]; otherwise, <c>false</c>.</value>
        public bool ShowVideos
        {
            get
            {
                return this.showVideos;
            }

            set
            {
                this.showVideos = value;
                this.OnPropertyChanged("ShowVideos");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show audio].
        /// </summary>
        /// <value>A <seealso cref="bool"/> value.<c>true</c> if [show audio]; otherwise, <c>false</c>.</value>
        public bool ShowAudio
        {
            get
            {
                return this.showAudio;
            }

            set
            {
                this.showAudio = value;
                this.OnPropertyChanged("ShowAudio");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show images].
        /// </summary>
        /// <value>A <seealso cref="bool"/> value.<c>true</c> if [show images]; otherwise, <c>false</c>.</value>
        public bool ShowImages
        {
            get
            {
                return this.showImages;
            }

            set
            {
                this.showImages = value;
                this.OnPropertyChanged("ShowImages");
            }
        }

       /// <summary>
        /// Gets the play selected asset.
        /// </summary>
        /// <value>The play selected asset.</value>
        public DelegateCommand<object> PlaySelectedAssetCommand
        {
            get
            {
                return this.playSelectedAssetCommand;
            }
        }

        /// <summary>
        /// Gets the add to media bin command.
        /// </summary>
        /// <value>The add to media bin command.</value>
        public DelegateCommand<object> AddItemCommand
        {
            get
            {
                return this.addItemCommand;
            }
        }

        /// <summary>
        /// Gets the add to media bin command.
        /// </summary>
        /// <value>The add to media bin command.</value>
        public DelegateCommand<string> HelpButtonCommand
        {
            get
            {
                return this.helpViewCommand;
            }
        }

        /// <summary>
        /// Activates this media library view.
        /// </summary>
        public void Activate()
        {
            this.regionManager.Regions[RegionNames.ToolsRegion].Activate(this.View);
        }

        /// <summary>
        /// Show the helpview popup.
        /// </summary>
        /// <param name="key">Not used value.</param>
        public void ShowHelpView(string key)
        {
            this.IsHelpWindowOpen = !this.IsHelpWindowOpen;
        }

        /// <summary>
        /// Shows the metadata of the current Asset.
        /// </summary>
        /// <param name="asset">The current asset.</param>
        public void ShowMetadata(Asset asset)
        {
            if (asset != null && !(asset is FolderAsset || asset is TitleAsset))
            {
                this.eventAggregator.GetEvent<ShowMetadataEvent>().Publish(asset);
            }
        }

        /// <summary>
        /// Called when any asset is added to MediaBin.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void OnAddAsset(Asset asset)
        {
            if (asset != null)
            {
                this.eventAggregator.GetEvent<AddAssetEvent>().Publish(asset);
            }
        }

        /// <summary>
        /// Called when double clicked on any asset. It shows the child asset if 
        /// the current asset is folder asset else publish the PlayerEvent.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void OnAssetSelected(Asset asset)
        {
            if (asset is FolderAsset)
            {
                this.ShowChildFolders(asset);
            }
            else
            {
                this.eventAggregator.GetEvent<PlayerEvent>().Publish(new PlayerEventPayload
                {
                    Asset = asset,
                    PlayerMode = PlayerMode.MediaLibrary
                });
            }
        }

        /// <summary>
        /// Gets the parent folder of the current folder.
        /// </summary>
        /// <returns>The <see cref="FolderAsset"/>.</returns>
        private FolderAsset GetParentFolderAsset()
        {
            if (this.currentFolderAsset == null || this.currentFolderAsset.ParentFolder == null)
            {
                return null;
            }

            return this.currentFolderAsset.ParentFolder;
        }

        /// <summary>
        /// Searches the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        private void Search(string filter)
        {
            this.currentFolderAsset = null;
            this.UpArrowCommand.RaiseCanExecuteChanged();

            // this.serviceFacade.LoadAssetsAsync(this.configurationService.GetMediaLibraryUri(), filter, this.configurationService.GetMaxNumberOfItems());
        }

        /// <summary>
        /// Plays the selected asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        private void PlaySelectedAsset(object assetId)
        {
            if (assetId is Guid)
            {
                Guid id = (Guid)assetId;

                Asset asset = this.Assets.FirstOrDefault(x => x.Id.Equals(id));

                if (asset != null && !(asset is FolderAsset))
                {
                    var payload = new PlayerEventPayload { Asset = asset, PlayerMode = PlayerMode.MediaLibrary };
                    this.eventAggregator.GetEvent<PlayerEvent>().Publish(payload);
                }
            }
        }

        /// <summary>
        /// Publishes the <see cref="AddAssetEvent"/> if the asset is valid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        private void AddToMediaBin(object assetId)
        {
            if (assetId is Guid)
            {
                Guid id = (Guid) assetId;

                Asset asset = this.Assets.FirstOrDefault(x => x.Id.Equals(id));

                if (asset != null)
                {
                    this.eventAggregator.GetEvent<AddAssetEvent>().Publish(asset);
                }
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the LibraryViewPresentationModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void LibraryViewPresentationModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ShowVideos") || e.PropertyName.Equals("ShowAudio") || e.PropertyName.Equals("ShowImages"))
            {
                this.FilterAssets();
            }
        }

        /// <summary>
        /// Handles the AssetsAvailableEvent event.
        /// </summary>
        /// <param name="e">The <see cref="Infrastructure.DataEventArgs{T}"/> instance containing the event data.</param>
        private void OnAssetsAvailable(RCE.Infrastructure.DataEventArgs<List<Asset>> e)
        {
            if (e.Error == null)
            {
                if (this.currentFolderAsset != null)
                {
                    this.currentFolderAsset.AddAssets(e.Data);
                }
                else
                {
                    this.currentAssets = e.Data;
                }

                this.FilterAssets();
            }
            else
            {
                IErrorView errorView = this.errorViewResolver();
                errorView.ErrorMessage = Resources.Resources.MediaLibraryLoadAssetsError;
                errorView.Show();
            }

            this.UpArrowCommand.RaiseCanExecuteChanged();
            this.View.HideProgressBar();
        }

        /// <summary>
        /// Filters the assets.
        /// </summary>
        private void FilterAssets()
        {
            List<Asset> filteredAssets = new List<Asset>();

            // Get all the assets in the current selected folder.
            IEnumerable<Asset> currentFolderAssets = (this.currentFolderAsset != null) ? this.currentFolderAsset.Assets.ToList() : this.currentAssets;

            if (this.ShowAudio)
            {
                filteredAssets.AddRange(currentFolderAssets.Where(x => x is AudioAsset));
            }

            if (this.ShowVideos)
            {
                filteredAssets.AddRange(currentFolderAssets.Where(x => x is VideoAsset));
            }

            if (this.ShowImages)
            {
                filteredAssets.AddRange(currentFolderAssets.Where(x => x is ImageAsset));
            }

            // Add all the the folder assets which exist in the current selected folder.
            filteredAssets.AddRange(currentFolderAssets.Where(x => x is FolderAsset));
            this.Assets = filteredAssets;
        }

        /// <summary>
        /// Increase/Decrese the value of the slider by default value.
        /// </summary>
        /// <param name="shiftType">Type of the shift.</param>
        private void ShiftScale(string shiftType)
        {
            switch (shiftType)
            {
                case "+":
                    if (this.Scale != 1)
                    {
                        this.Scale = (this.Scale + ShiftScaleMargin > 1) ? 1 : this.Scale + ShiftScaleMargin;
                    }

                    break;
                case "-":
                    if (this.Scale != 0)
                    {
                        this.Scale = (this.Scale - ShiftScaleMargin < 0) ? 0 : this.Scale - ShiftScaleMargin;
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Determines whether the slider value can be shifted in the given direction.
        /// </summary>
        /// <param name="shiftType">Type of the shift.</param>
        /// <returns>
        /// <c>true</c> if shift is possible; otherwise, <c>false</c>.
        /// </returns>
        private bool CanShiftScale(string shiftType)
        {
            switch (shiftType)
            {
                case "+":
                    if (this.scale < 1)
                    {
                        return true;
                    }

                    break;
                case "-":
                    if (this.scale > 0)
                    {
                        return true;
                    }

                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Shows the assets of the parent folder of the the current folder.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        private void ShowParentFolders(string folderName)
        {
            if (this.currentFolderAsset != null)
            {
                this.currentFolderAsset = this.GetParentFolderAsset();
                this.UpArrowCommand.RaiseCanExecuteChanged();
                this.FilterAssets();
            }
        }

        /// <summary>
        /// Determines whether the current folder has any parent folder or 
        /// it is the top level folder.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns>
        /// <c>true</c> if the current folder is not the top level folder; otherwise, <c>false</c>.
        /// </returns>
        private bool CanShowParentFolder(string folderName)
        {
            return this.currentFolderAsset != null;
        }

        /// <summary>
        /// Shows the assets of the given folder asset.
        /// </summary>
        /// <param name="asset">The <see cref="FolderAsset"/>.</param>
        private void ShowChildFolders(Asset asset)
        {
            FolderAsset folderAsset = asset as FolderAsset;
            if (folderAsset != null)
            {
                // Set the current folder.
                this.currentFolderAsset = folderAsset;
                this.FilterAssets();

                // TODO: Add a loaded property to the folder to know if is loaded.
                if (this.currentFolderAsset.Assets.Count == 0)
                {
                    this.View.ShowProgressBar();

                  // this.serviceFacade.LoadAssetsAsync(folderAsset.ProviderUri, this.configurationService.GetMaxNumberOfItems());
                }
                else
                {
                    this.UpArrowCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
