namespace IndoorWorx.Silverlight
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Telerik.Windows.Documents.Model;
    using System.IO;
    using Telerik.Windows.Documents.FormatProviders;
    using System.Windows;
    using Telerik.Windows.Documents.Layout;
    using Telerik.Windows.Documents;
    using System;
    using System.Reflection;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
       

        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();
            this.Title = ApplicationStrings.HomePageTitle;
        }

        

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

      

    }
}