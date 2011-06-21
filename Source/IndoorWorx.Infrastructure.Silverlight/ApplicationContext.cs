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
using IndoorWorx.Infrastructure.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IndoorWorx.Infrastructure
{
    public class ApplicationContext : INotifyPropertyChanged
    {
        public static ApplicationContext Initialize()
        {
            if (Current == null)
            {
                var context = new ApplicationContext();
                Application.Current.Resources.Add("ApplicationContext", context);
            }
            return Current;
        }

        private ApplicationContext()
        {
            ApplicationUser.CurrentUserChanged += (sender, e) =>
                {
                    if (ApplicationUser.CurrentUser != null)
                    {
                        VideoCount = ApplicationUser.CurrentUser.Videos.Count;
                        ApplicationUser.CurrentUser.PropertyChanged += UserPropertyChanged;
                        (ApplicationUser.CurrentUser.Videos as INotifyCollectionChanged).CollectionChanged += VideoCollectionChanged;
                    }
                    else
                    {
                        VideoCount = 0;
                    }
                };
        }

        private void UserPropertyChanged(object sender, PropertyChangedEventArgs args)
        {            
        }

        private void VideoCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            VideoCount = ApplicationUser.CurrentUser.Videos.Count;
        }

        private int videoCount;
        public int VideoCount
        {
            get { return videoCount; }
            set 
            { 
                videoCount = value;
                FirePropertyChanged("VideoCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ApplicationContext Current
        {
            get { return Application.Current.Resources["ApplicationContext"] as ApplicationContext; }
        }
    }
}
