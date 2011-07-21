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
using IndoorWorx.MyLibrary.Resources;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Services;

namespace IndoorWorx.MyLibrary.Views
{
    public class IntervalPresentationModel : BaseModel, IIntervalPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        public IntervalPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IIntervalView View { get; set; }

        private Interval interval;
        public Interval Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
                FirePropertyChanged("Interval");
            }
        }

        public string Title
        {
            get
            {
                switch (Mode)
                {
                    case CrudOperation.Create:
                        return MyLibraryResources.NewIntervalTitle;
                    case CrudOperation.Read:
                        return MyLibraryResources.ViewIntervalTitle;
                    case CrudOperation.Update:
                        return MyLibraryResources.EditIntervalTitle;
                    case CrudOperation.Delete:
                        return MyLibraryResources.DeleteIntervalTitle;
                }
                return string.Empty;
            }
        }

        private CrudOperation mode = CrudOperation.Read;
        public virtual CrudOperation Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                FirePropertyChanged("Mode");
                FirePropertyChanged("Title");
            }
        }        
    }
}
