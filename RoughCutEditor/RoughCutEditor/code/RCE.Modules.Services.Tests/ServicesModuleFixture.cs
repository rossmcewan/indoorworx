// <copyright file="ServicesModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TitlesModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Services.Tests
{
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Modules.Services.Views;

    /// <summary>
    /// Test class for <see cref="ServicesModule"/>.
    /// </summary>
    [TestClass]
    public class ServicesModuleFixture
    {
        /// <summary>
        /// Should register the views and models.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableServicesModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(10, container.Types.Count);
            Assert.AreEqual(typeof(NotificationView), container.Types[typeof(INotificationView)]);
            Assert.AreEqual(typeof(NotificationViewPresenter), container.Types[typeof(INotificationViewPresenter)]);
            Assert.AreEqual(typeof(DataServiceFacade), container.Types[typeof(IDataServiceFacade)]);
            Assert.AreEqual(typeof(AssetsDataServiceFacade), container.Types[typeof(IAssetsDataServiceFacade)]);
            Assert.AreEqual(typeof(NoOpEventDataParser), container.Types[typeof(IEventDataParser<EventData>)]);
            Assert.AreEqual(typeof(NoOpEventOffsetParser), container.Types[typeof(IEventDataParser<EventOffset>)]);
            Assert.AreEqual(typeof(ProjectService), container.Types[typeof(IProjectService)]);
            Assert.AreEqual(typeof(TimelineBarRegistry), container.Types[typeof(ITimelineBarRegistry)]);
            Assert.AreEqual(typeof(WVC1CodecPrivateDataParser), container.Types[typeof(ICodecPrivateDataParser)]);
        }

        /// <summary>
        /// Should add <see cref="NotificationView"/> to tools region.
        /// </summary>
        [TestMethod]
        public void ShouldAddNotificationViewToNotificationsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(INotificationViewPresenter), new MockNotificationViewPresenter());
            container.Bag.Add(typeof(IProjectService), new MockProjectService());
            toolsRegion.Name = RegionNames.NotificationsRegion;
            regionManager.Regions.Add(toolsRegion);

            var module = new ServicesModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(INotificationView));
        }

        /// <summary>
        /// Testable class for <see cref="ServicesModule"/>.
        /// </summary>
        private class TestableServicesModule : ServicesModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableServicesModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableServicesModule(IUnityContainer container)
                : base(container, new MockRegionManager())
            {
            }

            /// <summary>
            /// Invokes the register views and services.
            /// </summary>
            public void InvokeRegisterViewsAndServices()
            {
                this.RegisterViewsAndServices();
            }
        }
    }
}