﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Settings.Views
{
    public interface ISettingsView
    {
        ISettingsPresentationModel Model { get; }

        void Show();

        void Hide();
    }
}
