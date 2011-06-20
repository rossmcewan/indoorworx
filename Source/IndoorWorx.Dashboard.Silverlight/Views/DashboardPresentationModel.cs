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

        public void Refresh()
        {            
        }

        #endregion
    }
}
