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
using System.Windows.Navigation;
using IndoorWorx.Infrastructure;
using IndoorWorx.MyLibrary.Views;

namespace IndoorWorx.MyLibrary.Pages
{
    public partial class TrainingSetsPage : Page
    {
        bool reloadRequired = true;
        public TrainingSetsPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<ITrainingSetsView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as TrainingSetsPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private ITrainingSetsView View
        {
            get { return this.Content as ITrainingSetsView; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (reloadRequired)
                View.Model.Refresh();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }    
}
