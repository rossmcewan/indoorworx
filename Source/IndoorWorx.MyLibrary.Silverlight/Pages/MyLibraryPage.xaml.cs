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

namespace IndoorWorx.MyLibrary
{
    public partial class MyLibraryPage : Page
    {
        public MyLibraryPage()
        {
            InitializeComponent();
            var content = IoC.Resolve<IMyLibraryView>() as UserControl;
            if (content.Parent != null)
            {
                (content.Parent as MyLibraryPage).Content = null;
            }
            this.Content = content;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
