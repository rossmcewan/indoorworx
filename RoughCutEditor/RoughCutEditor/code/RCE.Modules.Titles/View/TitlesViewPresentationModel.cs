// <copyright file="TitlesViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TitlesViewPresentationModel.cs                     
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
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Regions;
    using RCE.Infrastructure;
    using RCE.Infrastructure.Events;
    using RCE.Infrastructure.Models;

    /// <summary>
    /// Presentation model for the title view.
    /// </summary>
    public class TitlesViewPresentationModel : BaseModel, ITitlesViewPresentationModel
    {
        /// <summary>
        /// The <seealso cref="IEventAggregator"/> instance used to publish and subscribe to events.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The <seealso cref="IDataServiceFacade"/> instance used to load the title templates.
        /// </summary>
        private readonly IDataServiceFacade serviceFacade;

        /// <summary>
        /// The <seealso cref="IConfigurationService"/> instance used to get the title templates XAML.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// The <seealso cref="IRegionManager"/> instance used to determine when the view is active.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The <seealso cref="Downloader"/> instance used to download the XAML templates.
        /// </summary>
        private readonly Downloader downloader;

        /// <summary>
        /// The field used to store the main title used for the titles.
        /// </summary>
        private string mainTitle;

        /// <summary>
        /// The field used to store the sub title used for the titles.
        /// </summary>
        private string subTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitlesViewPresentationModel"/> class.
        /// </summary>
        /// <param name="view">The view of Media Bin Module.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="serviceFacade">The service facade.</param>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="downloader">The downloader used to retrieve the xaml templates.</param>
        public TitlesViewPresentationModel(ITitlesView view, IEventAggregator eventAggregator, IDataServiceFacade serviceFacade, IConfigurationService configurationService, IRegionManager regionManager, Downloader downloader)
        {
            this.eventAggregator = eventAggregator;
            this.serviceFacade = serviceFacade;
            this.configurationService = configurationService;
            this.regionManager = regionManager;
            this.downloader = downloader;
            this.downloader.DownloadStringCompleted += this.OnClientOnDownloadStringCompleted;

            this.PopulateTitles();
            this.View = view;

            this.PropertyChanged += this.TitlesViewPresentationModel_PropertyChanged;
            this.MainTitle = "Main-Title Text.";
            this.SubTitle = "Sub-Title Text.";
            this.View.Model = this;
        }

        /// <summary>
        /// Gets the title templates.
        /// </summary>
        /// <value>The <see cref="ObservableCollection{T}"/> of title templates.</value>
        public ObservableCollection<TitleTemplate> TitleTemplates { get; private set; }

        /// <summary>
        /// Gets or sets the main title of the titles.
        /// </summary>
        /// <value>The main title.</value>
        public string MainTitle
        {
            get
            {
                return this.mainTitle;
            }

            set
            {
                this.mainTitle = value;
                this.OnPropertyChanged("MainTitle");
            }
        }

        /// <summary>
        /// Gets or sets the sub title of the titles.
        /// </summary>
        /// <value>The sub title.</value>
        public string SubTitle
        {
            get
            {
                return this.subTitle;
            }

            set
            {
                this.subTitle = value;
                this.OnPropertyChanged("SubTitle");
            }
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The <see cref="ITitlesView"/>.</value>
        public ITitlesView View { get; set; }

        /// <summary>
        /// Gets the header info.
        /// </summary>
        /// <value>The header info.</value>
        public string HeaderInfo
        {
            get { return Resources.Resources.HeaderInfo; }
        }

        /// <summary>
        /// Gets the header on icon.
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
        /// Gets a value indicating whether the TitleView is the active view.
        /// </summary>
        /// <value>A true if MediaBin is active;otherwise false.</value>
        public bool IsActive
        {
            get
            {
                return this.regionManager.Regions[RegionNames.ToolsRegion].ActiveViews.Where(x => x == this.View).SingleOrDefault() != null;
            }
        }

        /// <summary>
        /// Activates this media Titles view.
        /// </summary>
        public void Activate()
        {
            this.regionManager.Regions[RegionNames.ToolsRegion].Activate(this.View);
        }

        /// <summary>
        /// Publish the <see cref="AddAssetToTimelineEvent"/> to add the asset in the timeline at the current playhead position.
        /// </summary>
        /// <param name="titleTemplate">Title template of the title asset to be added to timeline.</param>
        public void AddTitleAssetToTimeline(TitleTemplate titleTemplate)
        {
            TitleAsset titleAsset = CreateTitleAsset(titleTemplate);

            if (titleTemplate != null)
            {
                this.eventAggregator.GetEvent<AddAssetToTimelineEvent>().Publish(titleAsset);
            }
        }

        /// <summary>
        /// Creates a <see cref="TitleAsset"/> associated to the <see cref="TitleTemplate"/> passed.
        /// </summary>
        /// <param name="titleTemplate">The title template of the title asset being created.</param>
        /// <returns>A new title asset.</returns>
        private static TitleAsset CreateTitleAsset(TitleTemplate titleTemplate)
        {
            TitleAsset titleAsset = new TitleAsset
                                        {
                                            Title = titleTemplate.Title,
                                            MainText = titleTemplate.MainText,
                                            SubText = titleTemplate.SubText,
                                            TitleTemplate = titleTemplate
                                        };

            return titleAsset;
        }

        /// <summary>
        /// This method populates the static pre-defined Titles for binding.
        /// </summary>
        private void PopulateTitles()
        {
            this.TitleTemplates = new ObservableCollection<TitleTemplate>();
            this.serviceFacade.LoadTitleTemplatesCompleted += (sender, e) =>
                      {
                          foreach (TitleTemplate titleTemplate in e.Data)
                          {
                              Uri xamlResource = this.configurationService.GetTitleTemplate(titleTemplate.Title);

                              downloader.DownloadStringAsync(xamlResource, titleTemplate);                              
                          }
                      };

            this.serviceFacade.LoadTitleTemplatesAsync();
        }

        /// <summary>
        /// Called when title XAML been downloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
        private void OnClientOnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs args)
        {
            TitleTemplate template = args.UserState as TitleTemplate;
            if (args.Error == null && template != null)
            {
                template.MainText = this.MainTitle;
                template.SubText = this.SubTitle;
                template.XamlResource = args.Result;
                this.TitleTemplates.Add(template);
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the TitlesViewPresentationModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void TitlesViewPresentationModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.TitleTemplates.ToList().ForEach(title =>
             {
                 title.MainText = this.MainTitle;
                 title.SubText = this.SubTitle;
             });
        }
    }
}
