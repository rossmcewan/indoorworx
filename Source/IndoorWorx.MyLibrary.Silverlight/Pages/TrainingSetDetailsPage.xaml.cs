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
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.MyLibrary.Views;

namespace IndoorWorx.MyLibrary.Pages
{
    public partial class TrainingSetDetailsPage : Page
    {
        bool reloadRequired = true;
        public TrainingSetDetailsPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<ITrainingSetDetailsView>() as UserControl;            
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as TrainingSetDetailsPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
            IoC.Resolve<IAuthenticationOperations>().LoggedOut += (sender, e) => SetContentNull();
            View.Model.TrainingSetRemoved += (sender, e) => SetContentNull();
        }

        private void SetContentNull()
        {
            SmartDispatcher.BeginInvoke(() => Content = null);
        }

        private ITrainingSetDetailsView View
        {
            get { return this.Content as ITrainingSetDetailsView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            string trainingSetId;
            if (this.NavigationContext.QueryString.TryGetValue("id", out trainingSetId))
            {
                View.Model.SelectTrainingSetWithId(new Guid(trainingSetId));
            }
        }
    }
}
