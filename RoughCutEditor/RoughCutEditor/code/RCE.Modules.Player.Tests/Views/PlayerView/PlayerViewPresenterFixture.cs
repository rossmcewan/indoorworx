// <copyright file="PlayerViewPresenterFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayerViewPresenterFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player.Tests.Views
{
    using System;
    using System.Windows.Media.Imaging;
    using Infrastructure.DragDrop;
    using Infrastructure.Events;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Presentation.Events;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Services.Contracts;
    using SMPTETimecode;
    using Comment = RCE.Infrastructure.Models.Comment;
    using Track = RCE.Infrastructure.Models.Track;
    using TrackType = RCE.Infrastructure.Models.TrackType;

    /// <summary>
    /// Test class for <see cref="PlayerViewPresenter"/>.
    /// </summary>
    [TestClass]
    public class PlayerViewPresenterFixture
    {
        /// <summary>
        /// Mock for <see cref="PlayerView"/>.
        /// </summary>
        private MockPlayerView view;

        /// <summary>
        /// Mock for <see cref="Microsoft.Practices.Composite.Events.IEventAggregator"/>.
        /// </summary>
        private MockEventAggregator eventAggregator;

        /// <summary>
        /// Mock for <see cref="TimelineModel"/>.
        /// </summary>
        private MockTimelineModel timelineModel;

        /// <summary>
        /// Mock for <see cref="KeyMappingEvent"/>.
        /// </summary>
        private MockKeyMappingEvent keyMappingEvent;

        /// <summary>
        /// Mock for <see cref="FullScreenEvent"/>.
        /// </summary>
        private MockFullScreenEvent fullScreenEvent;

        /// <summary>
        /// Mock for <see cref="PlayheadMovedEvent"/>.
        /// </summary>
        private MockPlayheadMovedEvent playheadMovedEvent;

        /// <summary>
        /// Mock for <see cref="PositionDoubleClickedEvent"/>.
        /// </summary>
        private MockPositionDoubleClickedEvent addCommentEvent;

        /// <summary>
        /// Mock for <see cref="SmpteTimeCodeChangedEvent"/>.
        /// </summary>
        private MockSmpteTimecodeChangedEvent smpteTimeCodeChangedEvent;

        /// <summary>
        /// Mock for <see cref="AspectRatioChangedEvent"/>.
        /// </summary>
        private MockAspectRatioChangedEvent aspectRatioChangedEvent;

        /// <summary>
        /// Mock for <see cref="PositionUpdatedEvent"/>.
        /// </summary>
        private MockPositionUpdatedEvent positionUpdatedEvent;

        /// <summary>
        /// Mock for <see cref="DownloadProgressChangedEvent"/>.
        /// </summary>
        private MockDownloadProgressChangedEvent downloadProgressChangedEvent;

        /// <summary>
        /// Mock for <see cref="PauseEvent"/>.
        /// </summary>
        private MockPauseEvent pauseEvent;

        /// <summary>
        /// Mock for <see cref="PlayerEvent"/>.
        /// </summary>
        private MockPlayerEvent playerEvent;

        /// <summary>
        /// Mock for visual <see cref="AggregateMediaModel"/>.
        /// </summary>
        private MockAggregateMediaModel visualMediaModel;

        /// <summary>
        /// Mock for audio <see cref="AggregateMediaModel"/>.
        /// </summary>
        private MockAggregateMediaModel audioMediaModel;

        /// <summary>
        /// Mock for title <see cref="AggregateMediaModel"/>.
        /// </summary>
        private MockAggregateMediaModel titleMediaModel;

        /// <summary>
        /// Mock for <see cref="PlayCommentEvent"/>.
        /// </summary>
        private MockPlayCommentEvent playCommentEvent;

        /// <summary>
        /// Mock for <see cref="HideMetadataEvent"/>.
        /// </summary>
        private MockHideMetadataEvent hideMetadataEvent;

        /// <summary>
        /// Mock for <see cref="ShowMetadataEvent"/>.
        /// </summary>
        private MockShowMetadataEvent showMetadataEvent;

        /// <summary>
        /// Mock for <see cref="PickThumbnailEvent"/>
        /// </summary>
        private MockPickThumbnailEvent pickThumbnailEvent;

        /// <summary>
        /// Mock for <see cref="thumbnailEvent"/>.
        /// </summary>
        private MockThumbnailEvent thumbnailEvent;

        /// <summary>
        /// Initializes the test class.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.view = new MockPlayerView();
            this.eventAggregator = new MockEventAggregator();
            this.timelineModel = new MockTimelineModel();
            this.keyMappingEvent = new MockKeyMappingEvent();
            this.fullScreenEvent = new MockFullScreenEvent();
            this.playheadMovedEvent = new MockPlayheadMovedEvent();
            this.pauseEvent = new MockPauseEvent();
            this.addCommentEvent = new MockPositionDoubleClickedEvent();
            this.positionUpdatedEvent = new MockPositionUpdatedEvent();
            this.downloadProgressChangedEvent = new MockDownloadProgressChangedEvent();
            this.playerEvent = new MockPlayerEvent();
            this.visualMediaModel = new MockAggregateMediaModel();
            this.audioMediaModel = new MockAggregateMediaModel();
            this.titleMediaModel = new MockAggregateMediaModel();
            this.playCommentEvent = new MockPlayCommentEvent();
            this.hideMetadataEvent = new MockHideMetadataEvent();
            this.showMetadataEvent = new MockShowMetadataEvent();

            this.aspectRatioChangedEvent = new MockAspectRatioChangedEvent();
            this.smpteTimeCodeChangedEvent = new MockSmpteTimecodeChangedEvent();
            this.pickThumbnailEvent = new MockPickThumbnailEvent();
            this.thumbnailEvent = new MockThumbnailEvent();

            this.eventAggregator.AddMapping<KeyMappingEvent>(this.keyMappingEvent);
            this.eventAggregator.AddMapping<FullScreenEvent>(this.fullScreenEvent);
            this.eventAggregator.AddMapping<SmpteTimeCodeChangedEvent>(this.smpteTimeCodeChangedEvent);
            this.eventAggregator.AddMapping<PlayheadMovedEvent>(this.playheadMovedEvent);
            this.eventAggregator.AddMapping<AspectRatioChangedEvent>(this.aspectRatioChangedEvent);
            this.eventAggregator.AddMapping<PauseEvent>(this.pauseEvent);
            this.eventAggregator.AddMapping<PositionUpdatedEvent>(this.positionUpdatedEvent);
            this.eventAggregator.AddMapping<DownloadProgressChangedEvent>(this.downloadProgressChangedEvent);
            this.eventAggregator.AddMapping<PlayerEvent>(this.playerEvent);
            this.eventAggregator.AddMapping<PlayCommentEvent>(this.playCommentEvent);
            this.eventAggregator.AddMapping<PositionDoubleClickedEvent>(this.addCommentEvent);
            this.eventAggregator.AddMapping<HideMetadataEvent>(this.hideMetadataEvent);
            this.eventAggregator.AddMapping<ShowMetadataEvent>(this.showMetadataEvent);
            this.eventAggregator.AddMapping<PickThumbnailEvent>(this.pickThumbnailEvent);
            this.eventAggregator.AddMapping<ThumbnailEvent>(this.thumbnailEvent);
        }

        /// <summary>
        /// Determines whether this instance inits the view.
        /// </summary>
        [TestMethod]
        public void CanInitPresenter()
        {
            var presenter = this.CreatePresenter();

            Assert.AreEqual(this.view, presenter.View);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls TogglePlay when <see cref="KeyMappingEvent"/>
        /// is published and <see cref="PlayerMode"/> is MediaBin.
        /// </summary>
        [TestMethod]
        public void ShouldCallTogglePlayWhenKeyMappingEventIsTriggerdAndPlayerModeIsMediaBin()
        {
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentFilter);

            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaBin;
            Assert.IsFalse(this.view.TogglePlayCalled);

            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentFilter);
            Assert.AreEqual(ThreadOption.PublisherThread, this.keyMappingEvent.SubscribeArgumentThreadOption);

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.Toggle);

            Assert.IsTrue(this.view.TogglePlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls TogglePlay when <see cref="KeyMappingEvent"/>
        /// is published and <see cref="PlayerMode"/> is Library.
        /// </summary>
        [TestMethod]
        public void ShouldCallTogglePlayWhenKeyMappingEventIsTriggerdAndPlayerModeIsMediaLibrary()
        {
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentFilter);

            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaLibrary;
            Assert.IsFalse(this.view.TogglePlayCalled);

            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentFilter);
            Assert.AreEqual(ThreadOption.PublisherThread, this.keyMappingEvent.SubscribeArgumentThreadOption);

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.Toggle);

            Assert.IsTrue(this.view.TogglePlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> doesn't call TogglePlay when <see cref="KeyMappingEvent"/>
        /// is published and <see cref="PlayerMode"/> is Timeline.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallTogglePlayWhenKeyMappingEventIsTriggerdAndPlayerModeIsTimeline()
        {
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentFilter);

            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            Assert.IsFalse(this.view.TogglePlayCalled);

            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentFilter);
            Assert.AreEqual(ThreadOption.PublisherThread, this.keyMappingEvent.SubscribeArgumentThreadOption);

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.Toggle);

            Assert.IsFalse(this.view.TogglePlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls TogglePlay method when
        /// <see cref="KeyMappingEvent"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldCallPlayWithKeyMappingEventSubscription()
        {
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentFilter);

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.TogglePlayCalled);

            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentFilter);
            Assert.AreEqual(ThreadOption.PublisherThread, this.keyMappingEvent.SubscribeArgumentThreadOption);

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.Toggle);

            Assert.IsTrue(this.view.TogglePlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> set the playermode to timeline when 
        /// <see cref="KeyMappingEvent"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldSetPlayerModeToTimeLineIfKeyMappingActionIsPlayTimelineWithKeyMappingEventSubscription()
        {
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNull(this.keyMappingEvent.SubscribeArgumentFilter);

            var presenter = this.CreatePresenter();

            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentAction);
            Assert.IsNotNull(this.keyMappingEvent.SubscribeArgumentFilter);
            Assert.AreEqual(ThreadOption.PublisherThread, this.keyMappingEvent.SubscribeArgumentThreadOption);

            Assert.AreNotEqual(PlayerMode.Timeline, presenter.PlayerMode);

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.PlayTimeline);

            Assert.AreEqual(PlayerMode.Timeline, presenter.PlayerMode);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> publishes <see cref="FullScreenEvent"/>
        /// when FullScreenModeEvent is triggered.
        /// </summary>
        [TestMethod]
        public void ShouldPublishEventWhenInvokingFullScreenChangedEvent()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.fullScreenEvent.PublishCalled);

            this.view.InvokeFullScreenChanged(FullScreenMode.Player);

            Assert.IsTrue(this.fullScreenEvent.PublishCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> subscribes to the <see cref="PlayCommentEvent"/>.
        /// </summary>
        [TestMethod]
        public void ShouldSubscribeToPlayCommentEvent()
        {
            this.playCommentEvent.SubscribeArgumentAction = null;
            this.playCommentEvent.SubscribeArgumentFilter = null;
            
            var preseter = this.CreatePresenter();

            Assert.IsNotNull(this.playCommentEvent.SubscribeArgumentAction);
            Assert.IsNotNull(this.playCommentEvent.SubscribeArgumentFilter);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> doesn't execute the 
        /// DropCommand if DraggedItem is null.
        /// </summary>
        [TestMethod]
        public void ShouldNotCanExecuteDropCommandIfDraggedItemIsNull()
        {
            var presenter = this.CreatePresenter();

            var payload = new DropPayload
                              {
                                  DraggedItem = null
                              };
            
            var result = presenter.DropCommand.CanExecute(payload);
            
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Should update smpte frame rate when <see cref="SmpteTimeCodeChangedEvent"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldUpdateSmpteFrameRateWithSmpteTimecodeChangedEventSubscription()
        {
            var frameRate = SmpteFrameRate.Smpte25;

            var presenter = this.CreatePresenter();

            this.view.SetCurrentSmpteFrameRateCalled = false;

            Assert.IsFalse(this.view.SetCurrentSmpteFrameRateCalled);

            this.smpteTimeCodeChangedEvent.SubscribeArgumentAction(frameRate);

            Assert.IsTrue(this.view.SetCurrentSmpteFrameRateCalled);
            Assert.AreEqual(frameRate, this.view.SetCurrentSmpteFrameRateArgument);
        }

        /// <summary>
        /// Should update AspectRatio rate when <see cref="AspectRatioChangedEvent"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldUpdateAspectRatioWithAspectRatioChangedEventSubscription()
        {
            var aspectRatio = AspectRatio.Wide;

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.SetAspectRatioCalled);

            this.aspectRatioChangedEvent.SubscribeArgumentAction(aspectRatio);

            Assert.IsTrue(this.view.SetAspectRatioCalled);
            Assert.AreEqual(aspectRatio, this.view.SetCurrentAspectRatio);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls Play of <see cref="AggregateMediaModel"/>
        /// when PlayClicked event is triggered from the <see cref="PlayerView"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPlayOnAggregateMediaModelsWhenPlayClickedEventIsInvokedAndPlayerModeIsTimeline()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;

            Assert.IsFalse(this.visualMediaModel.PlayCalled);
            Assert.IsFalse(this.audioMediaModel.PlayCalled);
            Assert.IsFalse(this.titleMediaModel.PlayCalled);

            this.view.InvokePlayClicked();

            Assert.IsTrue(this.visualMediaModel.PlayCalled);
            Assert.IsTrue(this.audioMediaModel.PlayCalled);
            Assert.IsTrue(this.titleMediaModel.PlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls Pause of <see cref="AggregateMediaModel"/>
        /// when PauseClicked event is triggered from the <see cref="PlayerView"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPauseOnAggregateMediaModelsWhenPauseClickedEventIsInvokedAndPlayerModeIsTimeline()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            
            this.visualMediaModel.IsPlaying = true;
            this.audioMediaModel.IsPlaying = true;
            this.visualMediaModel.IsPlaying = true;

            Assert.IsFalse(this.visualMediaModel.PauseCalled);
            Assert.IsFalse(this.audioMediaModel.PauseCalled);
            Assert.IsFalse(this.titleMediaModel.PauseCalled);

            this.view.InvokePauseClicked();

            Assert.IsTrue(this.visualMediaModel.PauseCalled);
            Assert.IsTrue(this.audioMediaModel.PauseCalled);
            Assert.IsTrue(this.titleMediaModel.PauseCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls TogglePlay of <see cref="PlayerView"/>
        /// when PlayClicked event is triggered from the <see cref="PlayerView"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallToTogglePlayWhenPlayClickedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaLibrary;

            Assert.IsFalse(this.view.TogglePlayCalled);

            this.view.InvokePlayClicked();

            Assert.IsTrue(this.view.TogglePlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls TogglePlay of <see cref="PlayerView"/>
        /// when PauseClicked event is triggered from the <see cref="PlayerView"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallToTogglePlayWhenPauseClickedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaLibrary;

            Assert.IsFalse(this.view.TogglePlayCalled);

            this.view.InvokePauseClicked();

            Assert.IsTrue(this.view.TogglePlayCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls AddElement and AddBlankElement of 
        /// <see cref="AggregateMediaModel"/> when <see cref="VideoAsset"/> is added.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementAndAddBlankOnAggregateMediaModelWhenVideoAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.AddElementCalled);
            Assert.IsFalse(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new VideoAsset() });

            Assert.IsTrue(this.visualMediaModel.AddElementCalled);
            Assert.IsTrue(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls AddElement and AddBlankElement of 
        /// <see cref="AggregateMediaModel"/> when <see cref="AudioAsset"/> is added.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementAndAddBlankOnAggregateMediaModelWhenAudioAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.AddElementCalled);
            Assert.IsFalse(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new AudioAsset() });

            Assert.IsTrue(this.audioMediaModel.AddElementCalled);
            Assert.IsTrue(this.audioMediaModel.AddBlankCalled);
            Assert.IsFalse(this.visualMediaModel.AddElementCalled);
            Assert.IsFalse(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls AddElement and AddBlankElement of 
        /// <see cref="AggregateMediaModel"/> when <see cref="ImageAsset"/> is added.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementAndAddBlankOnAggregateMediaModelWhenImageAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.AddElementCalled);
            Assert.IsFalse(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new ImageAsset() });

            Assert.IsTrue(this.visualMediaModel.AddElementCalled);
            Assert.IsTrue(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls AddElement and AddBlankElement of 
        /// <see cref="AggregateMediaModel"/> when <see cref="TitleAsset"/> is added.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementAndAddBlankOnAggregateMediaModelWhenTitleAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.titleMediaModel.AddElementCalled);
            Assert.IsFalse(this.titleMediaModel.AddBlankCalled);
            Assert.IsFalse(this.visualMediaModel.AddElementCalled);
            Assert.IsFalse(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
            
            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new TitleAsset() });

            Assert.IsTrue(this.titleMediaModel.AddElementCalled);
            Assert.IsTrue(this.titleMediaModel.AddBlankCalled);
            Assert.IsFalse(this.visualMediaModel.AddElementCalled);
            Assert.IsFalse(this.visualMediaModel.AddBlankCalled);
            Assert.IsFalse(this.audioMediaModel.AddElementCalled);
            Assert.IsFalse(this.audioMediaModel.AddBlankCalled);
        }

        /// <summary>
        /// Tests that AddElement of <see cref="PlayerView"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementOnViewWhenVideoAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.AddElementCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new VideoAsset() });

            Assert.IsTrue(this.view.AddElementCalled);
        }

        /// <summary>
        /// Tests that AddElement of <see cref="PlayerView"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementOnViewWhenAudioAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.AddElementCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new AudioAsset() });

            Assert.IsTrue(this.view.AddElementCalled);
        }

        /// <summary>
        /// Tests that AddElement of <see cref="PlayerView"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementOnViewWhenImageAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.AddElementCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new ImageAsset() });

            Assert.IsTrue(this.view.AddElementCalled);
        }

        /// <summary>
        /// Tests that AddElement of <see cref="PlayerView"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallAddElementOnViewWhenTitleAssetElementAddedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.AddElementCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement { Asset = new TitleAsset() });

            Assert.IsTrue(this.view.AddElementCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenVideoAssetElementAddedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });
            
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.Tracks[0].Shots.Add(timelineElement);
            this.timelineModel.InvokeElementAdded(timelineElement);

            Assert.IsTrue(this.visualMediaModel.ReorderElementsCalled);
            Assert.AreEqual(this.timelineModel.Tracks[0].Shots.Count, this.visualMediaModel.ReorderElementsArguments.Count);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenAudioAssetElementAddedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Audio });

            var timelineElement = new TimelineElement { Asset = new AudioAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.Tracks[0].Shots.Add(timelineElement);
            this.timelineModel.InvokeElementAdded(timelineElement);

            Assert.IsTrue(this.audioMediaModel.ReorderElementsCalled);
            Assert.AreEqual(this.timelineModel.Tracks[0].Shots.Count, this.audioMediaModel.ReorderElementsArguments.Count);
            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenImageAssetElementAddedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });
            var timelineElement = new TimelineElement { Asset = new ImageAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.Tracks[0].Shots.Add(timelineElement);
            this.timelineModel.InvokeElementAdded(timelineElement);

            Assert.IsTrue(this.visualMediaModel.ReorderElementsCalled);
            Assert.AreEqual(this.timelineModel.Tracks[0].Shots.Count, this.visualMediaModel.ReorderElementsArguments.Count);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementAdded event of <see cref="TimelineModel"/> is triggered for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenTitleAssetElementAddedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Title });

            var timelineElement = new TimelineElement { Asset = new TitleAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.Tracks[0].Shots.Add(timelineElement);
            this.timelineModel.InvokeElementAdded(timelineElement);

            Assert.IsTrue(this.titleMediaModel.ReorderElementsCalled);
            Assert.AreEqual(this.timelineModel.Tracks[0].Shots.Count, this.titleMediaModel.ReorderElementsArguments.Count);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that RemoveElement And RemoveBlankElement of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementAndRemoveBlankElementOnAggregateMediaModelWhenVideoAssetElementRemovedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new VideoAsset() });

            Assert.IsTrue(this.visualMediaModel.RemoveElementCalled);
            Assert.IsTrue(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);
        }

        /// <summary>
        /// Tests that RemoveElement And RemoveBlankElement of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementAndRemoveBlankElementOnAggregateMediaModelWhenAudioAssetElementRemovedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new AudioAsset() });

            Assert.IsTrue(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveElementCalled);
            Assert.IsTrue(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);
        }

        /// <summary>
        /// Tests that RemoveElement And RemoveBlankElement of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementAndRemoveBlankElementOnAggregateMediaModelWhenImageAssetElementRemovedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new ImageAsset() });

            Assert.IsTrue(this.visualMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsTrue(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);
        }

        /// <summary>
        /// Tests that RemoveElement And RemoveBlankElement of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementAndRemoveBlankElementOnAggregateMediaModelWhenTitleAssetElementRemovedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.titleMediaModel.RemoveBlankElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new TitleAsset() });

            Assert.IsFalse(this.visualMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveElementCalled);
            Assert.IsFalse(this.visualMediaModel.RemoveBlankElementCalled);
            Assert.IsFalse(this.audioMediaModel.RemoveBlankElementCalled);
            Assert.IsTrue(this.titleMediaModel.RemoveElementCalled);
            Assert.IsTrue(this.titleMediaModel.RemoveBlankElementCalled);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls EndBuffer of <see cref="PlayerView"/>
        /// when an element is delted at the current playhead position.
        /// </summary>
        [TestMethod]
        public void ShouldCallToEndBufferWhenDeletingAnElementThatIsUnderTheCurrentPosition()
        {
            var element = new TimelineElement 
            { 
                Asset = new AudioAsset(),
                Position = TimeCode.FromSeconds(3500d, SmpteFrameRate.Smpte2997NonDrop),
                InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2997NonDrop),
                OutPosition = TimeCode.FromSeconds(500d, SmpteFrameRate.Smpte2997NonDrop)
            };

            this.timelineModel.CurrentPosition = TimeCode.FromSeconds(3600d, SmpteFrameRate.Smpte2997NonDrop);
            
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.EndBufferCalled);

            this.timelineModel.InvokeElementRemoved(element);

            Assert.IsTrue(this.view.EndBufferCalled);
        }

        /// <summary>
        /// Tests that RemoveElement of <see cref="PlayerView"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementOnViewWhenVideoAssetElementRemoveEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.RemoveElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new VideoAsset() });

            Assert.IsTrue(this.view.RemoveElementCalled);
        }

        /// <summary>
        /// Tests that RemoveElement of <see cref="PlayerView"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementOnViewWhenAudioAssetElementRemoveEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.RemoveElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new AudioAsset() });

            Assert.IsTrue(this.view.RemoveElementCalled);
        }

        /// <summary>
        /// Tests that RemoveElement of <see cref="PlayerView"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementOnViewWhenImageAssetElementRemoveEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.RemoveElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new ImageAsset() });

            Assert.IsTrue(this.view.RemoveElementCalled);
        }

        /// <summary>
        /// Tests that RemoveElement of <see cref="PlayerView"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallRemoveElementOnViewWhenTitleAssetElementRemoveEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.RemoveElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement { Asset = new TitleAsset() });

            Assert.IsTrue(this.view.RemoveElementCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenVideoAssetElementRemovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });

            var timelineElement = new TimelineElement { Asset = new VideoAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementRemoved(timelineElement);

            Assert.IsTrue(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenAudioAssetElementRemovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Audio });

            var timelineElement = new TimelineElement { Asset = new AudioAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementRemoved(timelineElement);

            Assert.IsTrue(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenImageAssetElementRemovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });

            var timelineElement = new TimelineElement { Asset = new ImageAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementRemoved(timelineElement);

            Assert.IsTrue(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementRemoved event of <see cref="TimelineModel"/> is triggered for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenTitleAssetElementRemovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Title });

            var timelineElement = new TimelineElement { Asset = new TitleAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementRemoved(timelineElement);

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsTrue(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementMoved event of <see cref="TimelineModel"/> is triggered for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenVideoAssetElementMovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });

            var timelineElement = new TimelineElement { Asset = new VideoAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementMoved(timelineElement);

            Assert.IsTrue(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementMoved event of <see cref="TimelineModel"/> is triggered for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenAudioAssetElementMovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Audio });

            var timelineElement = new TimelineElement { Asset = new AudioAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementMoved(timelineElement);

            Assert.IsTrue(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementMoved event of <see cref="TimelineModel"/> is triggered for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenImageAssetElementMovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });

            var timelineElement = new TimelineElement { Asset = new ImageAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementMoved(timelineElement);

            Assert.IsTrue(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that ReorderElements of <see cref="AggregateMediaModel"/> is called when 
        /// ElementMoved event of <see cref="TimelineModel"/> is triggered for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallReorderElementsOnModelWhenTitleAssetElementMovedEventIsInvoked()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Title });

            var timelineElement = new TimelineElement { Asset = new TitleAsset() };
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.titleMediaModel.ReorderElementsCalled);

            this.timelineModel.InvokeElementMoved(timelineElement);

            Assert.IsFalse(this.visualMediaModel.ReorderElementsCalled);
            Assert.IsFalse(this.audioMediaModel.ReorderElementsCalled);
            Assert.IsTrue(this.titleMediaModel.ReorderElementsCalled);
        }

        /// <summary>
        /// Tests that Position of <see cref="AggregateMediaModel"/> is set when 
        /// <see cref="PlayheadMovedEvent"/> event is published.
        /// </summary>
        public void ShouldSetPositionWithPlayheadMovedEventSubscription()
        {
            var position = new TimeSpan(0, 0, 10, 0);
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.PositionSet);
            Assert.IsFalse(this.audioMediaModel.PositionSet);
            this.playheadMovedEvent.SubscribeArgumentAction(new PositionPayloadEventArgs(position));

            Assert.IsTrue(this.visualMediaModel.PositionSet);
            Assert.IsTrue(this.audioMediaModel.PositionSet);
        }

        /// <summary>
        /// Tests that <see cref="PlayerViewPresenter"/> calls Pause of <see cref="AggregateMediaModel"/>
        /// when <see cref="PauseEvent"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPauseOnAggregateModelWithPauseEventSubscription()
        {
            Assert.IsNull(this.pauseEvent.SubscribeArgumentAction);
            Assert.IsNull(this.pauseEvent.SubscribeArgumentFilter);

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.PauseCalled);
            Assert.IsFalse(this.audioMediaModel.PauseCalled);
            Assert.IsFalse(this.titleMediaModel.PauseCalled);

            Assert.IsNotNull(this.pauseEvent.SubscribeArgumentAction);
            Assert.AreEqual(ThreadOption.PublisherThread, this.pauseEvent.SubscribeArgumentThreadOption);

            this.pauseEvent.SubscribeArgumentAction(null);

            Assert.IsTrue(this.visualMediaModel.PauseCalled);
            Assert.IsTrue(this.audioMediaModel.PauseCalled);
            Assert.IsTrue(this.titleMediaModel.PauseCalled);
        }

        /// <summary>
        /// Tests that <see cref="PositionUpdatedEvent"/> is published when PositionUpdated event 
        /// from visual <see cref="AggregateMediaModel"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPositionUpdatedEventWhenInvokingPositionUpdatedOnVisualAggregateMediaModel()
        {
            var presenter = this.CreatePresenter();

            var position = new TimeSpan(200);

            Assert.IsFalse(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.IsFalse(this.view.SetCurrentTimeCalled);

            this.visualMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNotNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.AreEqual(position, this.positionUpdatedEvent.PublishArgumentPayload.Position);
            Assert.IsTrue(this.view.SetCurrentTimeCalled);
            Assert.IsNotNull(this.view.SetCurrentTimeArgument);
            Assert.AreEqual(position, this.view.SetCurrentTimeArgument);
        }

        /// <summary>
        /// Tests that <see cref="PositionUpdatedEvent"/> is published when PositionUpdated event 
        /// from audio <see cref="AggregateMediaModel"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPositionUpdatedEventWhenInvokingPositionUpdatedOnAudioAggregateMediaModel()
        {
            var presenter = this.CreatePresenter();

            var position = new TimeSpan(200);

            Assert.IsFalse(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.IsFalse(this.view.SetCurrentTimeCalled);

            this.audioMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNotNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.AreEqual(position, this.positionUpdatedEvent.PublishArgumentPayload.Position);
            Assert.IsTrue(this.view.SetCurrentTimeCalled);
            Assert.IsNotNull(this.view.SetCurrentTimeArgument);
            Assert.AreEqual(position, this.view.SetCurrentTimeArgument);
        }

        /// <summary>
        /// Tests that <see cref="PositionUpdatedEvent"/> is published when PositionUpdated event 
        /// from title <see cref="AggregateMediaModel"/> is published.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPositionUpdatedEventWhenInvokingPositionUpdatedOnTitleAggregateMediaModel()
        {
            var presenter = this.CreatePresenter();

            var position = new TimeSpan(200);

            Assert.IsFalse(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.IsFalse(this.view.SetCurrentTimeCalled);

            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNotNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.AreEqual(position, this.positionUpdatedEvent.PublishArgumentPayload.Position);
            Assert.IsTrue(this.view.SetCurrentTimeCalled);
            Assert.IsNotNull(this.view.SetCurrentTimeArgument);
            Assert.AreEqual(position, this.view.SetCurrentTimeArgument);
        }

        /// <summary>
        /// Publishes DownloadProgressChangedEvent when audio download progresses for the aggregate media model.
        /// </summary>
        [TestMethod]
        public void ShouldPublishDownloadProgressChangedEventWhenInvokingDownloadProgressChangedOnAudioAggregateMediaModel()
        {
            var presenter = this.CreatePresenter();

            var downloadProgressEventArgs = new AssetDownloadProgressEventArgs(new TimelineElement(), 0.2, 5);

            Assert.IsFalse(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.IsFalse(this.view.DownloadProgressChangedCalled);

            this.audioMediaModel.InvokeDownloadProgressChanged();

            Assert.IsTrue(this.downloadProgressChangedEvent.PublishCalled);
            Assert.IsNotNull(this.downloadProgressChangedEvent.PublishArgumentPayload);
        }

        /// <summary>
        /// Publishes DownloadProgressChangedEvent when video download progresses for the aggregate media model.
        /// </summary>
        [TestMethod]
        public void ShouldPublishDownloadProgressChangedEventWhenInvokingDownloadProgressChangedOnVideoAggregateMediaModel()
        {
            var presenter = this.CreatePresenter();

            var downloadProgressEventArgs = new AssetDownloadProgressEventArgs(new TimelineElement(), 0.2, 5);

            Assert.IsFalse(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.IsFalse(this.view.DownloadProgressChangedCalled);

            this.visualMediaModel.InvokeDownloadProgressChanged();

            Assert.IsTrue(this.downloadProgressChangedEvent.PublishCalled);
            Assert.IsNotNull(this.downloadProgressChangedEvent.PublishArgumentPayload);
        }

        /// <summary>
        /// Doesnot publish DownloadProgressChangedEvent when titles download progresses for the aggregate media model.
        /// </summary>
        [TestMethod]
        public void ShouldNotPublishDownloadProgressChangedEventWhenInvokingDownloadProgressChangedOnTitleAggregateMediaModel()
        {
            var presenter = this.CreatePresenter();

            var downloadProgressEventArgs = new AssetDownloadProgressEventArgs(new TimelineElement(), 0.2, 5);

            Assert.IsFalse(this.positionUpdatedEvent.PublishCalled);
            Assert.IsNull(this.positionUpdatedEvent.PublishArgumentPayload);
            Assert.IsFalse(this.view.DownloadProgressChangedCalled);

            this.titleMediaModel.InvokeDownloadProgressChanged();

            Assert.IsFalse(this.downloadProgressChangedEvent.PublishCalled);
            Assert.IsNull(this.downloadProgressChangedEvent.PublishArgumentPayload);
        }

        /// <summary>
        /// Set source with player event subscription when player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldSetSourceWithPlayerEventSubscriptionWhenPlayerModeIsNotTimeline()
        {
            var asset = new VideoAsset { Source = new Uri("http://test") };

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.SetSourceCalled);
            Assert.IsNull(this.view.SetSourceArgument);

            this.playerEvent.SubscribeArgumentAction(new PlayerEventPayload { Asset = asset, PlayerMode = PlayerMode.MediaBin });

            Assert.IsTrue(this.view.SetSourceCalled);
            Assert.AreEqual(asset.Source, this.view.SetSourceArgument);
        }

        /// <summary>
        /// Toggle play with player event subscription when player mode is timeline.
        /// </summary>
        [TestMethod]
        public void ShouldTogglePlayModelWithPlayerEventSubscriptionWhenPlayerModeIsTimeline()
        {
            var asset = new VideoAsset { Source = new Uri("http://test") };

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.visualMediaModel.PlayCalled);

            this.playerEvent.SubscribeArgumentAction(new PlayerEventPayload { Asset = asset, PlayerMode = PlayerMode.Timeline });

            Assert.IsTrue(this.visualMediaModel.PlayCalled);
        }

        /// <summary>
        /// Call MovetoStart when MoveStartClicked event is invoked and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallToMoveToStartWhenMoveStartClickedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.view.MoveToStartCalled);

            this.view.InvokeMoveToStartClicked();

            Assert.IsTrue(this.view.MoveToStartCalled);
        }

        /// <summary>
        /// Call MovetoEnd when MoveEndClicked event is invoked and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallToMoveToEndWhenMoveEndClickedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.view.MoveToEndCalled);

            this.view.InvokeMoveToEndClicked();

            Assert.IsTrue(this.view.MoveToEndCalled);
        }

        /// <summary>
        /// Call to StartFrameRewindForward when FrameRewindStarted event is invoked and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallToStartRewindForwardWhenRewindStartedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.view.StartRewindForwardCalled);

            this.view.InvokeRewindStarted();

            Assert.IsTrue(this.view.StartRewindForwardCalled);
        }

        /// <summary>
        /// Call to StartFrameRewindForward when FrameForwardStarted event is invoked and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallToStartRewindForwardWhenForwardStartedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.view.StartRewindForwardCalled);

            this.view.InvokeForwardStarted();

            Assert.IsTrue(this.view.StartRewindForwardCalled);
        }

        /// <summary>
        /// Call to EndFrameRewindForward when FrameRewindEnded event is invoked and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallToEndRewindForwardWhenRewindEndedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.view.EndRewindForwardCalled);

            this.view.InvokeRewindEnded();

            Assert.IsTrue(this.view.EndRewindForwardCalled);
        }

        /// <summary>
        /// Call to end EndFrameRewindForward when FrameForwardEnded event is invoked and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallToEndRewindForwardWhenForwardEndedEventIsInvokedAndPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.view.EndRewindForwardCalled);

            this.view.InvokeForwardEnded();

            Assert.IsTrue(this.view.EndRewindForwardCalled);
        }

        /// <summary>
        /// Call ShowComment if comment is at the current position.
        /// </summary>
        [TestMethod]
        public void ShouldCallShowCommentIfCommentIsAtTheCurrentPosition()
        {
            var presenter = this.CreatePresenter();
            var position = TimeSpan.FromSeconds(200);
            this.view.ShowCommentsCalled = false;
            Comment comment = new Comment { MarkIn = 200, MarkOut = 300, Text = "TestComment" };

            this.timelineModel.CommentElements.Add(comment);
            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.view.ShowCommentsCalled);
            Assert.AreSame(this.view.Comments[0], comment);
        }

        /// <summary>
        /// Call HideComment if current comment is not null and there is no comment at current position.
        /// </summary>
        [TestMethod]
        public void ShouldCallHideCommentIfCurrentCommentIsNotNullAndThereIsNoCommentAtCurrentPosition()
        {
            var presenter = this.CreatePresenter();
            var position = TimeSpan.FromSeconds(200);
            this.view.ShowCommentsCalled = false;
            this.view.HideCommentsCalled = false;
            Comment comment = new Comment() { MarkIn = 200, MarkOut = 300, Text = "TestComment" };

            this.timelineModel.CommentElements.Add(comment);
            this.titleMediaModel.InvokePositionUpdated(position);
            
            Assert.IsTrue(this.view.ShowCommentsCalled);
            Assert.AreSame(this.view.Comments[0], comment);
            
            position = TimeSpan.FromSeconds(400);
            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.view.HideCommentsCalled);
        }

        /// <summary>
        /// Should not call ShowComment if current comment same that as at current position.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallShowCommentIfCurrentCommentSameThatAtCurrentPosition()
        {
            var presenter = this.CreatePresenter();
            var position = TimeSpan.FromSeconds(200);
            this.view.ShowCommentsCalled = false;
            this.view.HideCommentsCalled = false;
            Comment comment = new Comment() { MarkIn = 200, MarkOut = 300, Text = "TestComment" };

            this.timelineModel.CommentElements.Add(comment);
            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.view.ShowCommentsCalled);
            Assert.AreSame(this.view.Comments[0], comment);

            position = TimeSpan.FromSeconds(230);
            this.view.ShowCommentsCalled = false;
            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.view.ShowCommentsCalled);
        }

        /// <summary>
        /// Should call ShowComments if current comment is not null and there is other comment at current position.
        /// </summary>
        [TestMethod]
        public void ShouldCallShowCommentsIfCurrentCommentIsNotNullAndThereIsOtherCommentAtCurrentPosition()
        {
            var presenter = this.CreatePresenter();
            var position = TimeSpan.FromSeconds(200);
            this.view.ShowCommentsCalled = false;
            this.view.HideCommentsCalled = false;
            Comment comment = new Comment() { MarkIn = 200, MarkOut = 300, Text = "TestComment" };
            Comment comment1 = new Comment() { MarkIn = 250, MarkOut = 400, Text = "TestComment" };

            this.timelineModel.CommentElements.Add(comment);
            this.timelineModel.CommentElements.Add(comment1);
            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.view.ShowCommentsCalled);
            Assert.AreSame(this.view.Comments[0], comment);

            position = TimeSpan.FromSeconds(350);
            this.view.ShowCommentsCalled = false;
            
            this.titleMediaModel.InvokePositionUpdated(position);

            Assert.IsTrue(this.view.ShowCommentsCalled);
            Assert.AreSame(this.view.Comments[0], comment1);
        }

        /// <summary>
        /// Should call to Play on <see cref="AggregateMediaModel"/> models when play comment is called.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPlayOnAggregateMediaModelsWhenPlayCommentIsCalled()
        {
            Comment comment = new Comment(Guid.NewGuid())
                                    {
                                        CommentType = CommentType.Timeline,
                                        MarkIn = 100,
                                        MarkOut = 200,
                                    };
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Comment;
            this.view.SetCurrentTimeCalled = false;

            Assert.IsFalse(this.visualMediaModel.PlayCalled);
            Assert.IsFalse(this.audioMediaModel.PlayCalled);
            Assert.IsFalse(this.titleMediaModel.PlayCalled);

            this.playCommentEvent.Publish(comment);

            Assert.IsTrue(this.visualMediaModel.PlayCalled);
            Assert.IsTrue(this.audioMediaModel.PlayCalled);
            Assert.IsTrue(this.titleMediaModel.PlayCalled);
            Assert.IsTrue(this.view.SetCurrentTimeCalled);
        }

        /// <summary>
        /// Should call to SetCurrentTime when MoveStartClicked event is invoked and player mode is comment.
        /// </summary>
        [TestMethod]
        public void ShouldCallToSetCurrentTimeWhenMoveStartClickedEventIsInvokedAndPlayerModeIsComment()
        {
            var presenter = this.CreatePresenter();
            Comment comment = new Comment(Guid.NewGuid())
            {
                CommentType = CommentType.Timeline,
                MarkIn = 100,
                MarkOut = 200,
            };

            // To set the currentPlayingComment variable.
            this.playCommentEvent.Publish(comment);
            presenter.PlayerMode = PlayerMode.Comment;
            this.view.SetCurrentTimeCalled = false;
            
            this.view.InvokeMoveToStartClicked();

            Assert.IsTrue(this.view.SetCurrentTimeCalled);
            Assert.IsTrue(this.view.SetCurrentTimeArgument.TotalSeconds == comment.MarkIn);
        }

        /// <summary>
        /// Should call to SetCurrentTime when MoveEndClicked event is invoked and player mode is comment.
        /// </summary>
        [TestMethod]
        public void ShouldCallToSetCurrentTimeWhenMoveEndClickedEventIsInvokedAndPlayerModeIsComment()
        {
            var presenter = this.CreatePresenter();
            Comment comment = new Comment(Guid.NewGuid())
            {
                CommentType = CommentType.Timeline,
                MarkIn = 100,
                MarkOut = 200,
            };

            // To set the currentPlayingComment variable.
            this.playCommentEvent.Publish(comment);
            presenter.PlayerMode = PlayerMode.Comment;
            this.view.SetCurrentTimeCalled = false;

            this.view.InvokeMoveToEndClicked();

            Assert.IsTrue(this.view.SetCurrentTimeCalled);
            Assert.IsTrue(this.view.SetCurrentTimeArgument.TotalSeconds == comment.MarkOut);
        }

        /// <summary>
        /// Should publish <see cref="addCommentEvent"/> event when add comment event is raised from view.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPositionDoubleClickedEventWhenAddCommentEventIsRaisedFromView()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            this.addCommentEvent.PositionPayloadEventArgs = null;
            this.addCommentEvent.PublishCalled = false;

            this.view.InvokeAddCommentClicked();

            Assert.IsTrue(this.addCommentEvent.PublishCalled);
        }

        /// <summary>
        /// Should publish <see cref="addCommentEvent"/> event when add comment event is raised from view and player mode is media bin.
        /// </summary>
        [TestMethod]
        public void ShouldNotPublishPositionDoubleClickedEventWhenAddCommentEventIsRaisedFromViewAndPlayerModeIsMediaBin()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaBin;
            this.addCommentEvent.PositionPayloadEventArgs = null;
            this.addCommentEvent.PublishCalled = false;

            this.view.InvokeAddCommentClicked();

            Assert.IsFalse(this.addCommentEvent.PublishCalled);
            Assert.AreEqual(null, this.addCommentEvent.PositionPayloadEventArgs);
        }

        /// <summary>
        /// Should publish <see cref="addCommentEvent"/> event when add comment event is raised from view and player mode is media library.
        /// </summary>
        [TestMethod]
        public void ShouldNotPublishPositionDoubleClickedEventWhenAddCommentEventIsRaisedFromViewAndPlayerModeIsMediaLibrary()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaLibrary;
            this.addCommentEvent.PositionPayloadEventArgs = null;
            this.addCommentEvent.PublishCalled = false;

            this.view.InvokeAddCommentClicked();

            Assert.IsFalse(this.addCommentEvent.PublishCalled);
            Assert.AreEqual(null, this.addCommentEvent.PositionPayloadEventArgs);
        }

        /// <summary>
        /// Should publish <see cref="addCommentEvent"/> event when add comment event is raised from view and player mode is comment.
        /// </summary>
        [TestMethod]
        public void ShouldNotPublishPositionDoubleClickedEventWhenAddCommentEventIsRaisedFromViewAndPlayerModeIsComment()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Comment;
            this.addCommentEvent.PositionPayloadEventArgs = null;
            this.addCommentEvent.PublishCalled = false;
            
            this.view.InvokeAddCommentClicked();

            Assert.IsFalse(this.addCommentEvent.PublishCalled);
            Assert.AreEqual(null, this.addCommentEvent.PositionPayloadEventArgs);
        }

        /// <summary>
        /// Should set IsMuted to true if it is false when MuteClicked event is triggerd.
        /// </summary>
        [TestMethod]
        public void ShouldSetIsMutedToTrueIfItIsFalseWhenMuteClickedEventIsTriggerd()
        {
            this.view.IsMuted = false;
            var presenter = this.CreatePresenter();

            this.view.InvokeMuteClicked();

            Assert.IsTrue(this.view.IsMuted);
        }

        /// <summary>
        /// Shoulds set IsMuted to false if it is true when MuteClicked event is triggerd.
        /// </summary>
        [TestMethod]
        public void ShouldSetIsMutedToFalseIfItIsTrueWhenMuteClickedEventIsTriggerd()
        {
            this.view.IsMuted = true;
            var presenter = this.CreatePresenter();

            this.view.InvokeMuteClicked();

            Assert.IsFalse(this.view.IsMuted);
        }

        /// <summary>
        /// Shoulds set mute property of visual and audio aggregate model if player mode 
        /// is timeline when MuteClicked event is triggerd.
        /// </summary>
        [TestMethod]
        public void ShouldSetMutePropertyOfVisualAndAudioAggregateModelIfPlayerModeIsTimelineWhenMuteClickedEventIsTriggerd()
        {
            var presenter = this.CreatePresenter();
            this.view.IsMuted = false;
            this.visualMediaModel.Mute = false;
            this.audioMediaModel.Mute = false;
            presenter.PlayerMode = PlayerMode.Timeline;
            
            this.view.InvokeMuteClicked();

            Assert.IsTrue(this.visualMediaModel.Mute);
            Assert.IsTrue(this.audioMediaModel.Mute);
        }

        /// <summary>
        /// Should not set mute property of visual and audio aggregate model if player mode
        ///  is not timeline when MuteClicked event is triggerd.
        /// </summary>
        [TestMethod]
        public void ShouldNotSetMutePropertyOfVisualAndAudioAggregateModelIfPlayerModeIsNotTimelineWhenMuteClickedEventIsTriggerd()
        {
            this.view.IsMuted = false;
            this.visualMediaModel.Mute = false;
            this.audioMediaModel.Mute = false;
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaBin;

            this.view.InvokeMuteClicked();

            Assert.IsFalse(this.visualMediaModel.Mute);
            Assert.IsFalse(this.audioMediaModel.Mute);
        }

        /// <summary>
        /// Should set the mute of aggregate model from to view is muted propery while playing aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldSetTheMuteOfAggregateModelFromToViewIsMutedProperyWhilePlayingAggregateModel()
        {
            Comment comment = new Comment(Guid.NewGuid())
            {
                CommentType = CommentType.Timeline,
                MarkIn = 100,
                MarkOut = 200,
            };
            var presenter = this.CreatePresenter();
            this.view.IsMuted = false;
            this.visualMediaModel.Mute = true;
            this.audioMediaModel.Mute = true;
            
            // We are using this event as it actully calls the PlayModel private method.
            this.playCommentEvent.Publish(comment);

            Assert.IsFalse(this.visualMediaModel.Mute);
            Assert.IsFalse(this.audioMediaModel.Mute);
        }

        /// <summary>
        /// Set IsVisible to false of aggregate model if player mode changes from timline to other.
        /// </summary>
        [TestMethod]
        public void SetIsVisibleToFalseOfAggregateModelIfPlayerModeChangesFromTimlineToOther()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            this.visualMediaModel.IsVisible = true;
            this.visualMediaModel.IsVisible = true;

            presenter.PlayerMode = PlayerMode.MediaBin;

            Assert.IsFalse(this.visualMediaModel.IsVisible);
            Assert.IsFalse(this.visualMediaModel.IsVisible);
        }

        /// <summary>
        /// Set IsVisible to false of aggregate model if player mode changes from other to timline.
        /// </summary>
        [TestMethod]
        public void SetIsVisibleToFalseOfAggregateModelIfPlayerModeChangesFromOtherToTimline()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaBin;
            this.visualMediaModel.IsVisible = false;
            this.visualMediaModel.IsVisible = false;
            this.view.StopCalled = false;

            presenter.PlayerMode = PlayerMode.Timeline;

            Assert.IsTrue(this.view.StopCalled);
            Assert.IsTrue(this.visualMediaModel.IsVisible);
            Assert.IsTrue(this.visualMediaModel.IsVisible);
        }

        /// <summary>
        /// Should call HideComment method if the comment is visible when player mode changes from timeline to media bin.
        /// </summary>
        [TestMethod]
        public void ShouldCallViewHideCommentMethodIfTheCommentIsVisibleWhenPlayerModeChangesFromTimelineToMediaBin()
        {
            this.timelineModel.CommentElements.Add(new Comment { MarkIn = 0, MarkOut = 1000.0 });
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            this.view.HideCommentsCalled = false;

            // Change the position so that the current comment can have some value.
            this.visualMediaModel.InvokePositionUpdated(TimeSpan.FromSeconds(200));
            presenter.PlayerMode = PlayerMode.MediaBin;
            
            Assert.IsTrue(this.view.HideCommentsCalled);            
        }

        /// <summary>
        /// Should call StartBuffer method of view if StartBuffer event is triggered from video aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldCallStartBufferMethodOfViewIfStartBufferEventIsTriggeredFromVideoAggregateModel()
        {
            var presenter = this.CreatePresenter();
            this.view.StartBufferCalled = false;

            this.visualMediaModel.InvokeBufferStart();
            
            Assert.IsTrue(this.view.StartBufferCalled);
        }

        /// <summary>
        /// Should EndBuffer method of view if EndBuffer event is triggered from video aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldCallEndBufferMethodOfViewIfEndBufferEventIsTriggeredFromVideoAggregateModel()
        {
            var presenter = this.CreatePresenter();
            this.view.EndBufferCalled = false;

            this.visualMediaModel.InvokeBufferEnd();

            Assert.IsTrue(this.view.EndBufferCalled);
        }

        /// <summary>
        /// Should StartBuffer method of view if start buffer event is triggered from audio aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldCallStartBufferMethodOfViewIfStartBufferEventIsTriggeredFromAudioAggregateModel()
        {
            var presenter = this.CreatePresenter();
            this.view.StartBufferCalled = false;

            this.audioMediaModel.InvokeBufferStart();

            Assert.IsTrue(this.view.StartBufferCalled);
        }

        /// <summary>
        /// Should EndBuffer method of view if EndBuffer event is triggered from audio aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldCallEndBufferMethodOfViewIfEndBufferEventIsTriggeredFromAudioAggregateModel()
        {
            var presenter = this.CreatePresenter();
            this.view.EndBufferCalled = false;

            this.audioMediaModel.InvokeBufferEnd();

            Assert.IsTrue(this.view.EndBufferCalled);
        }

        /// <summary>
        /// Should StartBuffer method of view if StartBuffer event is triggered from title aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallStartBufferMethodOfViewIfStartBufferEventIsTriggeredFromTitleAggregateModel()
        {
            var presenter = this.CreatePresenter();
            this.view.StartBufferCalled = false;

            this.titleMediaModel.InvokeBufferStart();

            Assert.IsFalse(this.view.StartBufferCalled);
        }

        /// <summary>
        /// Should EndBuffer method of view if EndBuffer event is triggered from audio aggregate model.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallEndBufferMethodOfViewIfEndBufferEventIsTriggeredFromTitleAggregateModel()
        {
            var presenter = this.CreatePresenter();
            this.view.EndBufferCalled = false;

            this.titleMediaModel.InvokeBufferEnd();

            Assert.IsFalse(this.view.EndBufferCalled);
        }

        /// <summary>
        /// Should HidePreviewImage method of view if drop command is executed for video asset.
        /// </summary>
        [TestMethod]
        public void ShouldCallHidePreviewImageMethodOfViewIfDropEventIsTriggeredForVideoAsset()
        {
            var presenter = this.CreatePresenter();
            this.view.HidePreviewImageCalled = false;

            var payload = new DropPayload
            {
                DraggedItem = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(30, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte30,
                    Title = "Test Video #1"
                },
                MouseEventArgs = null
            };

            Assert.IsFalse(this.view.SetSourceCalled);

            presenter.PlayerMode = PlayerMode.Timeline;
            presenter.DropCommand.Execute(payload);
            Assert.IsTrue(this.view.SetSourceCalled);
            Assert.IsTrue(this.view.HidePreviewImageCalled);
        }

        /// <summary>
        /// Should HidePreviewImage method of view if drop command is executed for audio asset.
        /// </summary>
        [TestMethod]
        public void ShouldCallHidePreviewImageMethodOfViewIfDropEventIsTriggeredForAudioAsset()
        {
            var presenter = this.CreatePresenter();
            this.view.HidePreviewImageCalled = false;

            var payload = new DropPayload
            {
                DraggedItem = new AudioAsset
                {
                    Duration = 2,
                    Title = "Test Audio #1"
                },
                MouseEventArgs = null
            };

            Assert.IsFalse(this.view.SetSourceCalled);

            presenter.PlayerMode = PlayerMode.Timeline;
            presenter.DropCommand.Execute(payload);
            Assert.IsTrue(this.view.SetSourceCalled);
            Assert.IsTrue(this.view.HidePreviewImageCalled);
        }

        /// <summary>
        /// Should HidePreviewImage method of view if drop command is executed for image asset.
        /// </summary>
        [TestMethod]
        public void ShouldCallHidePreviewImageMethodOfViewIfDropEventIsTriggeredForImageAsset()
        {
            var presenter = this.CreatePresenter();
            this.view.HidePreviewImageCalled = false;

            var payload = new DropPayload
            {
                DraggedItem = new ImageAsset
                {
                    Title = "Test Image #1"
                },
                MouseEventArgs = null
            };

            Assert.IsFalse(this.view.SetSourceCalled);

            presenter.PlayerMode = PlayerMode.MediaLibrary;
            presenter.DropCommand.Execute(payload);
            Assert.IsTrue(this.view.SetSourceCalled);
            Assert.IsTrue(this.view.HidePreviewImageCalled);
        }

        /// <summary>
        /// Should HidePreviewImage method of view if player event is triggered form timeline model.
        /// </summary>
        [TestMethod]
        public void ShouldCallHidePreviewImageMethodOfViewIfPlayerEventIsTriggeredFormTimelineModel()
        {
            var asset = new VideoAsset { Source = new Uri("http://test") };

            var presenter = this.CreatePresenter();
            this.view.HidePreviewImageCalled = false;

            Assert.IsFalse(this.view.SetSourceCalled);
            Assert.IsNull(this.view.SetSourceArgument);

            this.playerEvent.SubscribeArgumentAction(new PlayerEventPayload { Asset = asset, PlayerMode = PlayerMode.MediaBin });

            Assert.IsTrue(this.view.SetSourceCalled);
            Assert.AreEqual(asset.Source, this.view.SetSourceArgument);
            Assert.IsTrue(this.view.HidePreviewImageCalled);
        }

        /// <summary>
        /// Should HidePreviewImage method of view if Player mode is set to Timeline.
        /// </summary>
        [TestMethod]
        public void ShouldCallHidePreviewImageMethodOfViewWhenPlayerModeIsSetToTimeline()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            Assert.IsTrue(this.view.HidePreviewImageCalled);
        }

        /// <summary>
        /// Should HidePreviewImage method of view when player mode is set to comment.
        /// </summary>
        [TestMethod]
        public void ShouldCallHidePreviewImageMethodOfViewWhenPlayerModeIsSetToComment()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Comment;
            Assert.IsTrue(this.view.HidePreviewImageCalled);
        }

        /// <summary>
        /// If <see cref="keyMappingEvent"/> is published, playermode is timeline and player is playign timeline
        ///  then it should call pause of all the aggregate models in the player.
        /// </summary>
        [TestMethod]
        public void ShouldCallPauseOfAggregateModelIfPlayerModeIsTimeAndItIsPlayingLineAndkeyMappingEventIsPublished()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            this.visualMediaModel.PauseCalled = false;
            this.audioMediaModel.PauseCalled = false;
            this.titleMediaModel.PauseCalled = false;
            this.visualMediaModel.IsPlaying = true;
            this.audioMediaModel.IsPlaying = true;
            this.titleMediaModel.IsPlaying = true;

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.PausePlayer);

            Assert.IsTrue(this.visualMediaModel.PauseCalled);
            Assert.IsTrue(this.audioMediaModel.PauseCalled);
            Assert.IsTrue(this.titleMediaModel.PauseCalled);
        }

        /// <summary>
        /// If <see cref="keyMappingEvent"/> is published, playermode is timeline and player is in pause mode
        ///  then it should not call pause of all the aggregate models in the player.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallPauseOfAggregateModelIfPlayerModeIsTimeLineAndItIsInPauseModeAndkeyMappingEventIsPublished()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            this.visualMediaModel.PauseCalled = false;
            this.audioMediaModel.PauseCalled = false;
            this.titleMediaModel.PauseCalled = false;
            this.visualMediaModel.IsPlaying = false;
            this.audioMediaModel.IsPlaying = false;
            this.titleMediaModel.IsPlaying = false;

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.PausePlayer);

            Assert.IsFalse(this.visualMediaModel.PauseCalled);
            Assert.IsFalse(this.audioMediaModel.PauseCalled);
            Assert.IsFalse(this.titleMediaModel.PauseCalled);
        }

        /// <summary>
        /// Tests if <see cref="keyMappingEvent"/> is published, playermode is MediaBin
        ///  then it should not call pause of <see cref="PlayerView"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallPlayerViewIfPlayerModeIsMediaBinAndkeyMappingEventIsPublished()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaBin;
            this.view.PausePlayerCalled = false;

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.PausePlayer);

            Assert.IsTrue(this.view.PausePlayerCalled);
        }

        /// <summary>
        /// Tests if <see cref="keyMappingEvent"/> is published, playermode is MediaLibrary
        ///  then it should not call pause of <see cref="PlayerView"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallPlayerViewIfPlayerModeIsMediaLibraryAndkeyMappingEventIsPublished()
        {
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.MediaLibrary;
            this.view.PausePlayerCalled = false;

            this.keyMappingEvent.SubscribeArgumentAction(KeyMappingAction.PausePlayer);

            Assert.IsTrue(this.view.PausePlayerCalled);
        }

        /// <summary>
        /// Tests if the KeyMappingEvent Filter is being passed with KeyMappingAction PausePlayer.
        /// </summary>
        [TestMethod]
        public void ShouldPassKeyMappingEventFilterWhenKeyMappingActionIsPausePlayer()
        {
            var presenter = this.CreatePresenter();

            var result = this.keyMappingEvent.SubscribeArgumentFilter(KeyMappingAction.PausePlayer);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests if the KeyMappingEvent Filter is being passed with KeyMappingAction PlayTimeline.
        /// </summary>
        [TestMethod]
        public void ShouldPassKeyMappingEventFilterWhenKeyMappingActionIsPlayTimeline()
        {
            var presenter = this.CreatePresenter();

            var result = this.keyMappingEvent.SubscribeArgumentFilter(KeyMappingAction.PlayTimeline);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests if the KeyMappingEvent Filter is being passed with KeyMappingAction Toggle.
        /// </summary>
        [TestMethod]
        public void ShouldPassKeyMappingEventFilterWhenKeyMappingActionIsToggle()
        {
            var presenter = this.CreatePresenter();
            
            var result = this.keyMappingEvent.SubscribeArgumentFilter(KeyMappingAction.Toggle);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests if the scriptable member TooglePlayTimeline changes the player mode to timeline.
        /// </summary>
        [TestMethod]
        public void ShouldChangePlayerModeToTimelineWhenScriptableMemberTooglePlayTimelineIsExecuted()
        {
            var presenter = new PlayerViewPresenter(this.view, this.eventAggregator, this.timelineModel, this.visualMediaModel, this.audioMediaModel, this.titleMediaModel);
            
            presenter.PlayerMode = PlayerMode.MediaBin;

            presenter.TogglePlayTimeline();

            Assert.AreEqual(PlayerMode.Timeline, presenter.PlayerMode);
        }

        /// <summary>
        /// Tests if the scriptable member StopTimeline changes the player mode to timeline.
        /// </summary>
        [TestMethod]
        public void ShouldChangePlayerModeToTimelineWhenScriptableMemberStopTimelineIsExecuted()
        {
            var presenter = new PlayerViewPresenter(this.view, this.eventAggregator, this.timelineModel, this.visualMediaModel, this.audioMediaModel, this.titleMediaModel);

            presenter.PlayerMode = PlayerMode.MediaBin;

            presenter.StopTimeline();

            Assert.AreEqual(PlayerMode.Timeline, presenter.PlayerMode);
        }

        /// <summary>
        /// Tests that a null is returned when calling to GetMediaDataAtCurrentPosition and player mode is not timeline.
        /// </summary>
        [TestMethod]
        public void ShouldReturnNullWhenCallingToGetMediaDataAtCurrentPositionIfPlayerModeIsNotTimeline()
        {
            var presenter = this.CreatePresenter();

            presenter.PlayerMode = PlayerMode.MediaBin;

            var mediaData = presenter.GetMediaDataAtCurrentPosition();

            Assert.IsNull(mediaData);
        }

        /// <summary>
        /// Tests that FindMediaByElement is being called when calling to GetMediaDataAtCurrentPosition.
        /// </summary>
        [TestMethod]
        public void ShouldCallToFindMediaByElementOnAggregateMediaModelWhenCallingToGetMediaDataAtCurrentPosition()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });

            var timelineElement = new TimelineElement { Asset = new VideoAsset() };
            this.timelineModel.Tracks[0].Shots.Add(timelineElement);
            this.timelineModel.CurrentPosition = TimeCode.FromSeconds(10d, SmpteFrameRate.Smpte2997NonDrop);

            this.timelineModel.GetElementAtPositionReturnFunction += () => timelineElement;
            
            var presenter = this.CreatePresenter();
            presenter.PlayerMode = PlayerMode.Timeline;
            presenter.GetMediaDataAtCurrentPosition();

            Assert.IsTrue(this.visualMediaModel.FindMediaByElementCalled);
            Assert.AreEqual(timelineElement, this.visualMediaModel.FindMediaByElementArgument);
        }

        /// <summary>
        /// Tests that PickThumbnail is being called when PickThumbnailEvent event subscription action is being invoked.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPickThumbnailOnViewWithPickThumbnailEventIsSubscription()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.PickThumbnailCalled);

            this.pickThumbnailEvent.SubscribeArgumentAction(null);

            Assert.IsTrue(this.view.PickThumbnailCalled);
        }

        /// <summary>
        /// Tests that the ThumbnailEvent event is being published when the SetThumbnail method is called.
        /// </summary>
        [TestMethod]
        public void ShouldPublishThumbnailEventWhenCallingToSetThumbnail()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.thumbnailEvent.PublishCalled);

            var bitmap = new WriteableBitmap(10, 15);

            presenter.SetThumbnail(bitmap);

            Assert.IsTrue(this.thumbnailEvent.PublishCalled);
        }

        /// <summary>
        /// Tests that PickThumbnail is being called when PickThumbnailClicked event is invoked.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPickThumbnailOnViewWhenPickThumbnailClickedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.PickThumbnailCalled);

            this.view.InvokePickThumbnailClicked();

            Assert.IsTrue(this.view.PickThumbnailCalled);
        }

        /// <summary>
        /// Tests that PickThumbnail is being called when PickThumbnailClicked event is invoked.
        /// </summary>
        [TestMethod]
        public void ShouldCallToToggleSlowMotionOnViewWhenSlowMotionClickedEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.ToggleSlowMotionCalled);

            this.view.InvokeSlowMotionClicked();

            Assert.IsTrue(this.view.ToggleSlowMotionCalled);
        }

        /// <summary>
        /// Creates the presenter.
        /// </summary>
        /// <returns>The <see cref="PlayerViewPresenter"/>.</returns>
        private IPlayerViewPresenter CreatePresenter()
        {
            return new PlayerViewPresenter(this.view, this.eventAggregator, this.timelineModel, this.visualMediaModel, this.audioMediaModel, this.titleMediaModel);
        }
    }
}