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
using Microsoft.Practices.Composite.Presentation.Commands;

namespace IndoorWorx.Settings.Views
{
    public class SettingsPresentationModel : ISettingsPresentationModel
    {
        public SettingsPresentationModel()
        {
            this.CancelCommand = new DelegateCommand<object>(Cancel);
        }

        public ISettingsView View { get; set; }

        public ICommand CancelCommand { get; private set; }

        public void Cancel(object arg)
        {
            View.Hide();
        }
    }
}
