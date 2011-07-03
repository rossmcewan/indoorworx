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
            Application.Current.InstallStateChanged += (sender, e) =>
                {
                    FirePropertyChanged("InstallState");
                };
            ApplicationUser.CurrentUserChanged += (sender, e) =>
                {
                    FirePropertyChanged("CurrentUser");
                    if (ApplicationUser.CurrentUser != null)
                    {
                        VideoCount = ApplicationUser.CurrentUser.Videos.Count;
                        (ApplicationUser.CurrentUser.Videos as INotifyCollectionChanged).CollectionChanged += VideoCollectionChanged;
                        TemplateCount = ApplicationUser.CurrentUser.Templates.Count;
                        (ApplicationUser.CurrentUser.Templates as INotifyCollectionChanged).CollectionChanged += TemplateCollectionChanged;
                    }
                    else
                    {
                        VideoCount = 0;
                    }
                };
        }

        private void VideoCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            VideoCount = ApplicationUser.CurrentUser.Videos.Count;
        }

        private void TemplateCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            TemplateCount = ApplicationUser.CurrentUser.Templates.Count;
        }

        public virtual ApplicationUser CurrentUser
        {
            get { return ApplicationUser.CurrentUser; }
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

        private int templateCount;
        public virtual int TemplateCount
        {
            get { return templateCount; }
            set
            {
                templateCount = value;
                FirePropertyChanged("TemplateCount");
            }
        }

        public bool IsRunningOutOfBrowser
        {
            get { return Application.Current.IsRunningOutOfBrowser; }
        }

        public InstallState InstallState
        {
            get { return Application.Current.InstallState; }
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
