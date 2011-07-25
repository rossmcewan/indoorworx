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

namespace IndoorWorx.Infrastructure.Services
{
    public interface IAuthenticationOperations
    {
        event EventHandler LoggedIn;

        event EventHandler LoggedOut;

        bool IsAuthenticated { get; }

        bool IsInRole(string role);

        bool IsLoadingUser { get; }

        bool IsLoggingIn { get; }

        bool IsLoggingOut { get; }

        bool IsSavingUser { get; }
    }
}
