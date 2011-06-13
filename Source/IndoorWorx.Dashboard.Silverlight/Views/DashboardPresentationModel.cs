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
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using System.Collections.ObjectModel;

namespace IndoorWorx.Dashboard.Views
{
    public class DashboardPresentationModel : BaseModel, IDashboardPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        public DashboardPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region IDashboardPresentationModel Members

        public IDashboardView View { get; set; }

        private ICollection<Widget> availableWidgets = new ObservableCollection<Widget>();
        public ICollection<Widget> AvailableWidgets
        {
            get { return availableWidgets; }
            set
            {
                availableWidgets = value;
                FirePropertyChanged("AvailableWidgets");
            }
        }

        private ICollection<Widget> addedWidgets = new ObservableCollection<Widget>();
        public ICollection<Widget> AddedWidgets
        {
            get { return addedWidgets; }
            set
            {
                addedWidgets = value;
                FirePropertyChanged("AddedWidgets");
            }
        }

        public void AddWidget(Widget widget)
        {
            AddedWidgets.Add(widget);
            //if (!AddedWidgets.Any(x => x.Id == widget.Id))
            //{
            //    AddedWidgets.Add(widget);
            //}
        }

        public void Refresh()
        {
            //var currentUser = ApplicationUser.CurrentUser;
            //if (currentUser != null)
            //{
            //    availableWidgets.Clear();
            //    foreach (var widget in currentUser.AvailableWidgets)
            //        availableWidgets.Add(widget);
            //}
        }

        #endregion
    }
}
