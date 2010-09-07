// <copyright file="SettingsModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SettingsModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Settings.Tests
{
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// Test class for <see cref="SettingsModule"/>.
    /// </summary>
    [TestClass]
    public class SettingsModuleFixture
    {
        /// <summary>
        /// Should register the views.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableSettingsModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(SettingsView), container.Types[typeof(ISettingsView)]);
            Assert.AreEqual(typeof(SettingsViewPresentationModel), container.Types[typeof(ISettingsViewPresentationModel)]);
        }

        /// <summary>
        /// Should add library view to tools region.
        /// </summary>
        [TestMethod]
        public void ShouldAddLibraryViewToToolsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(ISettingsViewPresentationModel), new MockSettingsViewPresentationModel());

            regionManager.Regions.Add("ToolsRegion", toolsRegion);

            var module = new SettingsModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(ISettingsView));
        }

        /// <summary>
        /// Testable class for <see cref="SettingsModule"/>.
        /// </summary>
        internal class TestableSettingsModule : SettingsModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableSettingsModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableSettingsModule(IUnityContainer container)
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