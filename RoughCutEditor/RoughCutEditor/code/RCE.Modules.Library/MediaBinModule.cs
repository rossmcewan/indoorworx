﻿// <copyright file="MediaBinModule.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MediaBinModule.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.MediaBin
{
    using System;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using RCE.Infrastructure;

    /// <summary>
    /// Class to load the MediaBin Module.
    /// </summary>
    public class MediaBinModule : IModule
    {
        /// <summary>
        /// The <see cref="IUnityContainer"/> to register the views and models.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The <see cref="IRegionManager"/> to inser the mediabin view.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaBinModule"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="regionManager">The region manager.</param>
        [CLSCompliant(false)]
        public MediaBinModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// Initializes the MediaBinModule instance.
        /// </summary>
        public void Initialize()
        {
            this.RegisterViewsAndServices();
            IMediaBinViewPresentationModel presentationModel = this.container.Resolve<IMediaBinViewPresentationModel>();
            this.regionManager.RegisterViewWithRegionInIndex(RegionNames.ToolsRegion, presentationModel.View, 1);
        }

        /// <summary>
        /// Registers all the views and services.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IMediaBinView, MediaBinView>();
            this.container.RegisterType<IMediaBinViewPresentationModel, MediaBinViewPresentationModel>();
        }
    }
}
