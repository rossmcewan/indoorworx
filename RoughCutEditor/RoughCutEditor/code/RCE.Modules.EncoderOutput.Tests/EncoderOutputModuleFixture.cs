// <copyright file="EncoderOutputModuleFixture.cs" company="Microsoft Corporation">
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

namespace RCE.Modules.EncoderOutput.Tests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCE.Modules.EncoderOutput.Services;
    using RCE.Modules.EncoderOutput.Tests.Mocks;
    using RCE.Modules.EncoderOutput.Views;

    /// <summary>
    /// Test class for <see cref="EncoderOutputModule"/>.
    /// </summary>
    [TestClass]
    public class EncoderOutputModuleFixture
    {
        /// <summary>
        /// Should register the views and models.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableEncoderOutputModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(3, container.Types.Count);
            Assert.AreEqual(typeof(OutputGeneratorService), container.Types[typeof(IOutputGeneratorService)]);
            Assert.AreEqual(typeof(EncoderSettingsView), container.Types[typeof(IEncoderSettingsView)]);
            Assert.AreEqual(typeof(EncoderSettingsPresentationModel), container.Types[typeof(IEncoderSettingsPresentationModel)]);
        }

        /// <summary>
        /// Should add <see cref="EncoderSettingsView"/> to tools region.
        /// </summary>
        [TestMethod]
        public void ShouldAddEncoderSettingsViewToToolsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(IEncoderSettingsPresentationModel), new MockEncoderSettingsPresentationModel());
            toolsRegion.Name = "ToolsRegion";
            regionManager.Regions.Add(toolsRegion);

            var module = new EncoderOutputModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(IEncoderSettingsView));
        }

        /// <summary>
        /// Testable class for <see cref="EncoderOutputModule"/>.
        /// </summary>
        private class TestableEncoderOutputModule : EncoderOutputModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableEncoderOutputModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableEncoderOutputModule(IUnityContainer container)
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