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

namespace IndoorWorx.Silverlight
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<IShell>();
            shell.Show();
            return shell as DependencyObject;
        }

        protected override void ConfigureContainer()
        {
            //Container.RegisterType<IShell, Shell>();  
            Container.RegisterInstance<IShell>(Container.Resolve<Shell>());
            base.ConfigureContainer();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            //return ModuleCatalog.CreateFromXaml(new Uri("/IndoorWorx.Silverlight;component/ModulesCatalog.xaml", UriKind.Relative));
            return ModuleCatalog.CreateFromXaml(new Uri(PrismResources.ModulesCatalogLocation, UriKind.Relative));
        }
    }
}
