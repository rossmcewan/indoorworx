// <copyright file="TitlesModule.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: TitlesModule.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Titles
{
    using System;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using RCE.Infrastructure;

    /// <summary>
    /// Class to load the Titles Module.
    /// </summary>
    public class TitlesModule : IModule
    {
        /// <summary>
        /// The <seealso cref="IUnityContainer"/> container used to resolve dependencies.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The <seealso cref="IRegionManager"/> used to get the <seealso cref="IRegion"/>s.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitlesModule"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        [CLSCompliant(false)]
        public TitlesModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// Initializes the TitlesModule instance.
        /// </summary>
        public void Initialize()
        {
            this.RegisterViewsAndServices();
            ITitlesViewPresentationModel presentationModel = this.container.Resolve<ITitlesViewPresentationModel>();

            this.regionManager.RegisterViewWithRegionInIndex(RegionNames.ToolsRegion, presentationModel.View, 3);
        }

        /// <summary>
        /// Registers all the views and services.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<ITitlesView, TitlesView>();
            this.container.RegisterType<ITitlesViewPresentationModel, TitlesViewPresentationModel>();
        }
    }
}
