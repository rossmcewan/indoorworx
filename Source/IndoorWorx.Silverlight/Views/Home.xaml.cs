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
using System.Windows.Threading;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
        private DispatcherTimer timer;

        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();
            this.Title = ApplicationStrings.HomePageTitle;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.coverFlow.SelectedIndex = (this.coverFlow.SelectedIndex + 1) % (this.coverFlow.Items.Count);
        }
       
        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

      

    }
}