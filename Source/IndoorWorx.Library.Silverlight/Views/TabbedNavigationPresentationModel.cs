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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using Telerik.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Library.Views
{
    public class TabbedNavigationPresentationModel : BaseModel, ITabbedNavigationPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;

        public TabbedNavigationPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
        }

        private bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                FirePropertyChanged("IsBusy");
            }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                this.selectedItem = value;
                SelectedItemChanged();
                FirePropertyChanged("SelectedItem");
            }
        }

        private void SelectedItemChanged()
        {
            if (selectedItem is RadTreeViewItem)
            {
                var item = selectedItem as RadTreeViewItem;
                if (item.Tag is IMainRegionView && item.Tag is UserControl)
                {
                    var contains = MainRegionViews.Where(v => v.Content == item.Tag).FirstOrDefault();
                    if (contains == null)
                    {
                        AddMainRegionView(item.Tag as IMainRegionView);
                    }
                    else
                    {
                        //TODO Bring Tab to front
                    }
                }
            }
        }

        private ITabbedNavigationView view;
        public ITabbedNavigationView View
        {
            get
            {
                return this.view;
            }
            set
            {
                this.view = value;
                FirePropertyChanged("View");
            }
        }

        private ICollection<RadTreeViewItem> navigationItems = new ObservableCollection<RadTreeViewItem>();
        public ICollection<RadTreeViewItem> NavigationItems
        {
            get { return navigationItems; }
            set
            {
                navigationItems = value;
                FirePropertyChanged("NavigationItems");
            }
        }

        private ICollection<RadPane> mainRegionViews = new ObservableCollection<RadPane>();
        public ICollection<RadPane> MainRegionViews
        {
            get { return mainRegionViews; }
            set
            {
                mainRegionViews = value;
                FirePropertyChanged("MainRegionViews");
            }
        }

        public void AddNavigationItem(RadTreeViewItem navigationItem)
        {
            this.NavigationItems.Add(navigationItem);
        }


        public void AddMainRegionView(IMainRegionView mainRegionView)
        {
            if (mainRegionView != null)
            {
                this.MainRegionViews.Add(
                    new RadPane()
                    {
                        Header = mainRegionView.Header,
                        Content = mainRegionView as UserControl,
                        CanUserClose = true,
                        CanFloat = false
                        
                    });
            }
        }

    }
}
