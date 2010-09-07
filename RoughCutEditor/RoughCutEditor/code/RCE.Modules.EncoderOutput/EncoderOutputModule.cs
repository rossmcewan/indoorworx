// <copyright file="EncoderOutputModule.cs" company="Microsoft Corporation">
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

namespace RCE.Modules.EncoderOutput
{
    using System;
    using Infrastructure;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Services;
    using Views;

    /// <summary>
    /// Class to load the EncoderOutput Module.
    /// </summary>
    public class EncoderOutputModule : IModule
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
        /// Initializes a new instance of the <see cref="EncoderOutputModule"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        [CLSCompliant(false)]
        public EncoderOutputModule(IUnityContainer container, IRegionManager regionManager)
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
            IEncoderSettingsPresentationModel presentationModel = this.container.Resolve<IEncoderSettingsPresentationModel>();

            this.regionManager.RegisterViewWithRegionInIndex(RegionNames.ToolsRegion, presentationModel.View, 3);
        }

        /// <summary>
        /// Registers all the views and services.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IOutputGeneratorService, OutputGeneratorService>();
            this.container.RegisterType<IEncoderSettingsView, EncoderSettingsView>();
            this.container.RegisterType<IEncoderSettingsPresentationModel, EncoderSettingsPresentationModel>();
        }
    }
}