﻿// <copyright file="MetadataViewPresentationModelFixture.cs" company="Microsoft Corporation">
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

namespace RCE.Modules.Metadata.Tests.Views
{
    using System.Collections.Generic;
    using Microsoft.Practices.Composite.Presentation.Events;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using RCE.Infrastructure.Events;
    using RCE.Infrastructure.Models;

    /// <summary>
    /// A class for testing the <see cref="MetadataViewPresentationModel"/>.
    /// </summary>
    [TestClass]
    public class MetadataViewPresentationModelFixture
    {
        /// <summary>
        /// The mocked MetadataView.
        /// </summary>
        private MockMetadataView view;

        /// <summary>
        /// The mocked ConfigurationService.
        /// </summary>
        private MockConfigurationService configurationService;

        /// <summary>
        /// The mocked EventAggregator.
        /// </summary>
        private MockEventAggregator eventAggregator;

        /// <summary>
        /// The mocked ShowMetadataEvent.
        /// </summary>
        private MockShowMetadataEvent showMetadataEvent;

        /// <summary>
        /// The mocked HideMetadataEvent.
        /// </summary>
        private MockHideMetadataEvent hideMetadataEvent;

        /// <summary>
        /// Initializes the data for the tests.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.view = new MockMetadataView();
            this.showMetadataEvent = new MockShowMetadataEvent();
            this.hideMetadataEvent = new MockHideMetadataEvent();

            this.configurationService = new MockConfigurationService();
            this.eventAggregator = new MockEventAggregator();

            this.eventAggregator.AddMapping<ShowMetadataEvent>(this.showMetadataEvent);
            this.eventAggregator.AddMapping<HideMetadataEvent>(this.hideMetadataEvent);
        }

        /// <summary>
        /// Tests if the <see cref="MetadataViewPresentationModel"/> can be initialized.
        /// </summary>
        [TestMethod]
        public void CanInitPresentationModel()
        {
            var presentationModel = this.CreateMetadataViewPresentationModel();

            Assert.AreEqual(this.view, presentationModel.View);
        }

        /// <summary>
        /// Tests if the <see cref="MetadataViewPresentationModel"/> is being set on the view. 
        /// </summary>
        [TestMethod]
        public void ShouldSetPresenterIntoView()
        {
            var presentationModel = this.CreateMetadataViewPresentationModel();

            Assert.AreEqual(presentationModel, this.view.Model);
        }

        /// <summary>
        /// Tests if the <see cref="MetadataViewPresentationModel"/> subscribed to the <see cref="ShowMetadataEvent"/>.
        /// </summary>
        [TestMethod]
        public void ShouldSubscribeToShowMetadataEvent()
        {
            Assert.IsNull(this.showMetadataEvent.SubscribeArgumentAction);

            var presentationModel = this.CreateMetadataViewPresentationModel();

            Assert.IsNotNull(this.showMetadataEvent.SubscribeArgumentAction);
            Assert.AreEqual(ThreadOption.PublisherThread, this.showMetadataEvent.SubscribeArgumentThreadOption);
        }

        /// <summary>
        /// Tests if the <see cref="MetadataViewPresentationModel"/> subscribed to the <see cref="HideMetadataEvent"/>.
        /// </summary>
        [TestMethod]
        public void ShouldSubscribeToHideMetadataEvent()
        {
            Assert.IsNull(this.hideMetadataEvent.SubscribeArgumentAction);

            var presentationModel = this.CreateMetadataViewPresentationModel();

            Assert.IsNotNull(this.hideMetadataEvent.SubscribeArgumentAction);
            Assert.AreEqual(ThreadOption.PublisherThread, this.hideMetadataEvent.SubscribeArgumentThreadOption);
        }

        /// <summary>
        /// Tests if the PropertyChanged event is being raised when the ShowMetadataInformation property change.
        /// </summary>
        [TestMethod]
        public void ShouldRaiseOnPropertyChangedEventWhenShowMetadataInformationIsUpdated()
        {
            var propertyChangedRaised = false;
            string propertyChangedEventArgsArgument = null;

            var presentationModel = this.CreateMetadataViewPresentationModel();
            presentationModel.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyChangedEventArgsArgument = e.PropertyName;
            };

            Assert.IsFalse(propertyChangedRaised);
            Assert.IsNull(propertyChangedEventArgsArgument);

            presentationModel.ShowMetadataInformation = true;

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual("ShowMetadataInformation", propertyChangedEventArgsArgument);
        }

        /// <summary>
        /// Should set ShowMetadata to false when <see cref="HideMetadataEvent"/> event is published.
        /// </summary>
        [TestMethod]
        public void ShouldSetShowMetadataToFalseWhenHideMetadataEventIsPublished()
        {
            var presentationModel = this.CreateMetadataViewPresentationModel();

            Assert.IsFalse(presentationModel.ShowMetadataInformation);

            presentationModel.ShowMetadataInformation = true;
            
            this.hideMetadataEvent.SubscribeArgumentAction(null);

            Assert.IsFalse(presentationModel.ShowMetadataInformation);
        }

        /// <summary>
        /// Should check if ShowMetdata information is set to false by default.
        /// </summary>
        [TestMethod]
        public void ShouldCheckIfShowMetdataInformationIsSetToFalseByDefault()
        {
            var presentationModel = this.CreateMetadataViewPresentationModel();
            Assert.IsFalse(presentationModel.ShowMetadataInformation);
        }

        /// <summary>
        /// Should set ShowMetadata to true when <see cref="ShowMetadataEvent"/>
        /// event is published.
        /// </summary>
        [TestMethod]
        public void ShouldSetShowMetadataToTrueWhenShowMetadataEventIsPublished()
        {
            var presentationModel = this.CreateMetadataViewPresentationModel();

            Asset asset = new VideoAsset { Title = "Test Video" };

            presentationModel.ShowMetadataInformation = false;

            this.showMetadataEvent.SubscribeArgumentAction(asset);

            Assert.IsTrue(presentationModel.ShowMetadataInformation);
        }

        /// <summary>
        /// Should read metadata and populate asset metadata details.
        /// </summary>
        [TestMethod]
        public void ShouldReadMetadataAndPopulateAssetMetadataDetails()
        {
            this.configurationService.GetParameterValueReturnFunction = parameter =>
                                                                            {
                                                                                if (parameter == "MetadataFields")
                                                                                {
                                                                                    return "Title;Duration;FrameRate";
                                                                                }

                                                                                return null;
                                                                            };

            var presentationModel = this.CreateMetadataViewPresentationModel();

            Asset asset = new VideoAsset { Title = "Test Video" };

            Assert.IsNull(presentationModel.AssetMetadataDetails);

            this.showMetadataEvent.SubscribeArgumentAction(asset);

            Assert.AreEqual(3, presentationModel.AssetMetadataDetails.Count);
        }

        /// <summary>
        /// Creates the metadata view presentation model.
        /// </summary>
        /// <returns>The <see cref="MetadataViewPresentationModel"/>.</returns>
        private MetadataViewPresentationModel CreateMetadataViewPresentationModel()
        {
            return new MetadataViewPresentationModel(this.view, this.configurationService, this.eventAggregator);
        }
    }
}