// <copyright file="ToggleLinkElementCommandFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ToggleLinkElementCommandFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Timeline.Tests.Commands
{
    using Infrastructure.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using SMPTETimecode;
    using Timeline.Commands;

    /// <summary>
    /// A class for testing the <see cref="ToggleLinkElementCommand"/>.
    /// </summary>
    [TestClass]
    public class ToggleLinkElementCommandFixture
    {
        /// <summary>
        /// The mocked timeline model.
        /// </summary>
        private MockTimelineModel timelineModel;

        /// <summary>
        /// Initializes resources need it by the tests.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.timelineModel = new MockTimelineModel();
        }

        /// <summary>
        /// Tests that LinkPreviousElement method should not be called if there is no previous element.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallToLinkPreviousElementIfThereIsNoPreviousElement()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var currentElement = new TimelineElement
                                    {
                                        Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                                        InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                                        OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
                                    };

            track.Shots.Add(currentElement);

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.In);

            Assert.IsFalse(this.timelineModel.LinkPreviousElementCalled);

            command.Execute();

            Assert.IsFalse(this.timelineModel.LinkPreviousElementCalled);
        }

        /// <summary>
        /// Tests that LinkNextElement method should not be called if there is no next element.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallToLinkNextElementIfThereIsNoNextElement()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(currentElement);

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.Out);

            Assert.IsFalse(this.timelineModel.LinkNextElementCalled);

            command.Execute();

            Assert.IsFalse(this.timelineModel.LinkNextElementCalled);
        }

        /// <summary>
        /// Tests that the LinkPreviousElement method should be called.
        /// </summary>
        [TestMethod]
        public void ShouldCallToLinkPreviousElement()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var previousElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(previousElement);
            track.Shots.Add(currentElement);

            this.timelineModel.IsElementLinkedToReturnValue = false;

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

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.In);

            Assert.IsFalse(this.timelineModel.LinkPreviousElementCalled);

            command.Execute();

            Assert.IsTrue(this.timelineModel.LinkPreviousElementCalled);
        }

        /// <summary>
        /// Tests that the UnlinkPreviousElement method should be called when the command is executed 
        /// if the elements are linked.
        /// </summary>
        [TestMethod]
        public void ShouldCallToUnlinkPreviousElementIfTheElementsAreLinked()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var previousElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(previousElement);
            track.Shots.Add(currentElement);

            this.timelineModel.IsElementLinkedToReturnValue = true;

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

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.In);

            Assert.IsFalse(this.timelineModel.UnlinkElementsCalled);

            command.Execute();

            Assert.IsTrue(this.timelineModel.UnlinkElementsCalled);
        }

        /// <summary>
        /// Tests that the LinkNextElement method should be called.
        /// </summary>
        [TestMethod]
        public void ShouldCallToLinkNextElement()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var nextElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(40, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(currentElement);
            track.Shots.Add(nextElement);

            this.timelineModel.IsElementLinkedToReturnValue = false;

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position + currentElement.Duration)
                {
                    return nextElement;
                }
                else
                {
                    return null;
                }
            };

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.Out);

            Assert.IsFalse(this.timelineModel.LinkNextElementCalled);

            command.Execute();

            Assert.IsTrue(this.timelineModel.LinkNextElementCalled);
        }

        /// <summary>
        /// Tests that the UnlinkNextElement method should be called when the command is executed 
        /// if the elements are linked.
        /// </summary>
        [TestMethod]
        public void ShouldCallToUnlinkNextElementIfTheElementsAreLinked()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var nextElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(40, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(currentElement);
            track.Shots.Add(nextElement);

            this.timelineModel.IsElementLinkedToReturnValue = true;

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position + currentElement.Duration)
                {
                    return nextElement;
                }
                else
                {
                    return null;
                }
            };

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.Out);

            Assert.IsFalse(this.timelineModel.UnlinkElementsCalled);

            command.Execute();

            Assert.IsTrue(this.timelineModel.UnlinkElementsCalled);
        }

        /// <summary>
        /// Tests that the UnlinkPreviousElement method should be called when the command is unexecuted. 
        /// </summary>
        [TestMethod]
        public void ShouldCallToUnlinkPreviousElementIfTheElementsAreLinkedWhenUnExecuteCommand()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var previousElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(previousElement);
            track.Shots.Add(currentElement);

            this.timelineModel.IsElementLinkedToReturnValue = false;

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

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.In);

            command.Execute();

            this.timelineModel.IsElementLinkedToReturnValue = true;

            Assert.IsFalse(this.timelineModel.UnlinkElementsCalled);

            command.UnExecute();

            Assert.IsTrue(this.timelineModel.UnlinkElementsCalled);
        }

        /// <summary>
        /// Tests that the LinkPreviousElement method should be called when the command is unexecuted. 
        /// </summary>
        [TestMethod]
        public void ShouldCallToLinkPreviousElementIfTheElementsAreUnlinkedWhenUnExecuteCommand()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var previousElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(previousElement);
            track.Shots.Add(currentElement);

            this.timelineModel.IsElementLinkedToReturnValue = true;

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

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.In);

            command.Execute();

            this.timelineModel.IsElementLinkedToReturnValue = false;

            Assert.IsFalse(this.timelineModel.LinkPreviousElementCalled);

            command.UnExecute();

            Assert.IsTrue(this.timelineModel.LinkPreviousElementCalled);
        }

        /// <summary>
        /// Tests that the UnlinkNextElement method should be called when the command is unexecuted. 
        /// </summary>
        [TestMethod]
        public void ShouldCallToUnlinkNextElementIfTheElementsAreLinkedWhenUnExecuteCommand()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var nextElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(40, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(currentElement);
            track.Shots.Add(nextElement);

            this.timelineModel.IsElementLinkedToReturnValue = false;

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position + currentElement.Duration)
                {
                    return nextElement;
                }
                else
                {
                    return null;
                }
            };

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.Out);

            command.Execute();

            this.timelineModel.IsElementLinkedToReturnValue = true;

            Assert.IsFalse(this.timelineModel.UnlinkElementsCalled);

            command.UnExecute();

            Assert.IsTrue(this.timelineModel.UnlinkElementsCalled);
        }

        /// <summary>
        /// Tests that the LinkNextElement method should be called when the command is unexecuted. 
        /// </summary>
        [TestMethod]
        public void ShouldCallToLinkNextElementIfTheElementsAreUnlinkedWhenUnExecuteCommand()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var currentElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            var nextElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(40, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(currentElement);
            track.Shots.Add(nextElement);

            this.timelineModel.IsElementLinkedToReturnValue = true;

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position + currentElement.Duration)
                {
                    return nextElement;
                }
                else
                {
                    return null;
                }
            };

            ToggleLinkElementCommand command = new ToggleLinkElementCommand(this.timelineModel, track, currentElement, LinkPosition.Out);

            command.Execute();

            this.timelineModel.IsElementLinkedToReturnValue = false;

            Assert.IsFalse(this.timelineModel.LinkNextElementCalled);

            command.UnExecute();

            Assert.IsTrue(this.timelineModel.LinkNextElementCalled);
        }
    }
}
