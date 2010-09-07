// <copyright file="AdsModule.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: AdsModule.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Ads
{
    using System;
    using Infrastructure;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Class to load the AdsModule Module.
    /// </summary>
    public class AdsModule : IModule
    {
        /// <summary>
        /// The <see cref="IUnityContainer"/> to register the views and services.
        /// </summary>
        private readonly IUnityContainer container;

         /// <summary>
        /// Initializes a new instance of the <see cref="AdsModule"/> class.
        /// </summary>
        /// <param name="container">The instance of <see cref="IUnityContainer"/> interface.</param>
        [CLSCompliant(false)]
        public AdsModule(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Initializes the CommentsBarModule instance.
        /// </summary>
        public void Initialize()
        {
            this.RegisterViewsAndServices();

            ITimelineBarRegistry registry = this.container.Resolve<ITimelineBarRegistry>();

            registry.RegisterTimelineBarElement("Ad", () => this.container.Resolve<IAdEditBoxPresentationModel>());
        }

        /// <summary>
        /// Registers all the views and services.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IAdViewPreview, AdView>();
            this.container.RegisterType<IAdEditBox, AdEditBox>();
            this.container.RegisterType<IAdEditBoxPresentationModel, AdEditBoxPresentationModel>();
        }
    }
}
