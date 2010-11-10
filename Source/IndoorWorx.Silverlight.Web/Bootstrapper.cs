using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using IndoorWorx.Infrastructure.ServiceModel;

namespace IndoorWorx.Silverlight.Web
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {
            return null;
        }

        protected override Microsoft.Practices.Unity.IUnityContainer CreateContainer()
        {
            var container = base.CreateContainer();
            IoC.UnityContainer = container;
            return container;
        }

        protected override Microsoft.Practices.Composite.Modularity.IModuleCatalog GetModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}