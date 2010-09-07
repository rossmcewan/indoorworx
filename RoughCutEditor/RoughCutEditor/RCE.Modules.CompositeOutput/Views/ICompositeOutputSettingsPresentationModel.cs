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
using RCE.Infrastructure;

namespace RCE.Modules.CompositeOutput.Views
{
    public interface ICompositeOutputSettingsPresentationModel : IHeaderInfoProvider<string>
    {
        ICompositeOutputSettingsView View { get; }

        ICommand GenerateCompositeStreamManifestCommand { get; }
    }
}
