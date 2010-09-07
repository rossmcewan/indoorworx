// <copyright file="ServicesModule.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: SettingsModule.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Services
{
    using System;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Composite.UnityExtensions;
    using Microsoft.Practices.Unity;
    using Services;
    using Views;

    /// <summary>
    /// Provides an implementation of the <seealso cref="IModule"/>. Defines the services module.
    /// </summary>
    public class ServicesModule : IModule
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
        /// Initializes a new instance of the <see cref="ServicesModule"/> class. 
        /// </summary>
        /// <param name="container">The <seealso cref="IUnityContainer"/> container used to resolve dependencies.</param>
        /// <param name="regionManager">The <seealso cref="IRegionManager"/> used to get the <seealso cref="IRegion"/>s.</param>
        [CLSCompliant(false)]
        public ServicesModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// Initializes the Settings module.
        /// </summary>
        public void Initialize()
        {
            this.RegisterViewsAndServices();

            this.container.Resolve<IProjectService>();

            INotificationViewPresenter presenter = this.container.Resolve<INotificationViewPresenter>();

            this.regionManager.Regions[RegionNames.NotificationsRegion].Add(presenter.View);
        }

        /// <summary>
        /// Registers the views and presentation models used by the module.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<INotificationView, NotificationView>();

            this.container.RegisterType<INotificationViewPresenter, NotificationViewPresenter>();

            this.RegisterTypeIfMissing<IDataServiceFacade, DataServiceFacade>();

            this.RegisterTypeIfMissing<IAssetsDataServiceFacade, AssetsDataServiceFacade>();

            this.RegisterTypeIfMissing<IEventDataParser<EventData>, NoOpEventDataParser>();

            this.RegisterTypeIfMissing<IEventDataParser<EventOffset>, NoOpEventOffsetParser>();

            this.RegisterTypeIfMissing<IThumbnailService, AssetsThumbnailService>(true);

            this.container.RegisterType<IProjectService, ProjectService>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<ITimelineBarRegistry, TimelineBarRegistry>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<ICodecPrivateDataParser, WVC1CodecPrivateDataParser>(new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Registers a type if is not already registered in the container.
        /// </summary>
        /// <typeparam name="TInterface">The interface.</typeparam>
        /// <typeparam name="TImplementation">The implemenentation.</typeparam>
        private void RegisterTypeIfMissing<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            this.RegisterTypeIfMissing<TInterface, TImplementation>(false);
        }

        /// <summary>
        /// Registers a type if is not already registered in the container.
        /// </summary>
        /// <typeparam name="TInterface">The interface.</typeparam>
        /// <typeparam name="TImplementation">The implemenentation.</typeparam>
        /// <param name="isSingleton">Defines if the type should be registered as singleton or not.</param>
        private void RegisterTypeIfMissing<TInterface, TImplementation>(bool isSingleton) 
            where TImplementation : TInterface
        {
            if (!this.container.IsTypeRegistered(typeof(TInterface)))
            {
                if (isSingleton)
                {
                    this.container.RegisterType<TInterface, TImplementation>(new ContainerControlledLifetimeManager());
                }
                else
                {
                    this.container.RegisterType<TInterface, TImplementation>();
                }
            }
        }
    }
}