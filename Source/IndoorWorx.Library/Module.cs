using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Library.Services;
using IndoorWorx.Infrastructure.ServiceModel;
using Microsoft.Practices.ServiceLocation;

namespace IndoorWorx.Library
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
            IoC.Initialize(this.unityContainer);
        }


        #region IModule Members

        public void Initialize()
        {
            unityContainer.RegisterType<IApplicationUserService, ApplicationUserService>();
        }

        #endregion
    }
}
