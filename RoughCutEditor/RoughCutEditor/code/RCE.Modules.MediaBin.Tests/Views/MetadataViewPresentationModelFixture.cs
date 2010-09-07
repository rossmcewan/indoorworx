// <copyright file="MetadataViewPresentationModelFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MetadataViewPresentationModelFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.MediaBin.Tests.Views
{
    using System;
    using System.Xml.Linq;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// Test class for <see cref="MetadataViewPresentationModel"/>.
    /// </summary>
    [TestClass]
    public class MetadataViewPresentationModelFixture
    {
        /// <summary>
        /// Mock for <see cref="IConfigurationService"/>.
        /// </summary>
        private MockConfigurationService configurationService;

        /// <summary>
        /// Mock for <see cref="IEventDataParser{T}"/>.
        /// </summary>
        private MockEventDataParser eventDataParser;

        /// <summary>
        /// Mock for <see cref="IEventDataParser{T}" />.
        /// </summary>
        private MockEventOffsetParser eventOffsetParser;

        /// <summary>
        /// Initilize the default values.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.configurationService = new MockConfigurationService();
            this.eventDataParser = new MockEventDataParser();
            this.eventOffsetParser = new MockEventOffsetParser();
        }

        /// <summary>
        /// Tests if the MetadataFilters are being populated when the presentation model is created.
        /// </summary>
        [TestMethod]
        public void ShouldPopulateMetadataFilters()
        {
            var presentationModel = this.CreatePresentationModel();

            Assert.IsTrue(presentationModel.MetadataFilters.Count > 0);
        }

        /// <summary>
        /// Tests if tthe SelectedMetadataFilter is being set when the presentation model is created.
        /// </summary>
        [TestMethod]
        public void ShouldSetSelectedMetadataFilter()
        {
            var presentationModel = this.CreatePresentationModel();

            Assert.AreEqual(presentationModel.MetadataFilters[0], presentationModel.SelectedMetadataFilter);
        }

        /// <summary>
        /// Tests if the ParseEventData method is being called when the in stream collection being set contains items.
        /// </summary>
        [TestMethod]
        public void ShouldCallToParseEventDataOnEventDataParserIfInStreamCollectionHasItems()
        {
            var presentationModel = this.CreatePresentationModel();

            XElement element = new XElement("TestElement");
            InStreamCollection collection = new InStreamCollection { element };

            Assert.IsFalse(this.eventDataParser.ParseEventDataCalled);

            presentationModel.SetInStreamData(collection);

            Assert.IsTrue(this.eventDataParser.ParseEventDataCalled);
        }

        /// <summary>
        /// Tests if the ParseEventData method on EventOffsetParser is being called when the in stream collection being set contains items.
        /// </summary>
        [TestMethod]
        public void ShouldCallToParseEventDataOnEventOffsetParserIfInStreamCollectionHasItems()
        {
            var presentationModel = this.CreatePresentationModel();

            XElement element = new XElement("TestElement");
            InStreamCollection collection = new InStreamCollection { element };

            Assert.IsFalse(this.eventOffsetParser.ParseEventDataCalled);

            presentationModel.SetInStreamData(collection);

            Assert.IsTrue(this.eventOffsetParser.ParseEventDataCalled);
        }

        /// <summary>
        /// Tests if the ParseEvenDta method is being called when adding new items to the in stream collection.
        /// </summary>
        [TestMethod]
        public void ShouldCallToParseEventDataOnEventDataParserWhenAddingNewItemsToTheInStreamCollection()
        {
            var presentationModel = this.CreatePresentationModel();

            InStreamCollection collection = new InStreamCollection();

            presentationModel.SetInStreamData(collection);

            Assert.IsFalse(this.eventDataParser.ParseEventDataCalled);

            XElement element = new XElement("TestElement");

            collection.Add(element);

            Assert.IsTrue(this.eventDataParser.ParseEventDataCalled);
        }

        /// <summary>
        /// Tests if the ParseEvenDta method is being called when removing new items to the in stream collection.
        /// </summary>
        [TestMethod]
        public void ShouldCallToParseEventDataOnEventDataParserWhenRemovingItemsToTheInStreamCollection()
        {
            var presentationModel = this.CreatePresentationModel();

            XElement element = new XElement("TestElement");
            InStreamCollection collection = new InStreamCollection { element };

            presentationModel.SetInStreamData(collection);

            this.eventDataParser.ParseEventDataCalled = false;

            collection.Remove(element);

            Assert.IsTrue(this.eventDataParser.ParseEventDataCalled);
        }

        /// <summary>
        /// Tests if the ParseEvenData method on EventOffsetParser is being called when adding new items to the in stream collection.
        /// </summary>
        [TestMethod]
        public void ShouldCallToParseEventDataOnEventOffsetParserWhenAddingNewItemsToTheInStreamCollection()
        {
            var presentationModel = this.CreatePresentationModel();

            InStreamCollection collection = new InStreamCollection();

            presentationModel.SetInStreamData(collection);

            Assert.IsFalse(this.eventOffsetParser.ParseEventDataCalled);

            XElement element = new XElement("TestElement");

            collection.Add(element);

            Assert.IsTrue(this.eventOffsetParser.ParseEventDataCalled);
        }

        /// <summary>
        /// Tests if the ParseEvenData method on EventOffsetParser is being called when removing new items to the in stream collection.
        /// </summary>
        [TestMethod]
        public void ShouldCallToParseEventDataOnEventOffsetParserWhenRemovingItemsToTheInStreamCollection()
        {
            var presentationModel = this.CreatePresentationModel();

            XElement element = new XElement("TestElement");
            InStreamCollection collection = new InStreamCollection { element };

            presentationModel.SetInStreamData(collection);

            this.eventOffsetParser.ParseEventDataCalled = false;

            collection.Remove(element);

            Assert.IsTrue(this.eventOffsetParser.ParseEventDataCalled);
        }

        /// <summary>
        /// Tests if the Event offset is being applied to new events.
        /// </summary>
        [TestMethod] public void ShouldApplyOffsetToNewEventsBeingAdded()
        {
            var presentationModel = this.CreatePresentationModel();

            EventOffset eventOffset = new EventOffset(10);

            XElement offsetElement = new XElement("TestElement");

            this.eventOffsetParser.ParseEventDataReturn = eventOffset;

            InStreamCollection collection = new InStreamCollection { offsetElement };

            presentationModel.SetInStreamData(collection);

            this.eventOffsetParser.ParseEventDataReturn = null;

            EventData eventData = new EventData(Guid.NewGuid().ToString(), TimeSpan.FromSeconds(0), "Test");

            var actualTime = eventData.Time;

            XElement element = new XElement("TestElement");

            this.eventDataParser.ParseEventDataReturn = eventData;

            collection.Add(element);

            Assert.AreEqual(actualTime.Add(TimeSpan.FromSeconds(eventOffset.Offset)), eventData.Time);
        }

        /// <summary>
        /// Tests if the Event offset is being applied to existing events.
        /// </summary>
        [TestMethod]
        public void ShouldApplyOffsetToExistingEventsWhenOffsetIsBeingAdded()
        {
            var presentationModel = this.CreatePresentationModel();

            EventData eventData = new EventData(Guid.NewGuid().ToString(), TimeSpan.FromSeconds(0), "Test");

            var actualTime = eventData.Time;

            XElement element = new XElement("TestElement");

            this.eventDataParser.ParseEventDataReturn = eventData;

            InStreamCollection collection = new InStreamCollection { element };

            presentationModel.SetInStreamData(collection);

            this.eventDataParser.ParseEventDataReturn = null;

            EventOffset eventOffset = new EventOffset(10);

            XElement offsetElement = new XElement("TestElement");

            this.eventOffsetParser.ParseEventDataReturn = eventOffset;

            collection.Add(offsetElement);

            Assert.AreEqual(actualTime.Add(TimeSpan.FromSeconds(eventOffset.Offset)), eventData.Time);
        }

        /// <summary>
        /// Tests it the event data is being removed from the metadata collection when removing items from the in stream collection.
        /// </summary>
        [TestMethod]
        public void ShouldRemoveEventFromMetadataCollectionWhenRemovingItemsFromTheInStreamCollection()
        {
            var presentationModel = this.CreatePresentationModel();

            EventData eventData = new EventData(Guid.NewGuid().ToString(), TimeSpan.FromSeconds(0), "Test");

            XElement element = new XElement("TestElement");

            this.eventDataParser.ParseEventDataReturn = eventData;

            InStreamCollection collection = new InStreamCollection { element };

            presentationModel.SetInStreamData(collection);

            Assert.AreEqual(1, presentationModel.Metadata.Count);

            collection.Remove(element);

            Assert.AreEqual(0, presentationModel.Metadata.Count);
        }

        /// <summary>
        /// Tests if the parsed events are available for search.
        /// </summary>
        [TestMethod]
        public void ShouldSearchWithinParsedEvents()
        {
            var presentationModel = this.CreatePresentationModel();

            XElement element = new XElement("TestElement");

            this.eventDataParser.ParseEventDataReturn = new EventData(Guid.NewGuid().ToString(), TimeSpan.FromSeconds(0), "Test");

            InStreamCollection collection = new InStreamCollection { element };

            presentationModel.SetInStreamData(collection);

            this.eventDataParser.ParseEventDataReturn = new EventData(Guid.NewGuid().ToString(), TimeSpan.FromSeconds(0), "Mock");

            collection.Add(element);

            Assert.AreEqual(2, presentationModel.Metadata.Count);

            presentationModel.SearchCommand.Execute("Test");

            Assert.AreEqual(1, presentationModel.Metadata.Count);

            presentationModel.SearchCommand.Execute(String.Empty);

            Assert.AreEqual(2, presentationModel.Metadata.Count);
        }

        /// <summary>
        /// Creates the <see cref="MetadataViewPresentationModel"/>.
        /// </summary>
        /// <returns>The <see cref="MetadataViewPresentationModel"/>.</returns>
        private MetadataViewPresentationModel CreatePresentationModel()
        {
            return new MetadataViewPresentationModel(this.configurationService, this.eventDataParser, this.eventOffsetParser);
        }
    }
}
