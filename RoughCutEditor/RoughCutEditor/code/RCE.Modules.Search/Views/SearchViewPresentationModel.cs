// <copyright file="SearchViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SearchViewPresentationModel.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Search
{
    using System.Collections.Generic;
    using Infrastructure;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;

    public class SearchViewPresentationModel : BaseModel, ISearchViewPresentationModel
    {
        /// <summary>
        /// The <see cref="IConfigurationService"/> to get configuration parameters.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// The <seealso cref="IAssetsDataServiceFacade"/> instance used to used to get the list of assets.
        /// </summary>
        private readonly IAssetsDataServiceFacade assetsDataServiceFacade;

        /// <summary>
        /// The <see cref="IEventAggregator"/> to publish/subscribe for the events.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The <see cref="DelegateCommand{T}"/> to handle the serach.
        /// </summary>
        private readonly DelegateCommand<string> searchCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewPresentationModel"/> class.
        /// </summary>
        /// <param name="view">The view associated with this presentation model.</param>
        /// <param name="configurationService">The configuration service instance used to retrieve configuration values.</param>
        /// <param name="assetsDataServiceFacade">The service facade used to search for assets.</param>
        /// <param name="eventAggregator">The event aggregator instance used to publish/subscribe for events.</param>
        public SearchViewPresentationModel(ISearchView view, IConfigurationService configurationService, IAssetsDataServiceFacade assetsDataServiceFacade, IEventAggregator eventAggregator)
        {
            this.configurationService = configurationService;
            this.assetsDataServiceFacade = assetsDataServiceFacade;
            this.eventAggregator = eventAggregator;
            this.assetsDataServiceFacade.LoadAssetsCompleted += this.DataServiceFacade_LoadAssetsCompleted;
            this.View = view;

            this.searchCommand = new DelegateCommand<string>(this.Search);

            this.View.Model = this; 

            this.Search(null);
        }

        public ISearchView View { get; private set; }
        
        public string Title { get; set; }
        
        public DelegateCommand<string> SearchCommand
        {
            get { return this.searchCommand; }
        }

        private void Search(string parameter)
        {
            this.assetsDataServiceFacade.LoadAssetsAsync(parameter, this.configurationService.GetMaxNumberOfItems());
        }

        private void DataServiceFacade_LoadAssetsCompleted(object sender, Infrastructure.DataEventArgs<List<Asset>> e)
        {
            this.eventAggregator.GetEvent<AssetsAvailableEvent>().Publish(e);
        }
    }
}