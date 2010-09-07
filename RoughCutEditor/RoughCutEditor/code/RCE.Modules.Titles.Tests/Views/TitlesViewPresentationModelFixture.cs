// <copyright file="TitlesViewPresentationModelFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TitlesViewPresentationModelFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles.Tests.Views
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using RCE.Infrastructure;

    /// <summary>
    /// Test class for <see cref="TitlesViewPresentationModel"/>.
    /// </summary>
    [TestClass]
    public class TitlesViewPresentationModelFixture
    {
        /// <summary>
        /// Mock for <see cref="TitlesView"/>.
        /// </summary>
        private MockTitlesView view;

        /// <summary>
        /// Mock for IEventAggregator.
        /// </summary>
        private MockEventAggregator eventAggregator;

        /// <summary>
        /// Mock for <see cref="AddAssetToTimelineEvent"/>.
        /// </summary>
        private MockAddAssetToTimelineEvent addAssetToTimelineEvent;

        /// <summary>
        /// Mock for <see cref="IDataServiceFacade"/>.
        /// </summary>
        private MockDataServiceFacade serviceFacade;

        /// <summary>
        /// Mock for <see cref="IConfigurationService"/>.
        /// </summary>
        private MockConfigurationService configurationService;

        /// <summary>
        /// Mock for IRegionManger.
        /// </summary>
        private MockRegionManager regionManager;

        /// <summary>
        /// Mock for <see cref="Downloader"/>.
        /// </summary>
        private MockDownloader downloader;

        /// <summary>
        /// Mock for IRegion.
        /// </summary>
        private MockRegion toolsRegion;

        /// <summary>
        /// Initializes the data in test class.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.view = new MockTitlesView();
            this.eventAggregator = new MockEventAggregator();
            this.addAssetToTimelineEvent = new MockAddAssetToTimelineEvent();
            this.serviceFacade = new MockDataServiceFacade();
            this.configurationService = new MockConfigurationService();
            this.eventAggregator.AddMapping<AddAssetToTimelineEvent>(this.addAssetToTimelineEvent);
            this.regionManager = new MockRegionManager();
            this.downloader = new MockDownloader();
            this.toolsRegion = new MockRegion();

            this.toolsRegion.Name = RegionNames.ToolsRegion;
            this.regionManager.Regions.Add(this.toolsRegion);
        }

        /// <summary>
        /// Tests if <see cref="TitlesViewPresentationModel"/>
        /// is initilizing the <see cref="TitlePreview"/>.
        /// </summary>
        [TestMethod]
        public void CanInitPresentationModel()
        {
            var presentationModel = this.CreatePresentationModel();

            Assert.AreEqual(this.view, presentationModel.View);
        }

        /// <summary>
        /// Should set presentation model into view.
        /// </summary>
        [TestMethod]
        public void ShouldSetPresentationModelIntoView()
        {
            var presentationModel = this.CreatePresentationModel();

            Assert.AreSame(presentationModel, this.view.Model);
        }

        /// <summary>
        /// Should call to lLoadTitleTemplatesAsync on data service facade while initilizing.
        /// </summary>
        [TestMethod]
        public void ShouldCallToLoadTitleTemplatesAsyncOnDataServiceFacade()
        {
            Assert.IsFalse(this.serviceFacade.LoadTitleTemplatesAsyncCalled);

            var presentationModel = this.CreatePresentationModel();

            Assert.IsTrue(this.serviceFacade.LoadTitleTemplatesAsyncCalled);
        }

        /// <summary>
        /// Should download xaml resource and add the title template on collection.
        /// </summary>
        [TestMethod]
        public void ShouldDownloadXamlResourceAndAddTheTitleTemplateOnCollection()
        {
            var titleTemplate = new TitleTemplate { Title = "Test" };
            this.configurationService.GetParameterValueReturnFunction = parameter => parameter == "TitleTemplates" ? "Test|http://test" : null;

            this.downloader.Result = "<canvas></canvas>";
            this.downloader.UserState = titleTemplate;
            
            this.serviceFacade.TitleTemplates = new List<TitleTemplate> { titleTemplate };

            var presentationModel = this.CreatePresentationModel();
            
            Assert.AreEqual(1, presentationModel.TitleTemplates.Count);
            Assert.AreEqual(this.downloader.Result, titleTemplate.XamlResource);
        }

        /// <summary>
        /// Should not call to GetTemplateName on configuration service if there are no title templates.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallToGetTemplateNameOnConfigurationServiceIfThereAreNoTitleTemplates()
        {
            bool getTitleTemplateCalled = false;
            this.configurationService.GetParameterValueReturnFunction = parameter =>
            {
                if (parameter == "TitleTemplates")
                {
                    getTitleTemplateCalled = true;
                }

                return string.Empty;
            };

            var presentationModel = this.CreatePresentationModel();

            Assert.IsFalse(getTitleTemplateCalled);
        }

        /// <summary>
        /// Should call to GetTemplateName on configuration service if there are title templates.
        /// </summary>
        [TestMethod]
        public void ShouldCallToGetTemplateNameOnConfigurationServiceIfThereAreTitleTemplates()
        {
            this.serviceFacade.TitleTemplates.Add(new TitleTemplate { Title = "Test" });

            bool getTitleTemplateCalled = false;
            this.configurationService.GetParameterValueReturnFunction = parameter =>
            {
                if (parameter == "TitleTemplates")
                {
                    getTitleTemplateCalled = true;
                    return "Test|http://test";
                }

                return string.Empty;
            };

            var presentationModel = this.CreatePresentationModel();

            Assert.IsTrue(getTitleTemplateCalled);
        }

        /// <summary>
        /// Should raise OnPropertyChanged event when MainTitle is updated.
        /// </summary>
        [TestMethod]
        public void ShouldRaiseOnPropertyChangedEventWhenMainTitleIsUpdated()
        {
            var propertyChangedRaised = false;
            string propertyChangedEventArgsArgument = null;

            var presentationModel = this.CreatePresentationModel();
            presentationModel.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyChangedEventArgsArgument = e.PropertyName;
            };

            Assert.IsFalse(propertyChangedRaised);
            Assert.IsNull(propertyChangedEventArgsArgument);

            presentationModel.MainTitle = "text";

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual("MainTitle", propertyChangedEventArgsArgument);
        }

        /// <summary>
        /// Should raise OnPropertyChanged event when SubTitle is updated.
        /// </summary>
        [TestMethod]
        public void ShouldRaiseOnPropertyChangedEventWhenSubTitleIsUpdated()
        {
            var propertyChangedRaised = false;
            string propertyChangedEventArgsArgument = null;

            var presentationModel = this.CreatePresentationModel();
            presentationModel.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyChangedEventArgsArgument = e.PropertyName;
            };

            Assert.IsFalse(propertyChangedRaised);
            Assert.IsNull(propertyChangedEventArgsArgument);

            presentationModel.SubTitle = "text";

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual("SubTitle", propertyChangedEventArgsArgument);
        }

        /// <summary>
        /// Should update main text of titles when MainTitle is updated.
        /// </summary>
        [TestMethod]
        public void ShouldUpdateMainTextOfTitlesWhenMainTitleIsUpdated()
        {
            var presentationModel = this.CreatePresentationModel();
            presentationModel.TitleTemplates.Clear();
            presentationModel.TitleTemplates.Add(new TitleTemplate { MainText = "Main Text" });

            presentationModel.MainTitle = "Another Main Text";

            Assert.AreEqual(presentationModel.MainTitle, presentationModel.TitleTemplates[0].MainText);
        }

        /// <summary>
        /// Should update sub text of titles when SubTitle is updated.
        /// </summary>
        [TestMethod]
        public void ShouldUpdateSubTextOfTitlesWhenSubTitleIsUpdated()
        {
            var presentationModel = this.CreatePresentationModel();
            presentationModel.TitleTemplates.Clear();
            presentationModel.TitleTemplates.Add(new TitleTemplate { SubText = "Main Text" });

            presentationModel.SubTitle = "Another Main Text";

            Assert.AreEqual(presentationModel.SubTitle, presentationModel.TitleTemplates[0].SubText);
        }

        /// <summary>
        /// Should publish AddTitleAssetToTimeline while calling AddTitleAssetToTimeline.
        /// </summary>
        [TestMethod]
        public void ShouldPublishAddAssetToTimelineEvent()
        {
            var titleTemplate = new TitleTemplate
                                    {
                                        MainText = "MainText",
                                        SubText = "SubText"
                                    };

            var presentationModel = this.CreatePresentationModel();
            
            Assert.IsFalse(this.addAssetToTimelineEvent.PublishCalled);

            presentationModel.AddTitleAssetToTimeline(titleTemplate);

            Assert.IsTrue(this.addAssetToTimelineEvent.PublishCalled);
            Assert.IsInstanceOfType(this.addAssetToTimelineEvent.Asset, typeof(TitleAsset));
            Assert.AreSame(titleTemplate, ((TitleAsset)this.addAssetToTimelineEvent.Asset).TitleTemplate);
        }

        /// <summary>
        /// Should return true if the view is active.
        /// </summary>
        [TestMethod]
        public void ShouldReturnTrueIfTheViewIsActive()
        {
            var toolsRegion = new MockRegion { Name = "ToolsRegion" };

            this.regionManager.Regions.Add(toolsRegion);
            var presentationModel = this.CreatePresentationModel();

            var activeViews = new MockViewsCollection { presentationModel.View };
            toolsRegion.ActiveViews = activeViews;

            Assert.IsTrue(presentationModel.IsActive);
        }

        /// <summary>
        /// Should return false if the view is not active.
        /// </summary>
        [TestMethod]
        public void ShouldReturnFalseIfTheViewIsNotActive()
        {
            var toolsRegion = new MockRegion { Name = "ToolsRegion" };

            this.regionManager.Regions.Add(toolsRegion);
            var presentationModel = this.CreatePresentationModel();

            var activeViews = new MockViewsCollection();
            toolsRegion.ActiveViews = activeViews;

            Assert.IsFalse(presentationModel.IsActive);
        }

        /// <summary>
        /// Tests if the HeaderInfo property returns the expected value.
        /// </summary>
        [TestMethod]
        public void ShouldReturnHeaderInfoResource()
        {
            var presenter = this.CreatePresentationModel();

            var result = presenter.HeaderInfo;

            Assert.AreEqual(Resources.Resources.HeaderInfo, result);
        }

        /// <summary>
        /// Tests if the HeaderIconOff property returns the expected value.
        /// </summary>
        [TestMethod]
        public void ShouldReturnHeaderIconOffResource()
        {
            var presenter = this.CreatePresentationModel();

            var result = presenter.HeaderIconOff;

            Assert.AreEqual("/RCE.Modules.Titles;component/images/icon_off.png", result);
        }

        /// <summary>
        /// Tests if the HeaderIconOn property returns the expected value.
        /// </summary>
        [TestMethod]
        public void ShouldReturnHeaderIconOnResource()
        {
            var presenter = this.CreatePresentationModel();

            var result = presenter.HeaderIconOn;

            Assert.AreEqual("/RCE.Modules.Titles;component/images/icon_on.png", result);
        }

        /// <summary>
        /// Tests if the Activate of Region is called when Activate method is called.
        /// </summary>
        [TestMethod]
        public void ShouldActivateTheViewIfActivateIsCalled()
        {
            this.toolsRegion.SelectedItem = null;
            var presenter = this.CreatePresentationModel();

            presenter.Activate();

            Assert.AreSame(this.view, this.toolsRegion.SelectedItem);
        }

        /// <summary>
        /// Creates the presentation model.
        /// </summary>
        /// <returns>The <see cref="ITitlesViewPresentationModel"/>.</returns>
        private ITitlesViewPresentationModel CreatePresentationModel()
        {
            return new TitlesViewPresentationModel(this.view, this.eventAggregator, this.serviceFacade, this.configurationService, this.regionManager, this.downloader);
        }
    }
}
