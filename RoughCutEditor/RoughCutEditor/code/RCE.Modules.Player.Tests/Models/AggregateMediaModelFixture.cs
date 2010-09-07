// <copyright file="AggregateMediaModelFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: AggregateMediaModelFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Infrastructure.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Player.Models;
    using Services.Contracts;
    using SMPTETimecode;
    using TitleTemplate = RCE.Infrastructure.Models.TitleTemplate;

    /// <summary>
    /// Test class for <see cref="AggregateMediaModel"/>.
    /// </summary>
    [TestClass]
    public class AggregateMediaModelFixture
    {
        /// <summary>
        /// Tests if the blank element is added when AddBlank method is called.
        /// </summary>
        [TestMethod]
        public void ShouldAddBlankElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement();

            var blankData = aggregateMediaModel.AddBlank(timelineElement);

            Assert.IsNotNull(blankData);
            Assert.IsInstanceOfType(blankData, typeof(BlankMediaData));
            Assert.AreEqual(1, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Should add <see cref="VideoAsset"/> on AddElement call for <see cref="VideoAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldAddVideoAssetElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mediaData = aggregateMediaModel.AddElement(timelineElement);

            Assert.IsNotNull(mediaData);
            Assert.IsInstanceOfType(mediaData, typeof(PlayableMediaData));
            Assert.AreEqual(1, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Should add <see cref="ImageAsset"/> on AddElement call for <see cref="ImageAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldAddImageAssetElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new ImageAsset() };

            var mediaData = aggregateMediaModel.AddElement(timelineElement);

            Assert.IsNotNull(mediaData);
            Assert.IsInstanceOfType(mediaData, typeof(ImageMediaData));
            Assert.AreEqual(1, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Should add <see cref="AudioAsset"/> on AddElement call for <see cref="AudioAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldAddAudioAssetElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new AudioAsset() };

            var mediaData = aggregateMediaModel.AddElement(timelineElement);

            Assert.IsNotNull(mediaData);
            Assert.IsInstanceOfType(mediaData, typeof(PlayableMediaData));
            Assert.AreEqual(1, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Should add <see cref="TitleAsset"/> on AddElement call for <see cref="TitleAsset"/>.
        /// </summary>
        [TestMethod]
        public void ShouldAddTitleAssetElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement
                                      {
                                          Asset = new TitleAsset
                                                      {
                                                          TitleTemplate = new TitleTemplate
                                                              {
                                                                  XamlResource = Resources.Resources.TitleTemplateXAMLResource
                                                              }
                                                      }
                                      };

            var mediaData = aggregateMediaModel.AddElement(timelineElement);

            Assert.IsNotNull(mediaData);
            Assert.IsInstanceOfType(mediaData, typeof(TitleMediaData));
            Assert.AreEqual(1, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Tests that the element is remvoved from the <see cref="AggregateMediaModel"/>
        /// when RemoveElement is called.
        /// </summary>
        [TestMethod]
        public void ShouldRemoveElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mockMediaData = new MockMediaData { MockedTimelineElement = timelineElement };

            aggregateMediaModel.BaseMediaData.Add(mockMediaData);

            var mediaData = aggregateMediaModel.RemoveElement(timelineElement);

            Assert.IsNotNull(mediaData);
            Assert.AreEqual(mockMediaData, mediaData);
            Assert.AreEqual(0, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Tests that the blank element is remvoved from the <see cref="AggregateMediaModel"/>
        /// when RemoveBlankElement is called.
        /// </summary>
        [TestMethod]
        public void ShouldRemoveBlankElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var blankMediaData = new BlankMediaData(timelineElement);

            aggregateMediaModel.BaseMediaData.Add(blankMediaData);

            var mediaData = aggregateMediaModel.RemoveBlankElement(timelineElement);

            Assert.IsNotNull(mediaData);
            Assert.AreEqual(blankMediaData, mediaData);
            Assert.AreEqual(0, aggregateMediaModel.BaseMediaData.Count);
        }

        /// <summary>
        /// Tests if the Play and Show method of the <see cref="MediaData"/>
        /// is called when play of <see cref="AggregateMediaModel"/> is called.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPlayAndShowOnMediaWhenPlay()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mockMediaData = new MockMediaData
            {
                MockedTimelineElement = timelineElement,
                In = TimeSpan.FromSeconds(0),
                Out = TimeSpan.FromSeconds(300)
            };

            aggregateMediaModel.BaseMediaData.Add(mockMediaData);

            Assert.IsFalse(mockMediaData.ShowCalled);

            aggregateMediaModel.Position = TimeSpan.FromSeconds(150);

            Assert.IsTrue(mockMediaData.ShowCalled);

            Assert.IsFalse(mockMediaData.PlayCalled);
            Assert.IsFalse(aggregateMediaModel.IsPlaying);

            aggregateMediaModel.Play();

            Assert.IsTrue(mockMediaData.PlayCalled);
            Assert.IsTrue(aggregateMediaModel.IsPlaying);
        }

        /// <summary>
        /// Tests if the Play and Show method of the <see cref="MediaData"/>
        /// is not called when play of <see cref="AggregateMediaModel"/> is called.
        /// </summary>
        [TestMethod]
        public void ShouldNotCallToPlayAndShowOnMediaWhenIsAlreadyPlaying()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mockMediaData = new MockMediaData
            {
                MockedTimelineElement = timelineElement,
                In = TimeSpan.FromSeconds(0),
                Out = TimeSpan.FromSeconds(300)
            };

            aggregateMediaModel.BaseMediaData.Add(mockMediaData);

            Assert.IsFalse(mockMediaData.ShowCalled);

            aggregateMediaModel.Position = TimeSpan.FromSeconds(150);

            Assert.IsTrue(mockMediaData.ShowCalled);

            aggregateMediaModel.Play();

            Assert.IsTrue(mockMediaData.PlayCalled);

            mockMediaData.PlayCalled = false;
            mockMediaData.ShowCalled = false;

            aggregateMediaModel.Play();

            Assert.IsFalse(mockMediaData.PlayCalled);
            Assert.IsFalse(mockMediaData.ShowCalled);
        }

        /// <summary>
        /// Tests if the pause method of the <see cref="MediaData"/>
        /// is called when pause of <see cref="AggregateMediaModel"/> is called.
        /// </summary>
        [TestMethod]
        public void ShouldCallToPauseOnMediaWhenPause()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mockMediaData = new MockMediaData 
            { 
                MockedTimelineElement = timelineElement, 
                In = TimeSpan.FromSeconds(0), 
                Out = TimeSpan.FromSeconds(300)
            };

            aggregateMediaModel.BaseMediaData.Add(mockMediaData);
            aggregateMediaModel.Position = TimeSpan.FromSeconds(150);

            Assert.IsFalse(mockMediaData.PauseCalled);

            aggregateMediaModel.Pause();

            Assert.IsTrue(mockMediaData.PauseCalled);
        }

        /// <summary>
        /// Tests if the duration is calculated properly.
        /// </summary>
        [TestMethod]
        public void ShouldGetTheDurationBasedOnAllTheMediaData()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new ImageAsset(), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte30), InPosition = TimeCode.FromMinutes(10, SmpteFrameRate.Smpte30), OutPosition = TimeCode.FromMinutes(30, SmpteFrameRate.Smpte30) };

            aggregateMediaModel.AddElement(timelineElement);

            Assert.AreEqual(timelineElement.Duration.TotalSeconds, aggregateMediaModel.Duration.TotalSeconds);
        }

        /// <summary>
        /// Tests if the duration is 0 when there is no media in the <see cref="AggregateMediaModel"/>.
        /// </summary>
        [TestMethod]
        public void ShouldGetZeroDurationWhenMediaIsEmpty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            
            Assert.AreEqual(0, aggregateMediaModel.Duration.TotalSeconds);
        }

        /// <summary>
        /// Should not throw exception if there is no media while setting mute property.
        /// </summary>
        [TestMethod]
        public void ShouldNotThrowExceptionIfThereIsNoMediaWhileSettingMuteProperty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            aggregateMediaModel.Mute = true;
            
            aggregateMediaModel.Mute = false;
        }

        /// <summary>
        /// Should mute the current media data if current media is valid while setting mute property to true.
        /// </summary>
        [TestMethod]
        public void ShouldMuteTheCurrentMediaDataIfCurrentMediaIsValidWhileSettingMutePropertyToTrue()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset(), InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25) };

            aggregateMediaModel.AddElement(timelineElement);

            // Set the current media.
            aggregateMediaModel.Position = TimeSpan.FromSeconds(1000);
            
            Assert.IsFalse(aggregateMediaModel.BaseMediaData[0].IsMuted);

            aggregateMediaModel.Mute = true;

            Assert.IsTrue(aggregateMediaModel.BaseMediaData[0].IsMuted);
        }

        /// <summary>
        /// Should unmute the current media data if current media is valid while setting mute property to false.
        /// </summary>
        [TestMethod]
        public void ShouldUnMuteTheCurrentMediaDataIfCurrentMediaIsValidWhileSettingMutePropertyToFalse()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset(), InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25) };

            aggregateMediaModel.AddElement(timelineElement);

            aggregateMediaModel.BaseMediaData[0].IsMuted = true;

            aggregateMediaModel.Position = TimeSpan.FromSeconds(300);

            aggregateMediaModel.Mute = false;

            Assert.IsFalse(aggregateMediaModel.BaseMediaData[0].IsMuted);
        }

        /// <summary>
        /// Should not throw exception if there is no media at current position while setting mute property.
        /// </summary>
        [TestMethod]
        public void ShouldNotThrowExceptionIfThereIsNoMediaAtCurrentPositionWhileSettingMuteProperty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement
                                      {
                                          Asset = new VideoAsset(),
                                          InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25),
                                          OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25),
                                          Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25)
                                      };

            aggregateMediaModel.AddElement(timelineElement);
            aggregateMediaModel.Position = TimeSpan.FromSeconds(10000);

            aggregateMediaModel.Mute = true;
            aggregateMediaModel.Mute = false;
        }

        /// <summary>
        /// Should hide the current media data if current media is valid while setting is visible property to false.
        /// </summary>
        [TestMethod]
        public void ShouldHideTheCurrentMediaDataIfCurrentMediaIsValidWhileSettingIsVisiblePropertyToFalse()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset(), InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25) };

            aggregateMediaModel.AddElement(timelineElement);

            // Set the current media.
            aggregateMediaModel.Position = TimeSpan.FromSeconds(1000);
            aggregateMediaModel.BaseMediaData[0].Show();

            Assert.IsTrue(((UIElement)aggregateMediaModel.BaseMediaData[0].Media).Opacity == 1.0);

            aggregateMediaModel.IsVisible = false;

            Assert.IsTrue(((UIElement)aggregateMediaModel.BaseMediaData[0].Media).Opacity == 0.0);
        }

        /// <summary>
        /// Should show the current media data if current media is valid while setting is visible property to true.
        /// </summary>
        [TestMethod]
        public void ShouldShowTheCurrentMediaDataIfCurrentMediaIsValidWhileSettingIsVisiblePropertyToTrue()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement { Asset = new VideoAsset(), InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25) };

            aggregateMediaModel.AddElement(timelineElement);

            // Set the current media.
            aggregateMediaModel.Position = TimeSpan.FromSeconds(1000);
            aggregateMediaModel.BaseMediaData[0].Hide();

            Assert.IsTrue(((UIElement)aggregateMediaModel.BaseMediaData[0].Media).Opacity == 0.0);

            aggregateMediaModel.IsVisible = true;

            Assert.IsTrue(((UIElement)aggregateMediaModel.BaseMediaData[0].Media).Opacity == 1.0);
        }

        /// <summary>
        /// Should not throw exception if there is no media while setting is visible property.
        /// </summary>
        [TestMethod]
        public void ShouldNotThrowExceptionIfThereIsNoMediaWhileSettingIsVisibleProperty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            
            aggregateMediaModel.IsVisible = true;

            aggregateMediaModel.IsVisible = false;
        }

        /// <summary>
        /// Should not throw exception if there is no media at current position while setting is visible property.
        /// </summary>
        [TestMethod]
        public void ShouldNotThrowExceptionIfThereIsNoMediaAtCurrentPositionWhileSettingIsVisibleProperty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            
            var timelineElement = new TimelineElement { Asset = new VideoAsset(), InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25) };
            
            aggregateMediaModel.AddElement(timelineElement);

            aggregateMediaModel.Position = TimeSpan.FromSeconds(10000);
            
            aggregateMediaModel.IsVisible = true;
            
            aggregateMediaModel.IsVisible = false;
        }

        /// <summary>
        /// Should position as zero when media is empty.
        /// </summary>
        [TestMethod]
        public void ShouldGetZeroPositionWhenMediaIsEmpty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            Assert.AreEqual(0, aggregateMediaModel.Position.TotalSeconds);
        }

        /// <summary>
        /// Shoulds get null asset when media is empty.
        /// </summary>
        [TestMethod]
        public void ShouldGetNullAssetWhenMediaIsEmpty()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            Assert.IsNull(aggregateMediaModel.CurrentAsset);
        }

        /// <summary>
        /// Should return the asset associated with the media data at the current position.
        /// </summary>
        [TestMethod]
        public void ShouldReturnTheAssetAssociatedWithTheMediaDataUnderTheCurrentPosition()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var expectedAsset = new VideoAsset { Source = new Uri("http://test") };

            var timelineElement1 = new TimelineElement { Asset = new ImageAsset(), InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25) };
            var timelineElement2 = new TimelineElement { Asset = expectedAsset, InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte25), OutPosition = TimeCode.FromSeconds(5000d, SmpteFrameRate.Smpte25), Position = TimeCode.FromSeconds(1000d, SmpteFrameRate.Smpte25) };

            aggregateMediaModel.AddElement(timelineElement1);
            aggregateMediaModel.AddElement(timelineElement2);

            // Set the current media.
            aggregateMediaModel.Position = TimeSpan.FromSeconds(6000);

            Assert.AreEqual(expectedAsset, aggregateMediaModel.CurrentAsset);
        }

        /// <summary>
        /// Tests if the aggregate media model can find <see cref="MediaData"/> by <see cref="TimelineElement"/>.
        /// </summary>
        [TestMethod]
        public void ShouldFindMediaDataByTimelineElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mediaData = aggregateMediaModel.AddElement(timelineElement);

            var mediaDataFound = aggregateMediaModel.FindMediaByElement(timelineElement);

            Assert.IsNotNull(mediaDataFound);
            Assert.AreEqual(mediaData, mediaDataFound);
        }

        /// <summary>
        /// Tests if a null is returned when looking for a media data of an element not added to the aggregate media model.
        /// </summary>
        [TestMethod]
        public void ShouldReturnNullIfAggregateMediaModelDoesNotContainsAMediaDataForATimelineElement()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();
            var timelineElement = new TimelineElement { Asset = new VideoAsset() };

            var mediaDataFound = aggregateMediaModel.FindMediaByElement(timelineElement);

            Assert.IsNull(mediaDataFound);
        }

        [TestMethod]
        public void ShouldUpdateMediaDataPosition1()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement = new TimelineElement
                                      {
                                          Asset = new VideoAsset(),
                                          Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                                          InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                                          OutPosition = TimeCode.FromSeconds(600d, SmpteFrameRate.Smpte2398),
                                      };

            var mockMediaData = new MockMediaData { MockedTimelineElement = timelineElement };
            mockMediaData.Position = TimeSpan.FromSeconds(0);
            mockMediaData.In = TimeSpan.FromSeconds(timelineElement.InPosition.TotalSeconds);
            mockMediaData.Out = TimeSpan.FromSeconds(timelineElement.OutPosition.TotalSeconds);

            aggregateMediaModel.BaseMediaData.Add(mockMediaData);

            aggregateMediaModel.Position = TimeSpan.FromSeconds(300);

            Assert.AreEqual(aggregateMediaModel.Position, mockMediaData.Position);
        }

        [TestMethod]
        public void ShouldUpdateMediaDataPosition2()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement1 = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                OutPosition = TimeCode.FromSeconds(100d, SmpteFrameRate.Smpte2398),
            };

            var mockMediaData1 = new MockMediaData
                                     {
                                         MockedTimelineElement = timelineElement1,
                                         Position = TimeSpan.FromSeconds(0),
                                         In = TimeSpan.FromSeconds(timelineElement1.InPosition.TotalSeconds),
                                         Out = TimeSpan.FromSeconds(timelineElement1.OutPosition.TotalSeconds)
                                     };

            var timelineElement2 = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromSeconds(100d, SmpteFrameRate.Smpte2398),
                InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                OutPosition = TimeCode.FromSeconds(600d, SmpteFrameRate.Smpte2398),
            };

            var mockMediaData2 = new MockMediaData
                                     {
                                         MockedTimelineElement = timelineElement2,
                                         Position = TimeSpan.FromSeconds(0),
                                         In = TimeSpan.FromSeconds(timelineElement2.InPosition.TotalSeconds),
                                         Out = TimeSpan.FromSeconds(timelineElement2.OutPosition.TotalSeconds)
                                     };

            aggregateMediaModel.BaseMediaData.Add(mockMediaData1);
            aggregateMediaModel.BaseMediaData.Add(mockMediaData2);

            aggregateMediaModel.Position = TimeSpan.FromSeconds(300);

            Assert.AreEqual(new TimeSpan(0, 3, 20), mockMediaData2.Position);
        }

        [TestMethod]
        public void ShouldUpdateMediaDataPosition3()
        {
            var aggregateMediaModel = CreateTestableAggregateMediaModel();

            var timelineElement1 = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                InPosition = TimeCode.FromSeconds(0d, SmpteFrameRate.Smpte2398),
                OutPosition = TimeCode.FromSeconds(150d, SmpteFrameRate.Smpte2398),
            };

            var mockMediaData1 = new MockMediaData
            {
                MockedTimelineElement = timelineElement1,
                Position = TimeSpan.FromSeconds(0),
                In = TimeSpan.FromSeconds(timelineElement1.InPosition.TotalSeconds),
                Out = TimeSpan.FromSeconds(timelineElement1.OutPosition.TotalSeconds)
            };

            var timelineElement2 = new TimelineElement
            {
                Asset = new VideoAsset(),
                Position = TimeCode.FromSeconds(150d, SmpteFrameRate.Smpte2398),
                InPosition = TimeCode.FromSeconds(50d, SmpteFrameRate.Smpte2398),
                OutPosition = TimeCode.FromSeconds(600d, SmpteFrameRate.Smpte2398),
            };

            var mockMediaData2 = new MockMediaData
            {
                MockedTimelineElement = timelineElement2,
                Position = TimeSpan.FromSeconds(0),
                In = TimeSpan.FromSeconds(timelineElement2.InPosition.TotalSeconds),
                Out = TimeSpan.FromSeconds(timelineElement2.OutPosition.TotalSeconds)
            };

            aggregateMediaModel.BaseMediaData.Add(mockMediaData1);
            aggregateMediaModel.BaseMediaData.Add(mockMediaData2);

            aggregateMediaModel.Position = TimeSpan.FromSeconds(300);

            Assert.AreEqual(new TimeSpan(0, 3, 20), mockMediaData2.Position);
        }

        /// <summary>
        /// Creates the testable aggregate media model.
        /// </summary>
        /// <returns>The <see cref="TestableAggregateMediaModel"/>.</returns>
        private static TestableAggregateMediaModel CreateTestableAggregateMediaModel()
        {
            return new TestableAggregateMediaModel();
        }

        /// <summary>
        /// Testable class for <see cref="AggregateMediaModel"/>.
        /// </summary>
        private class TestableAggregateMediaModel : AggregateMediaModel
        {
            /// <summary>
            /// Gets the base media data.
            /// </summary>
            /// <value>The base media data.</value>
            public IList<MediaData> BaseMediaData
            {
                get { return this.MediaData; }
            }
        }
    }
}
