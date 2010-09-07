// <copyright file="PlayerModule.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: PlayerModule.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Player
{
    using System;
    using Infrastructure;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The <see cref="IModule"/> for player module. 
    /// It registers the player view and model in
    ///  the container and inserts the view in the player region.
    /// </summary>
    public class PlayerModule : IModule
    {
        /// <summary>
        /// The <see cref="IUnityContainer"/> to register the objects.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The <see cref="IRegionManager"/> to insert the view in the player region.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerModule"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="regionManager">The region manager.</param>
        [CLSCompliant(false)]
        public PlayerModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// Registers the objects that are used in the player module and 
        /// insert the player view in the player region.
        /// </summary>
        public void Initialize()
        {
            this.RegisterViewsAndServices();

            IPlayerViewPresenter presenter = this.container.Resolve<IPlayerViewPresenter>();

            this.regionManager.Regions[RegionNames.PlayerRegion].Add(presenter.View);
        }

        /// <summary>
        /// Registers the views and services.
        /// </summary>
        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IAggregateMediaModel, AggregateMediaModel>();
            this.container.RegisterType<IPlayerView, PlayerView>();
            this.container.RegisterType<IPlayerViewPresenter, PlayerViewPresenter>();
        }
    }
}
