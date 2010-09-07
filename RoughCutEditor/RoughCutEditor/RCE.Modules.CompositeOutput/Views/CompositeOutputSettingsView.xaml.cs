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

namespace RCE.Modules.CompositeOutput.Views
{
    public partial class CompositeOutputSettingsView : UserControl, ICompositeOutputSettingsView
    {
        public CompositeOutputSettingsView()
        {
            InitializeComponent();
        }

        public ICompositeOutputSettingsPresentationModel Model
        {
            get { return this.DataContext as ICompositeOutputSettingsPresentationModel; }
            set { this.DataContext = value; }
        }

        /// <summary>
        /// Shows a progress bar.
        /// </summary>
        public void ShowProgressBar()
        {
            this.ProgressBar.Visibility = System.Windows.Visibility.Visible;
            this.Spinner.BeginAnimation();
        }

        /// <summary>
        /// Hides the progress bar.
        /// </summary>
        public void HideProgressBar()
        {
            this.ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Spinner.StopAnimation();
        }
    }
}
