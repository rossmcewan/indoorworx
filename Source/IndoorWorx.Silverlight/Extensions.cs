using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Silverlight.Assets.Resources;
using IndoorWorx.Silverlight.Web;

namespace IndoorWorx.Silverlight
{
    public static class Extensions
    {
        public static HyperlinkButton AsHyperlinkButton(this NavigationInfo navigationInfo)
        {
            HyperlinkButton hb = new HyperlinkButton();
            hb.Style = Application.Current.Resources[ResourceDictionaryKeys.LinkStyle] as Style;
            hb.Content = navigationInfo.Content;
            hb.Tag = navigationInfo;
            hb.NavigateUri = new Uri(navigationInfo.NavigationUri, UriKind.Relative);
            return hb;
        }

        //public static bool IsAuthorized(this NavigationInfo navigationInfo)
        //{
        //    return navigationInfo.IsAuthorized(WebContext.Current.User);
        //}

        //public static bool IsAuthorized(this NavigationInfo navigationInfo, User user)
        //{
        //    return !navigationInfo.IsAuthenticationRequired ||
        //        (user.IsAuthenticated && (navigationInfo.Roles.Intersect(user.Roles).Count() == navigationInfo.Roles.Count));
        //}

        public static string GetAllowedRolesAsString(this NavigationInfo navigationInfo)
        {
            var result = string.Empty;
            for (int i = 0; i < navigationInfo.Allow.Count; i++)
            {
                result = string.Concat(navigationInfo.Allow.ElementAt(i), i == navigationInfo.Allow.Count - 1 ? string.Empty : ",");
            }
            return result;
        }

        public static string GetDeniedRolesAsString(this NavigationInfo navigationInfo)
        {
            var result = string.Empty;
            for (int i = 0; i < navigationInfo.Deny.Count; i++)
            {
                result = string.Concat(navigationInfo.Deny.ElementAt(i), i == navigationInfo.Deny.Count - 1 ? string.Empty : ",");
            }
            return result;
        }
    }
}
