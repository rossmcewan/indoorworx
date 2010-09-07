// <copyright file="MediaBinModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MediaBinModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.MediaBin.Tests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// Test class for <see cref="MediaBinModule"/>.
    /// </summary>
    [TestClass]
    public class MediaBinModuleFixture
    {
        /// <summary>
        /// Should register views in the unity container.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableMediaBinModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(MediaBinView), container.Types[typeof(IMediaBinView)]);
            Assert.AreEqual(typeof(MediaBinViewPresentationModel), container.Types[typeof(IMediaBinViewPresentationModel)]);
        }

        /// <summary>
        /// Should add Media bin view to tools region.
        /// </summary>
        [TestMethod]
        public void ShouldAddMediaBinViewToToolsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(IMediaBinViewPresentationModel), new MockMediaBinViewPresentationModel());
            toolsRegion.Name = "ToolsRegion";
            regionManager.Regions.Add(toolsRegion);

            var module = new MediaBinModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(IMediaBinView));
        }

        /// <summary>
        /// It is used to test <see cref="MediaBinModule"/> class.
        /// </summary>
        internal class TestableMediaBinModule : MediaBinModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableMediaBinModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableMediaBinModule(IUnityContainer container)
                : base(container, new MockRegionManager())
            {
            }

            /// <summary>
            /// Invokes the RegisterViewsAndServices method.
            /// </summary>
            public void InvokeRegisterViewsAndServices()
            {
                this.RegisterViewsAndServices();
            }
        }
    }
}
