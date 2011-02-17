using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IndoorWorx.Infrastructure.Services;

namespace IndoorWorx.Silverlight.Services
{
    public class AuthenticationOperations : IAuthenticationOperations
    {
        #region IAuthenticationOperations Members

        public bool IsAuthenticated
        {
            get { return WebContext.Current.User != null && WebContext.Current.User.IsAuthenticated; }
        }

        public bool IsInRole(string role)
        {
            return WebContext.Current.User != null && WebContext.Current.User.IsInRole(role);
        }

        public bool IsLoadingUser
        {
            get { return WebContext.Current.Authentication.IsLoadingUser; }
        }

        public bool IsLoggingIn
        {
            get { return WebContext.Current.Authentication.IsLoggingIn; }
        }

        public bool IsLoggingOut
        {
            get { return WebContext.Current.Authentication.IsLoggingOut; }
        }

        public bool IsSavingUser
        {
            get { return WebContext.Current.Authentication.IsSavingUser; }
        }

        #endregion
    }
}
