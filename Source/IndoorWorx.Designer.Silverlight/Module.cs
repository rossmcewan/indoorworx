﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Navigation;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using IndoorWorx.Designer.Resources;
using IndoorWorx.Infrastructure;
using IndoorWorx.Designer.Helpers;
using IndoorWorx.Designer.Views;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
            eventAggregator.GetEvent<DesignEvent>().Subscribe(Design, true);
        }

        public void Design(TrainingSetTemplate template)
        {
            var view = serviceLocator.GetInstance<IDesignerView>();
            view.Model.SelectedTemplate = template;
            view.Model.Show();
        }

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        private INavigationLinks NavigationLinks
        {
            get { return serviceLocator.GetInstance<INavigationLinks>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("DesignerResources", new ResourceWrapper());

            //unityContainer.RegisterInstance<IDesignerPresentationModel>(unityContainer.Resolve<DesignerPresentationModel>(), new ContainerControlledLifetimeManager());
            //unityContainer.RegisterInstance<IDesignerView>(unityContainer.Resolve<TabbedDesignerView>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IDesignerPresentationModel, DesignerPresentationModel>();
            unityContainer.RegisterType<IDesignerView, TabbedDesignerView>();
            unityContainer.RegisterType<IIntervalDesignerPresentationModel, IntervalDesignerPresentationModel>();
            unityContainer.RegisterType<IIntervalDesignerView, IntervalDesignerView>();

            //NavigationLinks.MapUri(
            //    new Uri("/Designer", UriKind.Relative),
            //    new Uri("/IndoorWorx.Designer.Silverlight;component/Pages/DesignerPage.xaml", UriKind.Relative));

            //NavigationLinks.Add(new Infrastructure.Models.NavigationInfo()
            //{
            //    Content = "mydesigner",                
            //    IsAuthenticationRequired = true,
            //    NavigationUri = "/Designer",
            //    Allow = new string[] { "*" },
            //    Deny = new string[] { "?" }
            //});            
        }

        #endregion
    }
}
