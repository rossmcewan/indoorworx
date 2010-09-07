// <copyright file="TimelinePresenterFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TimelinePresenterFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline.Tests
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.DragDrop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCE.Infrastructure.Events;
    using RCE.Infrastructure.Models;
    using RCE.Modules.Timeline.Commands;
    using RCE.Modules.Timeline.Tests.Mocks;
    using Services.Contracts;
    using SMPTETimecode;
    using Track = RCE.Infrastructure.Models.Track;
    using TrackType = RCE.Infrastructure.Models.TrackType;

    /// <summary>
    /// A class for testing the <see cref="TimelinePresenter"/>.
    /// </summary>
    [TestClass]
    public class TimelinePresenterFixture
    {
        /// <summary>
        /// The mocked TimelineView.
        /// </summary>
        private MockTimelineView view;

        /// <summary>
        /// The mocked TimelineModel.
        /// </summary>
        private MockTimelineModel timelineModel;

        /// <summary>
        /// The mocked EventAggregator.
        /// </summary>
        private MockEventAggregator eventAggregator;

        /// <summary>
        /// The mocked StartTimeCodeChangedEvent event.
        /// </summary>
        private MockStartTimeCodeChangedEvent startTimeCodeChangedEvent;

        /// <summary>
        /// The mocked AddAssetEvent event.
        /// </summary>
        private MockAddAssetEvent addAssetEvent;

        /// <summary>
        /// The mocked DeleteMediaBinAssetEvent event.
        /// </summary>
        private MockDeleteMediaBinAssetEvent deleteMediaBinAssetEvent;

        /// <summary>
        /// The mocked AddAssetToTimelineEvent event.
        /// </summary>
        private MockAddAssetToTimelineEvent addAssetToTimelineEvent;

        /// <summary>
        /// The mocked PauseEvent event.
        /// </summary>
        private MockPauseEvent pauseEvent;

        /// <summary>
        /// The mocked PositionUpdateEvent event.
        /// </summary>
        private MockPositionUpdatedEvent positionUpdatedEvent;

        /// <summary>
        /// The mocked PlayheadMovedEvent event.
        /// </summary>
        private MockPlayheadMovedEvent playheadMovedEvent;

        /// <summary>
        /// The mocked EditModeChangedEvent event.
        /// </summary>
        private MockEditModeChangedEvent editModeChangedEvent;

        /// <summary>
        /// The mocked ElementMovedEvent event.
        /// </summary>
        private MockElementMovedEvent elementMovedEvent;

        /// <summary>
        /// The mocked ProjectService service.
        /// </summary>
        private MockProjectService projectService;

        /// <summary>
        /// The mocked SmpteTimeCodeChangedEvent event.
        /// </summary>
        private MockSmpteTimecodeChangedEvent smpteTimeCodeChangedEvent;

        /// <summary>
        /// The mocked DownloadProgressChangedEvent event.
        /// </summary>
        private MockDownloadProgressChangedEvent downloadProgressChangedEvent;

        /// <summary>
        /// The mocked PositionDoubleClickedEvent event.
        /// </summary>
        private MockPositionDoubleClickedEvent positionDoubleClickedEvent;

        /// <summary>
        /// The mocked RefreshElementsEvent event.
        /// </summary>
        private MockRefreshElementsEvent refreshElementsEvent;

        /// <summary>
        /// The mocked ThumbnailEvent event.
        /// </summary>
        private MockPickThumbnailEvent pickThumbnailEvent;

        /// <summary>
        /// The mocked PlayerEvent event.
        /// </summary>
        private MockPlayerEvent playerEvent;

        /// <summary>
        /// The mocked Caretaker.
        /// </summary>
        private MockCaretaker caretaker;

        /// <summary>
        /// The mocked ConfigurationService service.
        /// </summary>
        private MockConfigurationService configurationService;

        /// <summary>
        /// Initializes resources need it by the tests.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.view = new MockTimelineView();
            this.timelineModel = new MockTimelineModel();
            this.eventAggregator = new MockEventAggregator();
            this.projectService = new MockProjectService();
            this.caretaker = new MockCaretaker();
            this.configurationService = new MockConfigurationService();

            this.addAssetEvent = new MockAddAssetEvent();
            this.deleteMediaBinAssetEvent = new MockDeleteMediaBinAssetEvent();
            this.pauseEvent = new MockPauseEvent();
            this.positionUpdatedEvent = new MockPositionUpdatedEvent();
            this.playheadMovedEvent = new MockPlayheadMovedEvent();
            this.editModeChangedEvent = new MockEditModeChangedEvent();
            this.elementMovedEvent = new MockElementMovedEvent();
            this.addAssetToTimelineEvent = new MockAddAssetToTimelineEvent();
            this.smpteTimeCodeChangedEvent = new MockSmpteTimecodeChangedEvent();
            this.startTimeCodeChangedEvent = new MockStartTimeCodeChangedEvent();
            this.downloadProgressChangedEvent = new MockDownloadProgressChangedEvent();
            this.positionDoubleClickedEvent = new MockPositionDoubleClickedEvent();
            this.refreshElementsEvent = new MockRefreshElementsEvent();
            this.playerEvent = new MockPlayerEvent();
            this.pickThumbnailEvent = new MockPickThumbnailEvent();

            this.eventAggregator.AddMapping<AddAssetEvent>(this.addAssetEvent);
            this.eventAggregator.AddMapping<DeleteMediaBinAssetEvent>(this.deleteMediaBinAssetEvent);
            this.eventAggregator.AddMapping<PauseEvent>(this.pauseEvent);
            this.eventAggregator.AddMapping<PositionUpdatedEvent>(this.positionUpdatedEvent);
            this.eventAggregator.AddMapping<PlayheadMovedEvent>(this.playheadMovedEvent);
            this.eventAggregator.AddMapping<EditModeChangedEvent>(this.editModeChangedEvent);
            this.eventAggregator.AddMapping<ElementMovedEvent>(this.elementMovedEvent);
            this.eventAggregator.AddMapping<SmpteTimeCodeChangedEvent>(this.smpteTimeCodeChangedEvent);
            this.eventAggregator.AddMapping<AddAssetToTimelineEvent>(this.addAssetToTimelineEvent);
            this.eventAggregator.AddMapping<StartTimeCodeChangedEvent>(this.startTimeCodeChangedEvent);
            this.eventAggregator.AddMapping<DownloadProgressChangedEvent>(this.downloadProgressChangedEvent);
            this.eventAggregator.AddMapping<PositionDoubleClickedEvent>(this.positionDoubleClickedEvent);
            this.eventAggregator.AddMapping<RefreshElementsEvent>(this.refreshElementsEvent);
            this.eventAggregator.AddMapping<PlayerEvent>(this.playerEvent);
            this.eventAggregator.AddMapping<PickThumbnailEvent>(this.pickThumbnailEvent);
        }

        /// <summary>
        /// Tests that the constructor sets the default duration.
        /// </summary>
        [TestMethod]
        public void ConstructorSetsDefaultDuration()
        {
            Assert.IsFalse(this.view.SetDurationCalled);

            var presenter = this.CreatePresenter();
            
            Assert.IsTrue(this.view.SetDurationCalled);
            Assert.AreEqual(TimelinePresenter.DefaultTimelineDuration, this.view.SetDurationArgument.TotalSeconds);
            Assert.AreEqual(TimelinePresenter.DefaultTimelineDuration, this.timelineModel.Duration.TotalSeconds);
        }

        /// <summary>
        /// Should set the presenter into view.
        /// </summary>
        [TestMethod]
        public void ShouldSetPresenterlIntoView()
        {
            var presenter = this.CreatePresenter();
            Assert.AreSame(presenter, this.view.Model);
        }

        /// <summary>
        /// Tests that the RemoveElementCommand to all the elements of a video asset when the DeleteMediaBinAsset event is being published.
        /// </summary>
        [TestMethod]
        public void ShouldCallToRemoveElementCommandToAllTheElementsOfTheGivenAssetByLookingForSameProviderUriWithDeleteMediaBinAssetEvent()
        {
            var asset = new VideoAsset { ProviderUri = new Uri("http://test") };
            var otherAsset = new VideoAsset { ProviderUri = new Uri("http://other") };

            var timelineElement0 = new TimelineElement { Asset = asset };
            var timelineElement1 = new TimelineElement { Asset = asset };
            var timelineElement2 = new TimelineElement { Asset = otherAsset };

            var track = new Track { TrackType = TrackType.Visual };

            track.Shots.Add(timelineElement0);
            track.Shots.Add(timelineElement1);
            track.Shots.Add(timelineElement2);

            this.timelineModel.Tracks.Add(track);
            
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.deleteMediaBinAssetEvent.SubscribeArgumentAction(asset);

            Assert.AreEqual(2, this.caretaker.ExecuteCommandNumberOfCalls);
            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(RemoveElementCommand));
        }

        /// <summary>
        /// Tests that the RemoveElementCommand to all the elements of an audio asset when the DeleteMediaBinAsset event is being published.
        /// </summary>
        [TestMethod]
        public void ShouldCallToRemoveElementCommandToAllTheElementsOfTheGivenAssetByLookingForSameIdWithDeleteMediaBinAssetEvent()
        {
            var asset = new AudioAsset();
            var otherAsset = new AudioAsset();

            var timelineElement0 = new TimelineElement { Asset = asset };
            var timelineElement1 = new TimelineElement { Asset = asset };
            var timelineElement2 = new TimelineElement { Asset = otherAsset };

            var track = new Track { TrackType = TrackType.Audio };

            track.Shots.Add(timelineElement0);
            track.Shots.Add(timelineElement1);
            track.Shots.Add(timelineElement2);

            this.timelineModel.Tracks.Add(track);

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.deleteMediaBinAssetEvent.SubscribeArgumentAction(asset);

            Assert.AreEqual(2, this.caretaker.ExecuteCommandNumberOfCalls);
            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(RemoveElementCommand));
        }

        /// <summary>
        /// Shoulds the call show asset download progress on view with download progress changed event subscription.
        /// </summary>
        [TestMethod]
        public void ShouldCallToShowAssetDownloadProgressOnViewWithDownloadProgressChangedEventSubscription()
        {
            var payload = new AssetDownloadProgressEventArgs(new TimelineElement(), 0.5, 5);
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.ShowAssetDownloadProgressCalled);

            this.downloadProgressChangedEvent.SubscribeArgumentAction(payload);

            Assert.IsTrue(this.view.ShowAssetDownloadProgressCalled);
        }

        /// <summary>
        /// Tests that the AddElement method should be called when invoking the ElementAdded event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToAddElementOnViewWhenInvokingElementAddedEvent()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.AddElementCalled);

            this.timelineModel.InvokeElementAdded(new TimelineElement());

            Assert.IsTrue(this.view.AddElementCalled);
        }

        /// <summary>
        /// Tests that the UnselectElement method should be called when invoking the ElementAdded event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToUnselectElementOnViewWhenInvokingElementAddedEvent()
        {
            var selectedTimelineElement = new TimelineElement();
            var newTimelineElement = new TimelineElement { Asset = new VideoAsset() };

            var presenter = this.CreatePresenter();

            this.view.InvokeElementSelect(selectedTimelineElement, TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2997NonDrop));

            Assert.IsFalse(this.view.UnselectElementCalled);

            this.timelineModel.InvokeElementAdded(newTimelineElement);

            Assert.IsTrue(this.view.UnselectElementCalled);
            Assert.AreEqual(selectedTimelineElement, this.view.UnselectElementArgument);
        }

        /// <summary>
        /// Tests that the UndoLevel should be retrieved from the ConfigurationService.
        /// </summary>
        [TestMethod]
        public void ShouldCallToGetUndoLevelOnConfigurationService()
        {
            bool getUndoLevelCalled = false;
            this.configurationService.GetParameterValueReturnFunction = parameter =>
            {
                if (parameter == "UndoLevel")
                {
                    getUndoLevelCalled = true;
                }

                return string.Empty;
            };

            var presenter = this.CreatePresenter();

            Assert.IsTrue(getUndoLevelCalled);
        }

        /// <summary>
        /// Tests that the UndoLevel should be set on the Caretaker.
        /// </summary>
        [TestMethod]
        public void ShouldCallToSetUndoLevelOnCaretaker()
        {
            Assert.IsFalse(this.caretaker.SetUndoLevelCalled);

            var presenter = this.CreatePresenter();

            Assert.IsTrue(this.caretaker.SetUndoLevelCalled);
        }

        /// <summary>
        /// Tests that the RemoveElement should be called when invoking the ElementRemoved event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToRemoveElementOnViewWhenInvokingElementRemovedEvent()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.RemoveElementCalled);

            this.timelineModel.InvokeElementRemoved(new TimelineElement());

            Assert.IsTrue(this.view.RemoveElementCalled);
        }

        /// <summary>
        /// Test that the RemoveElement should be called when invoking the ElementRemoved event even if
        /// the removed element is the selected.
        /// </summary>
        [TestMethod]
        public void ShouldCallToRemoveElementOnViewWhenInvokingElementRemovedEventEvenIfTheElementIsTheSelectedOne()
        {
            var timelineElement = new TimelineElement();
            var presenter = this.CreatePresenter();

            this.view.InvokeElementSelect(timelineElement, TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2997NonDrop));

            Assert.IsFalse(this.view.RemoveElementCalled);

            this.timelineModel.InvokeElementRemoved(timelineElement);

            Assert.IsTrue(this.view.RemoveElementCalled);
        }

        /// <summary>
        /// Tests that the ShowLink method should be called when invoking the ElementLinked event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToShowLinkOnViewWhenInvokingElementLinkedEvent()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var previousElement = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var currentElement = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(previousElement);
            track.Shots.Add(currentElement);

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position)
                {
                    return previousElement;
                }
                else
                {
                    return null;
                }
            };

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.ShowLinkCalled);

            this.timelineModel.InvokeElementLinked(currentElement);

            Assert.IsTrue(this.view.ShowLinkCalled);
        }

        /// <summary>
        /// Tests that the HideLink method should be called when invoking the ElementUnliked event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToHideLinkOnViewWhenInvokingElementUnlinkedEvent()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.HideLinkCalled);

            this.timelineModel.InvokeElementUnlinked(new TimelineElement());

            Assert.IsTrue(this.view.HideLinkCalled);
        }

        /// <summary>
        /// Tests that the ShowLink method should not be called when invoking the ElementLinked event
        /// if the element is null.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallToShowLinkOnViewWhenInvokingElementLinkedEventWithNullElement()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.ShowLinkCalled);

            this.timelineModel.InvokeElementLinked(null);

            Assert.IsFalse(this.view.ShowLinkCalled);
        }

        /// <summary>
        /// Tests that the HideLink method should not be called when invoking the ElementUnlinked event
        /// if the element is null.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallToHideLinkOnViewWhenInvokingElementUnlinkedEventWithNullElement()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.HideLinkCalled);

            this.timelineModel.InvokeElementUnlinked(null);

            Assert.IsFalse(this.view.HideLinkCalled);
        }

        /// <summary>
        /// Tests that the Drop Command should not be executed if the MouseEventArgs is null.
        /// </summary>
        [TestMethod]
        public void ShouldNotCanExecuteDropCommandIfMouseEventArgsIsNull()
        {
            var presenter = this.CreatePresenter();

            var payload = new DropPayload
            {
                DraggedItem = new VideoAsset(),
                MouseEventArgs = null
            };

            var result = presenter.DropCommand.CanExecute(payload);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that the AddElementCommand should not be executed if the asset dropped has not layer resolved.
        /// </summary>
        [TestMethod]
        public void ShouldNotExecuteAddElementCommandOnCaretakerWhenDropAssetIfNoLayerIsResolved()
        {
            var presenter = this.CreatePresenter();

            this.view.MockedResolvedLayerPosition = null;

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

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            presenter.DropCommand.Execute(payload);

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);
        }

        /// <summary>
        /// Tests that the AddElementCommand should be executed when a video asset is dropped.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteAddElementCommandOnCaretakerWhenDropVideoAsset()
        {
            var presenter = this.CreatePresenter();

            this.view.MockedResolvedLayerPosition = new LayerPosition
            {
                LayerType = TrackType.Visual,
                Position = TimeCode.FromAbsoluteTime(10, SmpteFrameRate.Smpte30)
            };

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

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            presenter.DropCommand.Execute(payload);

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(AddElementCommand));
        }

        /// <summary>
        /// Tests that the AddElementCommand should be executed when an image asset is dropped.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteAddElementCommandOnCaretakerWhenDropImageAsset()
        {
            var presenter = this.CreatePresenter();

            this.view.MockedResolvedLayerPosition = new LayerPosition
            {
                LayerType = TrackType.Visual,
                Position = TimeCode.FromAbsoluteTime(3, SmpteFrameRate.Smpte30)
            };

            var payload = new DropPayload
            {
                DraggedItem = new ImageAsset
                {
                    Title = "Test Image #1"
                },
                MouseEventArgs = null
            };

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            presenter.DropCommand.Execute(payload);

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(AddElementCommand));
        }

        /// <summary>
        /// Tests that the AddElementCommand should be executed when an audio asset is dropped.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteAddElementCommandOnCaretakerWhenDropAudioAsset()
        {
            var presenter = this.CreatePresenter();

            this.view.MockedResolvedLayerPosition = new LayerPosition
            {
                LayerType = TrackType.Audio,
                Position = TimeCode.FromAbsoluteTime(5, SmpteFrameRate.Smpte30)
            };

            var payload = new DropPayload
            {
                DraggedItem = new AudioAsset
                {
                    Duration = 2,
                    Title = "Test Audio #1"
                },
                MouseEventArgs = null
            };

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            presenter.DropCommand.Execute(payload);

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(AddElementCommand));
        }

        /// <summary>
        /// Tests that the RemoveElementCommand should be executed when invoking to Delete event.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteRemoveElementCommandWhenInvokingToDelete()
        {
            var presenter = this.CreatePresenter();

            var element = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte30,
                    Title = "Test Video #3"
                },
                InPosition = TimeCode.FromAbsoluteTime(250, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(750, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(5000, SmpteFrameRate.Smpte30)
            };

            this.view.InvokeElementSelect(element, TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30));

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.view.InvokeDelete();

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(RemoveElementCommand));
        }

        /// <summary>
        /// Tests that the RemoveElementCommand should not be called when invoking to Delete event 
        /// if there is no element selected.
        /// </summary>
        [TestMethod]
        public void ShouldNotExecuteRemoveElementCommandWhenInvokingToDeleteIfNoElementIsSelected()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.view.InvokeDelete();

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);
        }

        /// <summary>
        /// Should call to MoveElement on Model and to RefreshElement on View when invoking element position change.
        /// </summary>
        [TestMethod]
        public void ShouldCallToMoveElementOnModelAndRefreshElementOnViewWhenChangingPosition()
        {
            var presenter = this.CreatePresenter();

            var element = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte30,
                    Title = "Test Video #3"
                },
                InPosition = TimeCode.FromAbsoluteTime(250, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(750, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(5000, SmpteFrameRate.Smpte30)
            };

            var newPosition = TimeCode.FromAbsoluteTime(2000, this.view.SetDurationArgument.FrameRate);

            this.view.InvokeElementSelect(element, TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30));

            Assert.IsFalse(this.timelineModel.MoveElementCalled);
            Assert.IsFalse(this.view.RefreshElementCalled);

            this.view.InvokeElementPositionChange(newPosition);

            Assert.IsTrue(this.timelineModel.MoveElementCalled);
            Assert.IsTrue(this.view.RefreshElementCalled);
        }

        /// <summary>
        /// Tests that the moving an element to end of timeline should fixes position.
        /// </summary>
        [TestMethod]
        public void ShouldMoveElementToEndOfTimelineFixesPosition()
        {
            var presenter = this.CreatePresenterWithDemoData();
            var element = this.timelineModel.Tracks[0].Shots[2];
            var newPosition = TimeCode.FromAbsoluteTime(10000, this.view.SetDurationArgument.FrameRate);
            var newFixedPosition = TimeCode.FromAbsoluteTime(9700, this.view.SetDurationArgument.FrameRate);

            this.view.InvokeElementSelect(element, TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30));
            this.view.InvokeElementPositionChange(newPosition);

            Assert.AreEqual(newFixedPosition, this.timelineModel.MoveElementNewPositionArgument);
        }

        /// <summary>
        /// Tests that the Image MarkOut should be adjusted.
        /// </summary>
        [TestMethod]
        public void AdjustImageMarkOut()
        {
            var presenter = this.CreatePresenterWithDemoData();
            var element = this.timelineModel.Tracks[0].Shots[2];

            this.view.InvokeElementSelect(element, TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30));
            this.view.DoMoveElementMarkOut(TimeCode.FromAbsoluteTime(2000, this.view.SetDurationArgument.FrameRate));

            Assert.AreEqual(500, element.Duration.TotalSeconds);
            Assert.IsFalse(this.timelineModel.MoveElementCalled);
        }

        /// <summary>
        /// Tests that the Image MarkIn should be adjusted.
        /// </summary>
        [TestMethod]
        public void ShouldAdjustImageMarkIn()
        {
            var presenter = this.CreatePresenterWithDemoData();
            var element = this.timelineModel.Tracks[0].Shots[2];

            this.view.InvokeElementSelect(element, TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30));
            this.view.DoMoveElementMarkIn(TimeCode.FromAbsoluteTime(1600, this.view.SetDurationArgument.FrameRate));

            Assert.AreEqual(200, element.Duration.TotalSeconds);
            Assert.IsTrue(this.timelineModel.MoveElementCalled);
            Assert.AreEqual(TimeCode.FromAbsoluteTime(1600, this.view.SetDurationArgument.FrameRate), this.timelineModel.MoveElementNewPositionArgument);
        }

        /// <summary>
        /// Tests that the elements should be splitted when the SplitEvent is invoked.
        /// </summary>
        [TestMethod]
        public void ShouldSplitElementWhenSplitEventIsRaised()
        {
            var presenter = this.CreatePresenter();

            this.timelineModel.CurrentPosition = TimeCode.FromAbsoluteTime(5200, SmpteFrameRate.Smpte30);

            var element = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte30,
                    Title = "Test Video #3"
                },
                InPosition = TimeCode.FromAbsoluteTime(100, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(500, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(5000, SmpteFrameRate.Smpte30)
            };

            this.timelineModel.Tracks.Add(new Track());
            this.timelineModel.Tracks[0].Shots.Add(element);

            this.timelineModel.GetElementsAtPositionReturnValue = new List<TimelineElement> { element };

            var expectedOutPosition = this.timelineModel.CurrentPosition.TotalSeconds - element.Position.TotalSeconds  + element.InPosition.TotalSeconds;

            this.view.InvokeElementSelect(element, TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30));

            Assert.IsFalse(this.view.RefreshElementCalled);
            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.view.InvokeSplit();

            Assert.IsTrue(this.view.RefreshElementCalled);
            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);

            Assert.AreEqual(expectedOutPosition, this.view.RefreshElementArgument[0].OutPosition.TotalSeconds);
            
            // Assert.AreEqual(this.view.RefreshElementArgument[0].OutPosition, this.timelineModel.AddElementArgument.InPosition);
            // Assert.AreEqual(element.Position + this.view.RefreshElementArgument[0].Duration, this.timelineModel.AddElementArgument.Position);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(AddElementCommand));
        }

        /// <summary>
        /// Tests that no split operation should be performed if there is no elements under the playhead position.
        /// </summary>
        [TestMethod]
        public void ShouldNotSplitIfNoElementIsUnderThePlayheadPosition()
        {
            var presenter = this.CreatePresenter();

            this.timelineModel.CurrentPosition = TimeCode.FromAbsoluteTime(4200, SmpteFrameRate.Smpte30);

            var element = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte30,
                    Title = "Test Video #3"
                },
                InPosition = TimeCode.FromAbsoluteTime(100, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(500, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(5000, SmpteFrameRate.Smpte30)
            };

            this.timelineModel.Tracks.Add(new Track());
            this.timelineModel.Tracks[0].Shots.Add(element);

            this.timelineModel.GetElementsAtPositionReturnValue = new List<TimelineElement>();

            Assert.IsFalse(this.view.RefreshElementCalled);
            Assert.IsFalse(this.timelineModel.AddElementCalled);

            this.view.InvokeSplit();

            Assert.IsFalse(this.view.RefreshElementCalled);
            Assert.IsFalse(this.timelineModel.AddElementCalled);
        }

        /// <summary>
        /// Tests that the PauseEvent should be published when invokin to MovingPlayhead event.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPauseEventWhenInvokingToMovingPlayheadEvent()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.pauseEvent.PublishCalled);

            this.view.InvokeMovingPlayhead();

            Assert.IsTrue(this.pauseEvent.PublishCalled);
        }

        /// <summary>
        /// Tests that the SmpteFrameRate should be updated when the SmpteTimeCodeChangedEvent is being published.
        /// </summary>
        [TestMethod]
        public void ShouldUpdateSmpteFrameRateWithSmpteTimecodeChangedEventSubscription()
        {
            var frameRate = SmpteFrameRate.Smpte25;

            Assert.IsFalse(this.view.SetDurationCalled);

            var presenter = this.CreatePresenter();

            Assert.IsTrue(this.view.SetDurationCalled);

            this.view.SetDurationCalled = false;

            Assert.IsFalse(this.view.SetDurationCalled);

            this.smpteTimeCodeChangedEvent.SubscribeArgumentAction(frameRate);

            Assert.IsTrue(this.view.SetDurationCalled);
            Assert.AreEqual(frameRate, this.view.SetDurationArgument.FrameRate);
        }

        /// <summary>
        /// Tests that the Undo method of the Caretaker is being called when invoking to Undo event.
        /// </summary>
        [TestMethod]
        public void ShouldUndoOnCaretakerWhenInvokingToUndo()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.caretaker.UndoCalled);

            this.view.InvokeUndo();

            Assert.IsTrue(this.caretaker.UndoCalled);
        }

        /// <summary>
        /// Tests that the Redo method of the Caretaker is being callend when invoking to Redo event.
        /// </summary>
        [TestMethod]
        public void ShouldRedoOnCaretakerWhenInvokingToRedo()
        {
            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.caretaker.RedoCalled);

            this.view.InvokeRedo();

            Assert.IsTrue(this.caretaker.RedoCalled);
        }

        /// <summary>
        /// Tests that the PublishedEditModeChangedEvent event should be published when the ToggleEditMode
        /// is invoked.
        /// </summary>
        [TestMethod]
        public void ShouldPublishEditModeChangedEventEventIfToggleEditModeEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            this.editModeChangedEvent.PublishCalled = false;
            
            this.view.InvokeToggleEditMode();

            Assert.IsTrue(this.editModeChangedEvent.PublishCalled);
        }

        /// <summary>
        /// Tests that the PublishedEditModeChangedEvent event should be published when the IsInRippleMode property is changed.
        /// </summary>
        [TestMethod]
        public void ShouldPublishEditModeChangedEventEventIfIsInRippleModeIsChanged()
        {
            var presenter = this.CreatePresenter();

            this.editModeChangedEvent.PublishCalled = false;

            presenter.IsInRippleMode = true;

            Assert.IsTrue(this.editModeChangedEvent.PublishCalled);
        }

        /// <summary>
        /// Tests that SetStartTimeCode method should be called when the StartTimeCodeChangedEvent is being published.
        /// </summary>
        [TestMethod]
        public void ShouldCallToSetStartTimeCodeOnViewWithStartTimeCodeChangedEventSubscription()
        {
            var timeCode = TimeCode.FromHours(1, SmpteFrameRate.Smpte2997NonDrop);

            var presenter = this.CreatePresenter();

            this.view.SetStartTimeCodeCalled = false;

            this.startTimeCodeChangedEvent.SubscribeArgumentAction(timeCode);

            Assert.IsTrue(this.view.SetStartTimeCodeCalled);
            Assert.AreEqual(timeCode, this.view.SetStartTimeCodeArgument);
        }

        /// <summary>
        /// Tests that the AddElementCommand should be executed when the AddAssetToTimelineEvent is being published.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteAddElementCommandOnCaretakerWithAddAssetToTimelineEventSubscription()
        {
            var presenter = this.CreatePresenter();

            this.view.MockedResolvedLayerPosition = new LayerPosition
            {
                LayerType = TrackType.Visual,
                Position = TimeCode.FromAbsoluteTime(3, SmpteFrameRate.Smpte30)
            };

            var asset = new ImageAsset { Title = "Test Image #1" };

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.addAssetToTimelineEvent.SubscribeArgumentAction(asset);

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(AddElementCommand));
        }

        /// <summary>
        /// Tests that the REfreshElement method should be callend when invoking the ElementMovedEvent of the TimelineModel.
        /// </summary>
        [TestMethod]
        public void ShouldCallToRefreshElementOnViewWhenInvokingTimelineModelElementMovedEvent()
        {
            var timelineElement = new TimelineElement();

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.RefreshElementCalled);

            this.timelineModel.InvokeElementMoved(timelineElement);

            Assert.IsTrue(this.view.RefreshElementCalled);
            Assert.AreEqual(timelineElement, this.view.RefreshElementArgument[0]);
        }

        /// <summary>
        /// Tests that the SelectElement method should be called when invoking the SelectElement event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToSelectElementOnViewWhenInvokingSelectElementEvent()
        {
            var timelineElement = new TimelineElement();

            var presenter = this.CreatePresenter();

            Assert.IsFalse(this.view.SelectElementCalled);

            this.view.InvokeElementSelect(timelineElement, TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2997Drop));

            Assert.IsTrue(this.view.SelectElementCalled);
            Assert.AreEqual(timelineElement, this.view.SelectElementArgument);
        }

        /// <summary>
        /// Tests that the UnselectElement method should be called when invoking the SeelectElement event if there is an element selected.
        /// </summary>
        [TestMethod]
        public void ShouldCallToUnselectElementOnViewWhenInvokingSelectElementEventIfThereIsAnElementSelected()
        {
            var selectedTimelineElement = new TimelineElement();
            var newSelectedTimelineElement = new TimelineElement() { Asset = new VideoAsset() };

            var presenter = this.CreatePresenter();

            this.view.InvokeElementSelect(selectedTimelineElement, TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2997Drop));

            Assert.IsFalse(this.view.UnselectElementCalled);

            this.view.InvokeElementSelect(newSelectedTimelineElement, TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2997Drop));

            Assert.IsTrue(this.view.UnselectElementCalled);
            Assert.AreEqual(selectedTimelineElement, this.view.UnselectElementArgument);
        }

        /// <summary>
        /// Tests that the ExecuteLayerSnapshotCommand commadn should be executed when the StopMovingEvent is invoked if the StartMovingEvent was previously invoked.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteLayerSnapshotCommandOnCaretakerWhenStopMovingEventIsInvokedIfStartMovingEventWasInvoked()
        {
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var track = new Track { TrackType = TrackType.Visual };

            track.Shots.Add(timelineElement);

            this.timelineModel.Tracks.Add(track);

            var presenter = this.CreatePresenter();

            this.view.InvokeStartMoving(timelineElement);

            Assert.IsFalse(this.caretaker.ExecuteCommandCalled);

            this.view.InvokeStopMoving(timelineElement);

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(LayerSnapshotCommand));
        }

        /// <summary>
        /// Tests that the PositionDoubleClickedEvent event should be published when invoking the TopBarDoubleClicked event.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPositionDoubleClickedEventWhenTopBarDoubleClickedEventIsInvoked()
        {
            var payload = new PositionPayloadEventArgs(TimeSpan.FromSeconds(10));

            var presenter = this.CreatePresenter();

            this.positionDoubleClickedEvent.PublishCalled = false;

            this.view.InvokeTopBarDoubleClicked(payload);

            Assert.IsTrue(this.positionDoubleClickedEvent.PublishCalled);
            Assert.AreEqual(payload, this.positionDoubleClickedEvent.PublishArgumentPayload);
        }

        /// <summary>
        /// Tests that the RefreshElementsEvent event should be published when invoking the RefreshingElements event.
        /// </summary>
        [TestMethod]
        public void ShouldPublishRefreshElementsEventEventWhenRefresingElementsEventIsInvoked()
        {
            var payload = new RefreshElementsEventArgs(10);

            var presenter = this.CreatePresenter();

            this.refreshElementsEvent.PublishCalled = false;

            this.view.InvokeRefreshingElements(payload);

            Assert.IsTrue(this.refreshElementsEvent.PublishCalled);
            Assert.AreEqual(payload, this.refreshElementsEvent.PublishArgumentPayload);
        }

        /// <summary>
        /// Tests that the PlayerEvent should be published when invoking the TogglePlay event.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPlayerEventEventWhenTogglePlayEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            this.playerEvent.PublishCalled = false;

            this.view.InvokeTogglePlay();

            Assert.IsTrue(this.playerEvent.PublishCalled);
            Assert.AreEqual(PlayerMode.Timeline, this.playerEvent.PublishArgumentPayload.PlayerMode);
        }

        /// <summary>
        /// Tests that the HideLinks method should be callend when invoking the HidingLinks event.
        /// </summary>
        [TestMethod]
        public void ShouldCallToHideLinksOnViewWhenHidingLinksEventIsInvoked()
        {
            var timelineElement = new TimelineElement();
            var presenter = this.CreatePresenter();

            this.view.HideLinkCalled = false;

            this.view.InvokeHidingLinks(timelineElement);

            Assert.IsTrue(this.view.HideLinkCalled);
        }

        /// <summary>
        /// Tests that ShowLinks should be called when ShowLinks event is invoked with InPosition as LinkPosition
        /// and if there is an element in next position.
        /// </summary>
        [TestMethod]
        public void ShouldCallToShowLinksOnViewWhenShowLinksEventIsInvokedWithInPositionIfThereIsAElementInPreviuosPosition()
        {
            var timelineElement = new TimelineElement
                                      {
                                          Position = TimeCode.FromSeconds(60d, SmpteFrameRate.Smpte2997Drop),
                                          InPosition = TimeCode.FromSeconds(10d, SmpteFrameRate.Smpte2997Drop),
                                          OutPosition = TimeCode.FromSeconds(40d, SmpteFrameRate.Smpte2997Drop)
                                      };

            var previousElement = new TimelineElement();
            var presenter = this.CreatePresenter();

            this.view.ShowLinkCalled = false;

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
                                                                        {
                                                                            if (this.timelineModel.GetElementAtPositionPositionArgument == timelineElement.Position)
                                                                            {
                                                                                return previousElement;
                                                                            }

                                                                            return null;
                                                                        };

            this.view.InvokeShowingLinks(timelineElement);

            Assert.IsTrue(this.view.ShowLinkCalled);
            Assert.AreEqual(LinkPosition.In, this.view.ShowLinkLinkPositionArgument);
        }

        /// <summary>
        /// Tests that the ShowLinks method should be callend when invoking the ShowLinksEvent with OutPosition as LinkPosition
        /// and if there is an element in a next position.
        /// </summary>
        [TestMethod]
        public void ShouldCallToShowLinksOnViewWhenShowLinksEventIsInvokedWithOutPositionIfThereIsAElementInNextPosition()
        {
            var timelineElement = new TimelineElement
            {
                Position = TimeCode.FromSeconds(60d, SmpteFrameRate.Smpte2997Drop),
                InPosition = TimeCode.FromSeconds(10d, SmpteFrameRate.Smpte2997Drop),
                OutPosition = TimeCode.FromSeconds(40d, SmpteFrameRate.Smpte2997Drop)
            };

            var nextElement = new TimelineElement();
            var presenter = this.CreatePresenter();

            this.view.ShowLinkCalled = false;

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == timelineElement.Position + timelineElement.Duration)
                {
                    return nextElement;
                }

                return null;
            };

            this.view.InvokeShowingLinks(timelineElement);

            Assert.IsTrue(this.view.ShowLinkCalled);
            Assert.AreEqual(LinkPosition.Out, this.view.ShowLinkLinkPositionArgument);
        }

        /// <summary>
        /// Tests that the ToggleLinkElementCommand should be executed when the LinkingElement event is invoked.
        /// </summary>
        [TestMethod]
        public void ShouldExecuteToggleLinkElementCommandOnCaretakerWhenLinkingElementIsInvoked()
        {
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var presenter = this.CreatePresenter();

            this.caretaker.ExecuteCommandCalled = false;

            this.view.InvokeLinkingElement(new LinkElementEventArgs(timelineElement, LinkPosition.In));

            Assert.IsTrue(this.caretaker.ExecuteCommandCalled);
            Assert.IsInstanceOfType(this.caretaker.ExecuteCommandArgument, typeof(ToggleLinkElementCommand));
        }

        /// <summary>
        /// Tests that the PickThumbnailEvent event should be published when the PickThumbnail event
        /// is invoked.
        /// </summary>
        [TestMethod]
        public void ShouldPublishPickThumbnailEventEventIfPickThumbnailEventIsInvoked()
        {
            var presenter = this.CreatePresenter();

            this.pickThumbnailEvent.PublishCalled = false;

            this.view.InvokePickThumbnail();

            Assert.IsTrue(this.pickThumbnailEvent.PublishCalled);
        }

        [TestMethod]
        public void ShouldNotCanExecuteRemoveAudioTrackCommandIfThereIsOnlyOneAudioTrack()
        {
            this.projectService.GetCurrentProjectReturnValue = new RCE.Infrastructure.Models.Project
            {
                Timeline =
                    {
                        new Track { TrackType = TrackType.Audio, Number = 1 },
                    }
            };
            var presenter = this.CreatePresenter();

            this.projectService.InvokeProjectRetrieved();

            bool result = presenter.RemoveAudioTrackCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldCanExecuteRemoveAudioTrackCommandIfThereAreMoreThanOneAudioTrack()
        {
            this.projectService.GetCurrentProjectReturnValue = new RCE.Infrastructure.Models.Project
            {
                Timeline =
                    {
                        new Track { TrackType = TrackType.Audio, Number = 1 },
                        new Track { TrackType = TrackType.Audio, Number = 2 },
                    }
            };
            var presenter = this.CreatePresenter();

            this.projectService.InvokeProjectRetrieved();

            bool result = presenter.RemoveAudioTrackCommand.CanExecute(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldNotCanExecuteAddAudioTrackCommandIfMaxAudioTrackLimitWasReached()
        {
            this.projectService.GetCurrentProjectReturnValue = new RCE.Infrastructure.Models.Project
            {
                Timeline =
                    {
                        new Track { TrackType = TrackType.Audio, Number = 1 },
                        new Track { TrackType = TrackType.Audio, Number = 2 },
                    }
            };

            this.configurationService.GetParameterValueReturnFunction = parameter => parameter == "MaxNumberOfAudioTracks" ? "2" : null;

            var presenter = this.CreatePresenter();

            this.projectService.InvokeProjectRetrieved();

            bool result = presenter.AddAudioTrackCommand.CanExecute(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldCanExecuteAddAudioTrackCommandIfMaxAudioTrackLimitWasNotReached()
        {
            this.projectService.GetCurrentProjectReturnValue = new RCE.Infrastructure.Models.Project
            {
                Timeline =
                    {
                        new Track { TrackType = TrackType.Audio, Number = 1 },
                        new Track { TrackType = TrackType.Audio, Number = 2 },
                    }
            };

            this.configurationService.GetParameterValueReturnFunction = parameter => parameter == "MaxNumberOfAudioTracks" ? "5" : null;

            var presenter = this.CreatePresenter();

            this.projectService.InvokeProjectRetrieved();

            bool result = presenter.AddAudioTrackCommand.CanExecute(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldAddAudioTrackWhenExecutingAddAudioTrackCommand()
        {
            this.projectService.GetCurrentProjectReturnValue = new RCE.Infrastructure.Models.Project
                                                                   {
                                                                       Timeline = { new Track { TrackType = TrackType.Audio, Number = 1 } }
                                                                   };
            var presenter = this.CreatePresenter();

            this.projectService.InvokeProjectRetrieved();

            presenter.AddAudioTrackCommand.Execute(null);

            Assert.AreEqual(2, presenter.AudioTracks.Count);
            Assert.AreEqual(2, presenter.AudioTracks[1].Number);
        }

        [TestMethod]
        public void ShouldRemoveAudioTrackWhenExecutingAddAudioTrackCommand()
        {
            this.projectService.GetCurrentProjectReturnValue = new RCE.Infrastructure.Models.Project
            {
                Timeline =
                    {
                        new Track { TrackType = TrackType.Audio, Number = 1 },
                        new Track { TrackType = TrackType.Audio, Number = 2 }
                    }
            };
            var presenter = this.CreatePresenter();

            this.projectService.InvokeProjectRetrieved();

            presenter.RemoveAudioTrackCommand.Execute(null);

            Assert.AreEqual(1, presenter.AudioTracks.Count);
            Assert.AreEqual(1, presenter.AudioTracks[0].Number);
        }

        [TestMethod]
        public void ShouldIncreaseCurrentPositionByOneFrameWhenExecutingMoveFrameCommand()
        {
            TimeCode timeCode = new TimeCode(10, 10, 20, 5, SmpteFrameRate.Smpte24);
            
            var presenter = this.CreatePresenter();
            
            this.timelineModel.CurrentPosition = timeCode;

            presenter.MoveFrameCommand.Execute(1);

            timeCode = timeCode.Add(TimeCode.FromFrames(1, SmpteFrameRate.Smpte24));

            Assert.AreEqual(timeCode, this.timelineModel.CurrentPosition);
        }

        [TestMethod]
        public void ShouldDecreaseCurrentPositionByOneFrameWhenExecutingMoveFrameCommand()
        {
            TimeCode timeCode = new TimeCode(10, 10, 20, 5, SmpteFrameRate.Smpte24);

            var presenter = this.CreatePresenter();

            this.timelineModel.CurrentPosition = timeCode;

            presenter.MoveFrameCommand.Execute(-1);

            timeCode = timeCode.Subtract(TimeCode.FromFrames(1, SmpteFrameRate.Smpte24));

            Assert.AreEqual(timeCode, this.timelineModel.CurrentPosition);
        }

        /// <summary>
        /// Tests that the SnapModeEnable value should be retrieved from the ConfigurationService.
        /// </summary>
        [TestMethod]
        public void ShouldGetIfTheSnapModeIsEnableFromTheConfigurationService()
        {
            this.configurationService.GetParameterValueReturnFunction = parameter =>
            {
                if (parameter == "SnapModeEnabled")
                {
                    return "true";
                }

                return string.Empty;
            };

            var presenter = this.CreatePresenter();

            Assert.AreEqual(bool.Parse("true"), presenter.IsInSnapMode);
        }

        [TestMethod]
        public void ShouldMoveTimelinePositionToNextElementPositionWhenExecutingTheMoveNextClipCommand()
        {
            var element = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var visualTrack = new Track { TrackType = TrackType.Visual };
            visualTrack.Shots.Add(element);

            this.timelineModel.Tracks.Add(visualTrack);

            var presenter = this.CreatePresenter();

            this.timelineModel.CurrentPosition = TimeCode.FromAbsoluteTime(10, this.timelineModel.Duration.FrameRate);

            this.timelineModel.GetNextElementReturnFunction = (position, track) =>
                                                                  {
                                                                      if (position == this.timelineModel.CurrentPosition && track == visualTrack)
                                                                      {
                                                                          return element;
                                                                      }

                                                                      return null;
                                                                  };

            presenter.MoveNextClipCommand.Execute(null);

            Assert.AreEqual((element.Position + TimeCode.FromFrames(1, element.Position.FrameRate)).TotalSeconds, this.timelineModel.CurrentPosition.TotalSeconds);
        }

        [TestMethod]
        public void ShouldMoveTimelinePositionToPreviousElementPositionWhenExecutingTheMovePreviousClipCommand()
        {
            var element = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var visualTrack = new Track { TrackType = TrackType.Visual };
            visualTrack.Shots.Add(element);

            this.timelineModel.Tracks.Add(visualTrack);

            var presenter = this.CreatePresenter();

            this.timelineModel.CurrentPosition = TimeCode.FromAbsoluteTime(50, this.timelineModel.Duration.FrameRate);

            this.timelineModel.GetPreviousElementReturnFunction = (position, track) =>
            {
                if (position == this.timelineModel.CurrentPosition && track == visualTrack)
                {
                    return element;
                }

                return null;
            };

            presenter.MovePreviousClipCommand.Execute(null);

            Assert.AreEqual((element.Position + TimeCode.FromFrames(1, element.Position.FrameRate)).TotalSeconds, this.timelineModel.CurrentPosition.TotalSeconds);
        }

        /// <summary>
        /// Tests if the OnPropertyChanged event is being raised when the IsInRippleMode property change.
        /// </summary>
        [TestMethod]
        public void ShouldRaiseOnPropertyChangedEventWhenIsInRippleModeIsUpdated()
        {
            var propertyChangedRaised = false;
            string propertyChangedEventArgsArgument = null;

            var presenter = this.CreatePresenter();
            presenter.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyChangedEventArgsArgument = e.PropertyName;
            };

            Assert.IsFalse(propertyChangedRaised);
            Assert.IsNull(propertyChangedEventArgsArgument);

            presenter.IsInRippleMode = true;

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual("IsInRippleMode", propertyChangedEventArgsArgument);
        }

        [TestMethod]
        public void ShouldGetCorrectTimelineDurationAfterAddingAnElementToTheTimelineModel()
        {
            var element = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var visualTrack = new Track { TrackType = TrackType.Visual };
            visualTrack.Shots.Add(element);

            var presenter = this.CreatePresenter();

            Assert.AreEqual(0, presenter.TimelineDuration.TotalSeconds);

            this.timelineModel.Tracks.Add(visualTrack);

            Assert.AreEqual(element.Duration.TotalSeconds, presenter.TimelineDuration.TotalSeconds);
        }

        /// <summary>
        /// Creates the TimelinePresenter for testing.
        /// </summary>
        /// <returns>The TimelinePresenter with all the dependencies mocked.</returns>
        private ITimelinePresenter CreatePresenter()
        {
            return new TimelinePresenter(this.view, this.eventAggregator, this.timelineModel, this.projectService, this.caretaker, this.configurationService);
        }

        /// <summary>
        /// Creates the TimelinePresenter for testing.
        /// </summary>
        /// <returns>The TimelinePresenter with all the dependencies mocked and with sample data.</returns>
        private ITimelinePresenter CreatePresenterWithDemoData()
        {
            this.timelineModel.Tracks.Add(new Track { TrackType = TrackType.Visual });
            var presenter = this.CreatePresenter();
            this.timelineModel.Duration = TimeCode.FromAbsoluteTime(10000, SmpteFrameRate.Smpte30);
            this.view.SetDuration(this.timelineModel.Duration);

            // video 1
            // dur: 1000  (0/1000)
            // start: 0
            var element1 = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte24,
                    Title = "Test Video #1"
                },
                InPosition = TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30)
            };

            this.timelineModel.Tracks[0].Shots.Add(element1);

            // video 2
            // dur: 500 (500/1000)
            // start: 1000
            var element2 = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte25,
                    Title = "Test Video #2"
                },
                InPosition = TimeCode.FromAbsoluteTime(500, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30)
            };

            this.timelineModel.Tracks[0].Shots.Add(element2);

            // image 3
            // dur: 300 (300)
            // start: 1500
            var element3 = new TimelineElement
            {
                Asset = new ImageAsset
                {
                    Title = "Test Image #1"
                },
                InPosition = TimeCode.FromAbsoluteTime(0, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(300, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(1500, SmpteFrameRate.Smpte30)
            };

            this.timelineModel.Tracks[0].Shots.Add(element3);

            // video 3
            // dur: 500 (250/750)
            // start: 5000
            var element4 = new TimelineElement
            {
                Asset = new VideoAsset
                {
                    Duration = TimeCode.FromAbsoluteTime(1000, SmpteFrameRate.Smpte30),
                    FrameRate = SmpteFrameRate.Smpte30,
                    Title = "Test Video #3"
                },
                InPosition = TimeCode.FromAbsoluteTime(250, SmpteFrameRate.Smpte30),
                OutPosition = TimeCode.FromAbsoluteTime(750, SmpteFrameRate.Smpte30),
                Position = TimeCode.FromAbsoluteTime(5000, SmpteFrameRate.Smpte30)
            };

            this.timelineModel.Tracks[0].Shots.Add(element4);

            return presenter;
        }
    }
}