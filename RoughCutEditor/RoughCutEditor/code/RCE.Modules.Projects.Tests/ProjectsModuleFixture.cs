// <copyright file="ProjectsModuleFixture.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: ProjectsModuleFixture.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Projects.Tests
{
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    /// <summary>
    /// A class for testing the <see cref="ProjectsModule"/>.
    /// </summary>
    [TestClass]
    public class ProjectsModuleFixture
    {
        /// <summary>
        /// Tests that the views are being registered in the container.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterViews()
        {
            var container = new MockUnityContainer();
            var module = new TestableProjectsModule(container);

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(2, container.Types.Count);
            Assert.AreEqual(typeof(ProjectView), container.Types[typeof(IProjectView)]);
            Assert.AreEqual(typeof(ProjectViewPresenter), container.Types[typeof(IProjectViewPresenter)]);
        }

        /// <summary>
        /// Tests that the <see cref="ProjectView"/> is being added to the <seealso cref="IRegion">Tools Region</seealso>.
        /// </summary>
        [TestMethod]
        public void ShouldAddProjectViewToToolsRegion()
        {
            var toolsRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();

            container.Bag.Add(typeof(IProjectViewPresenter), new MockProjectViewPresenter());

            regionManager.Regions.Add("ToolsRegion", toolsRegion);

            var module = new ProjectsModule(container, regionManager);

            Assert.AreEqual(0, toolsRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolsRegion.AddedViews.Count);
            Assert.IsInstanceOfType(toolsRegion.AddedViews[0], typeof(IProjectView));
        }

        /// <summary>
        /// Defines a testable <see cref="ProjectsModule"/>.
        /// </summary>
        internal class TestableProjectsModule : ProjectsModule
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableProjectsModule"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            public TestableProjectsModule(IUnityContainer container)
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