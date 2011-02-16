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

namespace IndoorWorx.ForMe.Views
{
    public partial class ProfileView : UserControl, IProfileView
    {
        public ProfileView(IProfilePresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
        }

      
        public IProfilePresentationModel Model
        {
            get { return this.DataContext as IProfilePresentationModel; }
        }

        private void changeProfilePictureButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void changePasswordButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
