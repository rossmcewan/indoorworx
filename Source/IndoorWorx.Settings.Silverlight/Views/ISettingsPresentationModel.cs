using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IndoorWorx.Settings.Views
{
    public interface ISettingsPresentationModel
    {
        ISettingsView View { get; set; }

        ICommand CancelCommand { get; }
    }
}
