// <copyright file="MetadataViewPresentationModel.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MetadataViewPresentationModel.cs                     
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.ServiceLocation;
    using SMPTETimecode;

    /// <summary>
    /// Presentation Model for the Metadata view.
    /// </summary>
    public class MetadataViewPresentationModel : BaseModel
    {
        /// <summary>
        /// The <see cref="IConfigurationService"/> to get configuration parameters.
        /// </summary>
        private readonly IConfigurationService configurationService;

        /// <summary>
        /// The <see cref="IEventDataParser{T}"/> used to parse events.
        /// </summary>
        private readonly IEventDataParser<EventData> eventDataParser;

        /// <summary>
        /// The <see cref="IEventDataParser{T}"/> used to parse offset events.
        /// </summary>
        private readonly IEventDataParser<EventOffset> eventOffsetParser;

        /// <summary>
        /// Contains the available metadata.
        /// </summary>
        private readonly IList<EventData> availableMetadata;

        /// <summary>
        /// The results message pattern used for showing results message.
        /// </summary>
        private const string ResultsMessagePattern = "{0} of {1} results {2}";

        /// <summary>
        /// Contains the results message instance.
        /// </summary>
        private string resultsText;

        /// <summary>
        /// The collection of events.
        /// </summary>
        private InStreamCollection inStreamData;

        /// <summary>
        /// The current event offset.
        /// </summary>
        private TimeSpan currentEventOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataViewPresentationModel"/> class.
        /// </summary>
        public MetadataViewPresentationModel()
            : this(
            ServiceLocator.Current.GetInstance<IConfigurationService>(), 
            ServiceLocator.Current.GetInstance<IEventDataParser<EventData>>(), 
            ServiceLocator.Current.GetInstance<IEventDataParser<EventOffset>>())
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataViewPresentationModel"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service instance.</param>
        /// <param name="eventDataParser">The event data parser instance.</param>
        /// <param name="eventOffsetParser">The event offset parser instance.</param>
        public MetadataViewPresentationModel(IConfigurationService configurationService, IEventDataParser<EventData> eventDataParser, IEventDataParser<EventOffset> eventOffsetParser)
        {
            this.configurationService = configurationService;
            this.eventDataParser = eventDataParser;
            this.eventOffsetParser = eventOffsetParser;
            this.SearchCommand = new DelegateCommand<string>(this.Search);
            this.Metadata = new ObservableCollection<EventData>();
            this.availableMetadata = new List<EventData>();
            this.MetadataFilters = new List<string> { "Both", "Play By Play", "Commentary" };
            this.SelectedMetadataFilter = this.MetadataFilters[0];
        }
        
        /// <summary>
        /// Gets or sets the event data collection.
        /// </summary>
        /// <value>The collection of events.</value>
        public ObservableCollection<EventData> Metadata { get; set; }

        /// <summary>
        /// Gets the search command being executed when search ocurrs.
        /// </summary>
        /// <value>The search command instance.</value>
        public DelegateCommand<string> SearchCommand { get; private set; }

        /// <summary>
        /// Gets or sets teh start offset being applied to the events.
        /// </summary>
        /// <value>The start offset.</value>
        public TimeCode StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the results text message being shown to the user.
        /// </summary>
        /// <value>The results text message.</value>
        public string ResultsText
        {
            get 
            {
                return this.resultsText; 
            }

            set
            {
                this.resultsText = value;

                this.OnPropertyChanged("ResultsText");
            }
        }

        /// <summary>
        /// Gets the list of available filters.
        /// </summary>
        /// <value>The list of available filters.</value>
        public IList<string> MetadataFilters { get; private set; }

        /// <summary>
        /// Gets or sets the selected filter.
        /// </summary>
        /// <value>The selected metadata filter.</value>
        public string SelectedMetadataFilter { get; set; }

        /// <summary>
        /// Sets the current stream data.
        /// </summary>
        /// <param name="inStreamData">The current stream data.</param>
        public void SetInStreamData(InStreamCollection inStreamData)
        {
            this.inStreamData = inStreamData;

            if (inStreamData.Count > 0)
            {
                this.AddEvents(inStreamData.ToList());
            }

            this.inStreamData.CollectionChanged += this.InStreamData_CollectionChanged;
        }

        /// <summary>
        /// Searches over teh available metadata.
        /// </summary>
        /// <param name="parameter">The search parameter.</param>
        private void Search(string parameter)
        {
            List<EventData> results = new List<EventData>();
            int? currentLimit = this.configurationService.GetParameterValueAsInt("SearchWithinAssetsLimit");

            this.Metadata.Clear();
            this.ResultsText = string.Empty;

            this.availableMetadata.Where(x => x.Text.ToUpper(CultureInfo.InvariantCulture).Contains(parameter.ToUpper()))
                .ToList()
                .ForEach(m => results.Add(m));

            results.Sort((t1, t2) => t1.Time.CompareTo(t2.Time));

            bool limitExceeded = currentLimit.HasValue && results.Count > currentLimit;

            if (limitExceeded)
            {
                this.SetLimitExceededMessage(currentLimit.Value, results.Count);
                results = results.Take(currentLimit.Value).ToList();
            }
            else
            {
                this.SetResultsMessage(results.Count);
            }

            results.ForEach(x => this.Metadata.Add(x));
        }

        /// <summary>
        /// Shows an event.
        /// </summary>
        /// <param name="eventData">The event being shown.</param>
        private void ShowEvent(EventData eventData)
        {
            int? currentLimit = this.configurationService.GetParameterValueAsInt("SearchWithinAssetsLimit");

            bool limitExceeded = currentLimit.HasValue && this.Metadata.Count > currentLimit;

            if (limitExceeded)
            {
                this.SetLimitExceededMessage(currentLimit.Value, this.availableMetadata.Count);
            }
            else
            {
                this.Metadata.Add(eventData);
                this.SetResultsMessage(this.Metadata.Count);
            }
        }

        /// <summary>
        /// Hides an event.
        /// </summary>
        private void HideEvent()
        {
            int? currentLimit = this.configurationService.GetParameterValueAsInt("SearchWithinAssetsLimit");

            bool limitExceeded = currentLimit.HasValue && this.Metadata.Count > currentLimit;

            if (limitExceeded)
            {
                this.SetLimitExceededMessage(currentLimit.Value, this.availableMetadata.Count);
            }
            else
            {
                this.SetResultsMessage(this.Metadata.Count);
            }
        }

        /// <summary>
        /// Sets the limit exceeded message to the results text.
        /// </summary>
        /// <param name="currentLimit">The current limit.</param>
        /// <param name="availableItems">The number of available items.</param>
        private void SetLimitExceededMessage(int currentLimit, int availableItems)
        {
            this.ResultsText = string.Format(ResultsMessagePattern, currentLimit, availableItems, "(limit exceeded)");
        }

        /// <summary>
        /// Sets the results message to the results text.
        /// </summary>
        /// <param name="currentItems">The number of current items.</param>
        private void SetResultsMessage(int currentItems)
        {
            this.ResultsText = string.Format(ResultsMessagePattern, currentItems, currentItems, string.Empty);
        }

        /// <summary>
        /// Handles the CollectionChanged event of the InStreamData collection. Adds/Removes events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args instance containing event data.</param>
        private void InStreamData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.AddEvents(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    this.RemoveEvents(e.OldItems);
                    break;
            }
        }

        /// <summary>
        /// Adds a list of events to the current items collection.
        /// </summary>
        /// <param name="items">The list of items being added.</param>
        private void AddEvents(IList items)
        {
            double? offset = this.configurationService.GetParameterValueAsDouble("GSISOffsetSeconds");

            foreach (XElement element in items)
            {
                EventOffset eventOffset = this.eventOffsetParser.ParseEventData(element);

                if (eventOffset == null)
                {
                    EventData eventData = this.eventDataParser.ParseEventData(element);

                    if (eventData != null)
                    {
                        eventData.Time = eventData.Time.Add(TimeSpan.FromSeconds(offset.GetValueOrDefault())).Add(this.currentEventOffset);
                        this.availableMetadata.Add(eventData);

                        this.ShowEvent(eventData);
                    }
                }
                else
                {
                    this.currentEventOffset = TimeSpan.FromSeconds(eventOffset.Offset);
                    
                    foreach (EventData eventData in this.availableMetadata)
                    {
                        eventData.Time = eventData.Time.Add(this.currentEventOffset);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the list of items from the current event collection.
        /// </summary>
        /// <param name="items">The items being removed.</param>
        private void RemoveEvents(IList items)
        {
            foreach (XElement element in items)
            {
                EventOffset eventOffset = this.eventOffsetParser.ParseEventData(element);

                if (eventOffset == null)
                {
                    EventData eventData = this.eventDataParser.ParseEventData(element);

                    if (eventData != null)
                    {
                        EventData currentEventData =
                            this.availableMetadata.Where(x => x.Id == eventData.Id).FirstOrDefault();

                        if (currentEventData != null)
                        {
                            this.availableMetadata.Remove(currentEventData);
                            this.Metadata.Remove(currentEventData);

                            this.HideEvent();
                        }
                    }
                }
                else
                {
                    foreach (EventData eventData in this.availableMetadata)
                    {
                        eventData.Time = eventData.Time.Subtract(this.currentEventOffset);
                    }

                    this.currentEventOffset = TimeSpan.Zero;
                }
            }
        }
    }
}
