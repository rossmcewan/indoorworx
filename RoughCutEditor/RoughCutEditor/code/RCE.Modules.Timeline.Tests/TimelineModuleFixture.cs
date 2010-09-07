// <copyright file="TimelineModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TimelineModuleFixture.cs                     
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
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Timeline.Commands;

    /// <summary>
    /// A class for testing the <see cref="TimelineModule"/>.
    /// </summary>
    [TestClass]
    public class TimelineModuleFixture
    {
        /// <summary>
        /// Tests that the views should be registered in the container.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableTimelineModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(3, container.Types.Count);
            Assert.AreEqual(typeof(TimelineView), container.Types[typeof(ITimelineView)]);
            Assert.AreEqual(typeof(TimelinePresenter), container.Types[typeof(ITimelinePresenter)]);
            Assert.AreEqual(typeof(Caretaker), container.Types[typeof(ICaretaker)]);
        }

        /// <summary>
        /// Tests that the TimelineView should be added to the Timeline region.
        /// </summary>
        [TestMethod]
        public void ShouldAddTimelineViewToTimelineRegion()
        {
            var timelineRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(ITimelinePresenter), new MockTimelinePresenter());

            regionManager.Regions.Add("TimelineRegion", timelineRegion);

            var module = new TimelineModule(container, regionManager);

            Assert.AreEqual(0, timelineRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, timelineRegion.AddedViews.Count);
            Assert.IsInstanceOfType(timelineRegion.AddedViews[0], typeof(ITimelineView));
        }

        /// <summary>
        /// Testable Timeline Module.
        /// </summary>
        internal class TestableTimelineModule : TimelineModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableTimelineModule"/> class.
            /// </summary>
            /// <param name="container">The container used to register views an services.</param>
            public TestableTimelineModule(IUnityContainer container)
                : base(container, new MockRegionManager())
            {
            }

            /// <summary>
            /// Invokes the RegisterViewAndServices method.
            /// </summary>
            public void InvokeRegisterViewsAndServices()
            {
                this.RegisterViewsAndServices();
            }
        }
    }
}