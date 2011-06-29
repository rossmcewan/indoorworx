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
using Microsoft.Practices.Composite.UnityExtensions;
using IndoorWorx.Infrastructure;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Modularity;
using IndoorWorx.Silverlight.Assets.Resources;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Silverlight.Services;
using System.Collections.Generic;
using IndoorWorx.Library.Services;

namespace IndoorWorx.Silverlight
{
    public class Bootstrapper : UnityBootstrapper
    {
        private readonly IDictionary<string, string> settings;
        public Bootstrapper(IDictionary<string, string> settings)
        {
            this.settings = settings;
        }

        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<IShell>();
            shell.Show();
            return shell as DependencyObject;
        }

        protected override void ConfigureContainer()
        {
            var configurationService = new ConfigurationService(settings);
            Container.RegisterInstance<IConfigurationService>(configurationService, new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IAuthenticationOperations>(Container.Resolve<AuthenticationOperations>());
            Container.RegisterInstance<IShell>(Container.Resolve<Shell>());          
            base.ConfigureContainer();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            IConfigurationService configurationService = this.Container.Resolve<IConfigurationService>();

            string moduleCatalog = configurationService.GetParameterValue("ModulesCatalog");

            Uri moduleCatelogUri = new Uri(moduleCatalog, UriKind.Relative);
            IModuleCatalog catalog = ModuleCatalog.CreateFromXaml(moduleCatelogUri);

            return catalog;
            //return ModuleCatalog.CreateFromXaml(new Uri("/IndoorWorx.Silverlight;component/ModulesCatalog.xaml", UriKind.Relative));
            //return ModuleCatalog.CreateFromXaml(new Uri(PrismResources.ModulesCatalogLocation, UriKind.Relative));
        }
    }
}
