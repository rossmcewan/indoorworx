// <copyright file="MetadataModule.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MetadataModule.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Metadata
{
    using System;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using RCE.Infrastructure;
    using RCE.Modules.Metadata;

   /// <summary>
    /// Provides an implementation of the <seealso cref="IModule"/>. Defines the metadata module.
    /// </summary>
    public class MetadataModule : IModule
    {
        /// <summary>
        /// Private global variable to hold the reference to <see cref="IUnityContainer"/>
        /// passed in the MetadataModule constructor.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// Private global variable to hold the reference to <see cref="IRegionManager"/>
        /// passed in the MetadataModule constructor.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataModule"/> class.
        /// </summary>
        /// <param name="container">The <seealso cref="IUnityContainer"/> container used to resolve dependencies.</param>
        /// <param name="regionManager">The <seealso cref="IRegionManager"/> used to get the <seealso cref="IRegion"/>s.</param>
        [CLSCompliant(false)]
        public MetadataModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// Initializes the Metadata module.
        /// </summary>
        public void Initialize()
        {
            this.RegisterViewsAndServices();
            IMetadataViewPresentationModel presentationModel = this.container.Resolve<IMetadataViewPresentationModel>();
            this.regionManager.Regions[RegionNames.MetadataRegion].Add(presentationModel.View);
        }

        /// <summary>
        /// Registers the views and presentation models used by the module.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IMetadataView, MetadataView>();
            this.container.RegisterType<IMetadataViewPresentationModel, MetadataViewPresentationModel>();
        }
    }
}
