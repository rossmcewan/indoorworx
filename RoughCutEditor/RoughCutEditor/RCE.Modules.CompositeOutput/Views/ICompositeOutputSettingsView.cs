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

namespace RCE.Modules.CompositeOutput.Views
{
    public interface ICompositeOutputSettingsView
    {
        ICompositeOutputSettingsPresentationModel Model { get; set; }

        void ShowProgressBar();

        void HideProgressBar();
    }
}
