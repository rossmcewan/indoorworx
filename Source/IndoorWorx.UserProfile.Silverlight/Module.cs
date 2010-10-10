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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Navigation;

namespace IndoorWorx.UserProfile
{
    public class Module : IModule
    {
        private readonly IServiceLocator serviceLocator;
        public Module(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            NavigationService.AddNavigationLink(new Infrastructure.Models.NavigationInfo()
            {
                Content = "User Profile",
                IsAuthenticationRequired = true,
                NavigationUri = "/IndoorWorx.UserProfile.Silverlight;component/Views/UserProfile.dyn.xaml",
                PackageName = "IndoorWorx.UserProfile.Silverlight.xap",
                Allow = new string[] { "*" },
                Deny = new string[] { "?" }
            });
        }

        #endregion
    }
}
