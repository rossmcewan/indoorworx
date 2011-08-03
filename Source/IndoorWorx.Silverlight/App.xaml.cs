namespace IndoorWorx.Silverlight
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using IndoorWorx.Silverlight.Controls;
    using Telerik.Windows.Controls;
    using System.Windows.Controls.Theming;
    using IndoorWorx.Infrastructure;
    using IndoorWorx.Infrastructure.Services;
    using IndoorWorx.Infrastructure.Models;
    using IndoorWorx.Silverlight.Views;
    using IndoorWorx.Library.Services;
    using Microsoft.Practices.Composite.Events;
    using System.Collections.Generic;
    using System.Windows.Browser;
    using IndoorWorx.Silverlight.Web;
    using IndoorWorx.Library.Views;
    using System.Diagnostics;
    using Telerik.Windows.Controls.DragDrop;

    /// <summary>
    /// Main <see cref="Application"/> class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates a new <see cref="App"/> instance.
        /// </summary>
        public App()
        {
            RadDragAndDropManager.ExecutionMode = DragExecutionMode.Legacy;
            InitializeComponent();

            if (Application.Current.IsRunningOutOfBrowser)
            {
                StyleManager.ApplicationTheme = new Telerik.Windows.Controls.MetroTheme();
                SmartDispatcher.Initialize(Deployment.Current.Dispatcher);
                
                // Create a WebContext and add it to the ApplicationLifetimeObjects
                // collection.  This will then be available as WebContext.Current.
                WebContext webContext = new WebContext();
                webContext.Authentication = new FormsAuthentication();
                
                this.ApplicationLifetimeObjects.Add(webContext);
            }
        }        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationContext.Initialize();
            this.RootVisual = new UserControlContainer();
            if (Application.Current.IsRunningOutOfBrowser)
            {
                Application.Current.CheckAndDownloadUpdateCompleted += (obj, args) =>
                {
                    if (args.UpdateAvailable)
                    {
                        UpdateAvailableWindow.Show(()=>Application.Current.MainWindow.Close());                        
                    }
                    else
                    {
                        var settingsUriString = "Settings.xml";
                        Uri source = Application.Current.Host.Source;

                        string location;
                        if (Debugger.IsAttached)
                        {
                            location = "http://localhost:3415/";
                        }
                        else
                        {
                            location = source.AbsoluteUri.Substring(0, source.AbsoluteUri.IndexOf("ClientBin", StringComparison.OrdinalIgnoreCase));
                        }

                        settingsUriString = String.Concat(location, settingsUriString);

                        Uri settingsUri = new Uri(settingsUriString, UriKind.Absolute);

                        SettingsClient settingsService = new SettingsClient(settingsUri);
                        settingsService.GetSettingsCompleted += this.SettingsService_GetSettingsCompleted;
                        settingsService.GetSettingsAsync();
                    }
                };
                Application.Current.CheckAndDownloadUpdateAsync();
            }
            else
            {
                RootContainer.SwitchControl(new ApplicationInstallerView());
            }
        }

        public UserControlContainer RootContainer
        {
            get { return this.RootVisual as UserControlContainer; }
        }

        private void SettingsService_GetSettingsCompleted(object sender, DataEventArgs<IDictionary<string, string>> args)
        {
            IDictionary<string, string> settings = args.Value;
            SmartDispatcher.BeginInvoke(() => this.Run(settings));
        }

        private void Run(IDictionary<string, string> settings)
        {
            // This will enable you to bind controls in XAML files to WebContext.Current
            // properties
            this.Resources.Add("WebContext", WebContext.Current);

            (WebContext.Current.Authentication as WebAuthenticationService).DomainContext = new AuthenticationContext(new Uri(settings["AuthenticationUri"], UriKind.Absolute));

            // This will automatically authenticate a user when using windows authentication
            // or when the user chose "Keep me signed in" on a previous login attempt
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Show some UI to the user while LoadUser is in progress
            //this.InitializeRootVisual();
            new Bootstrapper(settings).Run();
        }

        /// <summary>
        /// Invoked when the <see cref="LoadUserOperation"/> completes. Use this
        /// event handler to switch from the "loading UI" you created in
        /// <see cref="InitializeRootVisual"/> to the "application UI"
        /// </summary>
        private void Application_UserLoaded(LoadUserOperation operation)
        {
            if (operation.User != null && operation.User.Identity != null && operation.User.Identity.IsAuthenticated)
            {
                var userService = IoC.Resolve<IApplicationUserService>();
                if (userService != null)
                {
                    userService.ApplicationUserRetrieved += (sender, e) =>
                        {
                            ApplicationUser.CurrentUser = e.Value;
                        };
                    userService.ApplicationUserRetrievalError += (sender, e) =>
                        {
                            ErrorWindow.CreateNew(e.Value);
                        };
                    userService.RetrieveApplicationUser(operation.User.Identity.Name);
                }
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // a ChildWindow control.
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            //}
        }
    }
}