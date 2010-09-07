// <copyright file="PlayerModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayerModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player.Tests
{
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// Test class for <see cref="PlayerModule"/>.
    /// </summary>
    [TestClass]
    public class PlayerModuleFixture
    {
        /// <summary>
        /// Tests if the views and models are registerd.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestablePlayerModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(3, container.Types.Count);
            Assert.AreEqual(typeof(AggregateMediaModel), container.Types[typeof(IAggregateMediaModel)]);
            Assert.AreEqual(typeof(PlayerView), container.Types[typeof(IPlayerView)]);
            Assert.AreEqual(typeof(PlayerViewPresenter), container.Types[typeof(IPlayerViewPresenter)]);
        }

        /// <summary>
        /// Tests if the <see cref="PlayerView"/> is inserted in the player region.
        /// </summary>
        [TestMethod]
        public void ShouldAddPlayerViewToPlayerRegion()
        {
            var playerRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(IPlayerViewPresenter), new MockPlayerViewPresenter());

            regionManager.Regions.Add("PlayerRegion", playerRegion);

            var module = new PlayerModule(container, regionManager);

            Assert.AreEqual(0, playerRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, playerRegion.AddedViews.Count);
            Assert.IsInstanceOfType(playerRegion.AddedViews[0], typeof(IPlayerView));
        }

        /// <summary>
        /// Testable class for <see cref="PlayerModule"/>.
        /// </summary>
        internal class TestablePlayerModule : PlayerModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestablePlayerModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestablePlayerModule(IUnityContainer container)
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