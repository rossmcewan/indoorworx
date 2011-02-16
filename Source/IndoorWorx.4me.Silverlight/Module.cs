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
using IndoorWorx.ForMe.Helpers;
using IndoorWorx.ForMe.Views;
using Telerik.Windows.Controls;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.ForMe.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using IndoorWorx.Infrastructure;
using IndoorWorx.ForMe.Controls;

namespace IndoorWorx.ForMe
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;

        }

        private INavigationLinks NavigationLinks
        {
            get { return serviceLocator.GetInstance<INavigationLinks>(); }
        }

        private IForMePresentationModel ForMePresentationModel
        {
            get { return serviceLocator.GetInstance<IForMePresentationModel>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("ForMeResources", new ResourceWrapper());

            unityContainer.RegisterInstance<IForMePresentationModel>(unityContainer.Resolve<ForMePresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IForMeView>(unityContainer.Resolve<ForMeView>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IProfilePresentationModel>(unityContainer.Resolve<ProfilePresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IProfileView>(unityContainer.Resolve<ProfileView>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IActivitiesViewPresentationModel>(unityContainer.Resolve<ActivitiesViewPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IActivitiesView>(unityContainer.Resolve<ActivitiesView>(), new ContainerControlledLifetimeManager());

            NavigationLinks.MapUri(
                new Uri("/4me", UriKind.Relative),
                new Uri("/IndoorWorx.4me.Silverlight;component/Views/ForMePage.xaml", UriKind.Relative));

            NavigationLinks.Add(new Infrastructure.Models.NavigationInfo()
            {
                Content = "4me",
                IsAuthenticationRequired = true,
                NavigationUri = "/4me",
                Allow = new string[] { "?" },
                Deny = new string[] { "" }
            });

            AddNavigationItems();
            RegisterEvents();
        }

        #endregion


        private void RegisterEvents()
        {
            eventAggregator.GetEvent<ShowActivitiesEvent>().Subscribe( (view) =>
            {
                ForMePresentationModel.AddMainRegionView(view as IMainRegionView);
            }, ThreadOption.UIThread, true);

        }

       
        private void AddNavigationItems()
        {
            var profile = new RadTreeViewItem()
            {
                Header = Resources.ForMeResources.ProfileNavigationHeader,
                Tag  = unityContainer.Resolve<IProfileView>(),
                IsExpanded = true
            };

            profile.Items.Add(new RadTreeViewItem()
            { 
                Header = Resources.ForMeResources.ActivitiesNavigationHeader,
                Tag = unityContainer.Resolve<IActivitiesView>()
            });

            profile.Items.Add(new RadTreeViewItem()
            {
                Header = Resources.ForMeResources.SocialMediaAccountsNavigationHeader
            });

            ForMePresentationModel.AddNavigationItem(profile);

        }

    }
}
