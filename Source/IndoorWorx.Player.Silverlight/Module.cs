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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Navigation;
using IndoorWorx.Player.Views;
using IndoorWorx.Player.Helpers;

namespace IndoorWorx.Player
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
        }

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("PlayerResources", new ResourceWrapper());

            unityContainer.RegisterType<IPlayerPresentationModel, PlayerPresentationModel>();
            unityContainer.RegisterType<IPlayerView, PlayerView>();
            //unityContainer.RegisterInstance<IPlayerPresentationModel>(unityContainer.Resolve<PlayerPresentationModel>(), new ContainerControlledLifetimeManager());
            //unityContainer.RegisterInstance<IPlayerView>(unityContainer.Resolve<PlayerView>(), new ContainerControlledLifetimeManager());

            //NavigationService.AddNavigationLink(new Infrastructure.Models.NavigationInfo()
            //{
            //    Content = "Player",
            //    IsAuthenticationRequired = true,
            //    NavigationUri = "/IndoorWorx.Player.Silverlight;component/Views/Dynamic/PlayerShim.xaml",
            //    PackageName = "IndoorWorx.Player.Silverlight.xap",
            //    Allow = new string[] { "?" },
            //    Deny = new string[] { "" }
            //});
        }

        #endregion
    }
}
