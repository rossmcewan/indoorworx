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
using IndoorWorx.Infrastructure.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Settings.Views;
using IndoorWorx.Infrastructure;
using IndoorWorx.Settings.Helpers;

namespace IndoorWorx.Settings
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;

            eventAggregator.GetEvent<SettingsEvent>().Subscribe(ShowSettings, ThreadOption.UIThread, true);
        }

        public void ShowSettings(object arg)
        {
            var settingsView = serviceLocator.GetInstance<ISettingsView>();
            settingsView.Show();
        }

        public void Initialize()
        {
            Application.Current.Resources.Add("SettingsResources", new ResourceWrapper());
            unityContainer.RegisterInstance<ISettingsPresentationModel>(unityContainer.Resolve<SettingsPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ISettingsView>(unityContainer.Resolve<SettingsView>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IGeneralSettingsPresentationModel>(unityContainer.Resolve<GeneralSettingsPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IGeneralSettingsView>(unityContainer.Resolve<GeneralSettingsView>(), new ContainerControlledLifetimeManager());
        }
    }
}
