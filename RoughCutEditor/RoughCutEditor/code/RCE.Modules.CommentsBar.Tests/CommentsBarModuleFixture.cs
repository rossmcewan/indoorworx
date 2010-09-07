// <copyright file="CommentsBarModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: CommentsBarModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.CommentsBar.Tests
{
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// Test class for <see cref="CommentsBarModule"/>.
    /// </summary>
    [TestClass]
    public class CommentsBarModuleFixture
    {
        /// <summary>
        /// Tests if the views and models are registered.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableCommentsBarModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(CommentsBarView), container.Types[typeof(ICommentsBarView)]);
            Assert.AreEqual(typeof(CommentsBarPresenter), container.Types[typeof(ICommentsBarPresenter)]);
        }

        /// <summary>
        /// Tests if the <see cref="CommentsBarView"/> is added into the CommentBarRegion.
        /// </summary>
        [TestMethod]
        public void ShouldAddPlayerViewToPlayerRegion()
        {
            var playerRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(ICommentsBarPresenter), new MockCommentsBarPresenter());

            regionManager.Regions.Add("CommentsBarRegion", playerRegion);

            var module = new CommentsBarModule(container, regionManager);

            Assert.AreEqual(0, playerRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, playerRegion.AddedViews.Count);
            Assert.IsInstanceOfType(playerRegion.AddedViews[0], typeof(ICommentsBarView));
        }

        /// <summary>
        /// Testable class for <see cref="CommentsBarModule"/>.
        /// </summary>
        internal class TestableCommentsBarModule : CommentsBarModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableCommentsBarModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableCommentsBarModule(IUnityContainer container)
                : base(container, new MockRegionManager())
            {
            }

            /// <summary>
            /// Invokes the RegisterViewsAndServices.
            /// </summary>
            public void InvokeRegisterViewsAndServices()
            {
                this.RegisterViewsAndServices();
            }
        }
    }
}