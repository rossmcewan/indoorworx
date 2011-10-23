using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IndoorWorx.Settings.Views
{
    public partial class GeneralSettingsView : UserControl, IGeneralSettingsView
    {
        public GeneralSettingsView(IGeneralSettingsPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public IGeneralSettingsPresentationModel Model
        {
            get { return this.DataContext as IGeneralSettingsPresentationModel; }
        }
    }
}
