﻿// <copyright file="LibraryModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: LibraryModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Library.Tests
{
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// Test class for <see cref="LibraryModule"/>.
    /// </summary>
    [TestClass]
    public class LibraryModuleFixture
    {
        /// <summary>
        /// Should register views in the unity container.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableLibraryModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(LibraryView), container.Types[typeof(ILibraryView)]);
            Assert.AreEqual(typeof(LibraryViewPresentationModel), container.Types[typeof(ILibraryViewPresentationModel)]);
        }

        /// <summary>
        /// Should add <see cref="LibraryView"/> to tools region.
        /// </summary>
        [TestMethod]
        public void ShouldAddLibraryViewToToolsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(ILibraryViewPresentationModel), new MockLibraryViewPresentationModel());

            regionManager.Regions.Add("ToolsRegion", toolsRegion);

            var module = new LibraryModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(ILibraryView));
        }

        /// <summary>
        /// It is used to test <see cref="LibraryModule"/> class.
        /// </summary>
        internal class TestableLibraryModule : LibraryModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableLibraryModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableLibraryModule(IUnityContainer container)
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