// <copyright file="RemoveElementCommandFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: RemoveElementCommandFixture.cs                     
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
    using Services.Contracts;
    using SMPTETimecode;
    using Timeline.Commands;
    using Track = RCE.Infrastructure.Models.Track;
    using TrackType = RCE.Infrastructure.Models.TrackType;

    /// <summary>
    /// A class for testing the <see cref="RemoveElementCommand"/>.
    /// </summary>
    [TestClass]
    public class RemoveElementCommandFixture
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
            this.timelineModel.Duration = TimeCode.FromAbsoluteTime(10000, SmpteFrameRate.Smpte30);
        }

        /// <summary>
        /// Tests that RemoveElement method on TimelineModel should be called  when executing the command.
        /// </summary>
        [TestMethod]
        public void ShouldCallToRemoveElementOnTimelineModel()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var timelineElement = new TimelineElement();

            RemoveElementCommand command = new RemoveElementCommand(this.timelineModel, track, EditMode.Gap, timelineElement);

            Assert.IsFalse(this.timelineModel.RemoveElementCalled);

            command.Execute();

            Assert.IsTrue(this.timelineModel.RemoveElementCalled);

            Assert.AreEqual(timelineElement, this.timelineModel.RemoveElementArgument);
        }

        /// <summary>
        /// Tests that MoveElement method on TimelineModel should be called when executing the command using RipplMode as <see cref="EditMode"/>.
        /// </summary>
        [TestMethod]
        public void ShouldCallToMoveElementsOnTimelineModelInRippleModeWhenExecuteCommand()
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

            var nextElement = new TimelineElement
                                  {
                                      Position = TimeCode.FromAbsoluteTime(40, this.timelineModel.Duration.FrameRate),
                                      InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                                      OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
                                  };

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
                                                                     {
                                                                         if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position)
                                                                         {
                                                                             return previousElement;
                                                                         }
                                                                         else if (currentElement.Position + currentElement.Duration == this.timelineModel.GetElementAtPositionPositionArgument)
                                                                         {
                                                                             return nextElement;
                                                                         }
                                                                         else
                                                                         {
                                                                             return null;
                                                                         }
                                                                     };

            RemoveElementCommand command = new RemoveElementCommand(this.timelineModel, track, EditMode.Ripple, currentElement);

            Assert.IsFalse(this.timelineModel.MoveElementCalled);

            command.Execute();

            Assert.IsTrue(this.timelineModel.MoveElementCalled);

            Assert.AreEqual(nextElement, this.timelineModel.MoveElementElementArgument);
        }

        /// <summary>
        /// Tests that the AddElement method on TimelineModel should be called when unexecuting the command.
        /// </summary>
        [TestMethod]
        public void ShouldCallToAddElementOnTimelineModelWhenUnExecuteCommand()
        {
            var track = new Track { TrackType = TrackType.Visual };
            this.timelineModel.Tracks.Add(track);

            var timelineElement = new TimelineElement();

            RemoveElementCommand command = new RemoveElementCommand(this.timelineModel, track, EditMode.Gap, timelineElement);

            command.Execute();

            Assert.IsFalse(this.timelineModel.AddElementCalled);

            command.UnExecute();

            Assert.IsTrue(this.timelineModel.AddElementCalled);

            Assert.AreEqual(timelineElement, this.timelineModel.AddElementArgument);
        }

        /// <summary>
        /// Tests that the MoveElement method on TimelineModel should be called when unexecuting the command and the edit mode is Ripple.
        /// </summary>
        [TestMethod]
        public void ShouldCallToMoveElementsOnTimelineModelInRippleModeWhenUnExecuteCommand()
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

            var nextElement = new TimelineElement
            {
                Position = TimeCode.FromAbsoluteTime(40, this.timelineModel.Duration.FrameRate),
                InPosition = TimeCode.FromAbsoluteTime(0, this.timelineModel.Duration.FrameRate),
                OutPosition = TimeCode.FromAbsoluteTime(20, this.timelineModel.Duration.FrameRate)
            };

            track.Shots.Add(previousElement);
            track.Shots.Add(currentElement);
            track.Shots.Add(nextElement);

            this.timelineModel.GetElementAtPositionReturnFunction = () =>
            {
                if (this.timelineModel.GetElementAtPositionPositionArgument == currentElement.Position)
                {
                    return previousElement;
                }
                else if (currentElement.Position + currentElement.Duration == this.timelineModel.GetElementAtPositionPositionArgument)
                {
                    return nextElement;
                }
                else
                {
                    return null;
                }
            };

            RemoveElementCommand command = new RemoveElementCommand(this.timelineModel, track, EditMode.Ripple, currentElement);

            command.Execute();

            this.timelineModel.MoveElementCalled = false;
            this.timelineModel.MoveElementElementArgument = null;

            Assert.IsFalse(this.timelineModel.MoveElementCalled);

            command.UnExecute();

            Assert.IsTrue(this.timelineModel.MoveElementCalled);

            Assert.AreEqual(nextElement, this.timelineModel.MoveElementElementArgument);
        }
    }
}
