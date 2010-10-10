using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using IndoorWorx.Infrastructure.Navigation;
using IndoorWorx.Library.Navigation;

namespace IndoorWorx.Library
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        public Module(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        #region IModule Members

        public void Initialize()
        {
            unityContainer.RegisterInstance<INavigationService>(unityContainer.Resolve<NavigationService>());
        }

        #endregion
    }
}
