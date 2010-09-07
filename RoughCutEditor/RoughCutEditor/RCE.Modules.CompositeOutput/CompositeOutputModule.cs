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
using Microsoft.Practices.Composite.Regions;
using RCE.Modules.CompositeOutput.Services;
using RCE.Modules.CompositeOutput.Views;
using RCE.Infrastructure;

namespace RCE.Modules.CompositeOutput
{
    public class CompositeOutputModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;
        public CompositeOutputModule(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            this.RegisterViewsAndServices();
            var presentationModel = unityContainer.Resolve<ICompositeOutputSettingsPresentationModel>();
            this.regionManager.RegisterViewWithRegionInIndex(RegionNames.ToolsRegion, presentationModel.View, 4);
        }

        #endregion

        private void RegisterViewsAndServices()
        {
            unityContainer.RegisterType<ICompositeStreamManifestGeneratorService, CompositeStreamManifestGeneratorService>();
            unityContainer.RegisterType<ICompositeOutputSettingsView, CompositeOutputSettingsView>();
            unityContainer.RegisterType<ICompositeOutputSettingsPresentationModel, CompositeOutputSettingsPresentationModel>();
        }
    }
}
