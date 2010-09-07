// <copyright file="MetadataModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MetadataModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Metadata.Tests
{
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// A class for testing the <see cref="MetadataModule"/>.
    /// </summary>
    [TestClass]
    public class MetadataModuleFixture
    {
        /// <summary>
        /// Tests that the views are being registered in the container.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableMetadataModuleModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(MetadataView), container.Types[typeof(IMetadataView)]);
            Assert.AreEqual(typeof(MetadataViewPresentationModel), container.Types[typeof(IMetadataViewPresentationModel)]);
        }

        /// <summary>
        /// Tests that the <see cref="MetadataView"/> is being added to the <seealso cref="IRegion">Metadata Region</seealso>.
        /// </summary>
        [TestMethod]
        public void ShouldAddMetadataViewToMetadataRegion()
        {
            var metadataRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(IMetadataViewPresentationModel), new MockMetadataViewPresentationModel());

            regionManager.Regions.Add("MetadataRegion", metadataRegion);

            var module = new MetadataModule(container, regionManager);

            Assert.AreEqual(0, metadataRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, metadataRegion.AddedViews.Count);
            Assert.IsInstanceOfType(metadataRegion.AddedViews[0], typeof(IMetadataView));
        }

        /// <summary>
        /// Defines a testable <see cref="MetadataModule"/>.
        /// </summary>
        internal class TestableMetadataModuleModule : MetadataModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableMetadataModuleModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableMetadataModuleModule(IUnityContainer container)
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