namespace IndoorWorx.Silverlight.LoginUI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using Telerik.Windows.Controls;

    /// <summary>
    /// <see cref="ChildWindow"/> class that controls the registration process.
    /// </summary>
    public partial class LoginRegistrationWindow : RadWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();
        private LoginForm loginForm = new LoginForm();
        private RegistrationForm registrationForm = new RegistrationForm();

        /// <summary>
        /// Creates a new <see cref="LoginRegistrationWindow"/> instance.
        /// </summary>
        public LoginRegistrationWindow()
        {
            InitializeComponent();
            this.registrationForm.SetParentWindow(this);
            this.loginForm.SetParentWindow(this);
            NavigateToLogin();
        }

        /// <summary>
        /// Notifies the <see cref="LoginRegistrationWindow"/> window that it can only close
        /// if <paramref name="operation"/> is finished or can be cancelled
        /// </summary>
        /// <param name="operation">The pending operation to monitor</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }


        public virtual void NavigateToLogin()
        {
            this.radTransitionControl.Content = loginForm;
            this.Header = loginForm.Header;
        }


        public virtual void NavigateToRegistration()
        {
            this.radTransitionControl.Content = registrationForm;
            this.Header = registrationForm.Header;
        }

        /// <summary>
        /// Prevents the window from closing while there are operations in progress
        /// </summary>
        private void LoginWindow_Closing(object sender, CancelEventArgs eventArgs)
        {
            foreach (OperationBase operation in this.possiblyPendingOperations)
            {
                if (!operation.IsComplete)
                {
                    if (operation.CanCancel)
                    {
                        operation.Cancel();
                    }
                    else
                    {
                        eventArgs.Cancel = true;
                    }
                }
            }
        }
    }
}