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
            StyleManager.ApplicationTheme = new Telerik.Windows.Controls.TransparentTheme();
            //ExpressionDarkTheme.SetIsApplicationTheme(this, true);
            SmartDispatcher.Initialize(Deployment.Current.Dispatcher);
            InitializeComponent();

            // Create a WebContext and add it to the ApplicationLifetimeObjects
            // collection.  This will then be available as WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // This will enable you to bind controls in XAML files to WebContext.Current
            // properties
            this.Resources.Add("WebContext", WebContext.Current);
            this.Resources.Add("ApplicationContext", new ApplicationContext());

            // This will automatically authenticate a user when using windows authentication
            // or when the user chose "Keep me signed in" on a previous login attempt
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Show some UI to the user while LoadUser is in progress
            //this.InitializeRootVisual();
            new Bootstrapper().Run();
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
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}