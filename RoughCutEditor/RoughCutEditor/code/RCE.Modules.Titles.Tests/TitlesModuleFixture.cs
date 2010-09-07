// <copyright file="TitlesModuleFixture.cs" company="Microsoft Corporation">
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

namespace RCE.Modules.Titles.Tests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCE.Modules.Titles.Tests.Mocks;

    /// <summary>
    /// Test class for <see cref="TitlesModule"/>.
    /// </summary>
    [TestClass]
    public class TitlesModuleFixture
    {
        /// <summary>
        /// Should register the views and models.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableTitlesModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(TitlesView), container.Types[typeof(ITitlesView)]);
            Assert.AreEqual(typeof(TitlesViewPresentationModel), container.Types[typeof(ITitlesViewPresentationModel)]);
        }

        /// <summary>
        /// Should add <see cref="TitlesView"/> to tools region.
        /// </summary>
        [TestMethod]
        public void ShouldAddTitlesViewToToolsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(ITitlesViewPresentationModel), new MockTitlesViewPresentationModel());
            toolsRegion.Name = "ToolsRegion";
            regionManager.Regions.Add(toolsRegion);

            var module = new TitlesModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(ITitlesView));
        }

        /// <summary>
        /// Testable class for <see cref="TitlesModule"/>.
        /// </summary>
        private class TestableTitlesModule : TitlesModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableTitlesModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableTitlesModule(IUnityContainer container)
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