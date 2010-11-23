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
using IndoorWorx.Infrastructure.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace IndoorWorx.Library.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceLocator serviceLocator;
        public NavigationService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        private INavigationLinks Links
        {
            get { return serviceLocator.GetInstance<INavigationLinks>(); }
        }

        #region INavigationService Members

        public void AddNavigationLink(Infrastructure.Models.NavigationInfo info)
        {
            Links.Add(info);
        }

        public bool RemoveNavigationLink(Infrastructure.Models.NavigationInfo info)
        {
            return Links.Remove(info);
        }

        public void NavigateTo(Uri uri)
        {
            Application.Current.Host.NavigationState = uri.ToString();
        }

        #endregion
    }
}
