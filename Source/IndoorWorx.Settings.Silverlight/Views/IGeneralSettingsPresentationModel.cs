using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Settings.Views
{
    public interface IGeneralSettingsPresentationModel
    {
        IGeneralSettingsView View { get; set; }
    }
}
