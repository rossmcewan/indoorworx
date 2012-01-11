using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Enums;

namespace IndoorWorx.Settings.Views
{
    public interface IGeneralSettingsPresentationModel
    {
        void Initialize();

        IGeneralSettingsView View { get; set; }

        string FirstName { get; set; }

        string Surname { get; set; }

        Genders Gender { get; set; }

        string About { get; set; }

        int FTP { get; set; }

        int FTHR { get; set; }
    }
}
