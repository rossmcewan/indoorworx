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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Library.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IServiceLocator serviceLocator;
        public ActivityService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
        }

        private IndoorWorx.Library.ActivityServiceReference.ActivityServiceClient proxy = null;
        public IndoorWorx.Library.ActivityServiceReference.ActivityServiceClient Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = new IndoorWorx.Library.ActivityServiceReference.ActivityServiceClient();
                } 
                return proxy;
            }
        }

        #region ActivityType Methods
        public event EventHandler<DataEventArgs<ICollection<ActivityType>>> ActivityTypesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> ActivityTypesRetrievalError;

        public void RetrieveActivityTypes()
        {
            var activityTypes = Cache.Get("ActivityType") as ICollection<ActivityType>;
            if (activityTypes != null)
            {
                if (ActivityTypesRetrieved != null)
                    ActivityTypesRetrieved(this, new DataEventArgs<ICollection<ActivityType>>(activityTypes));
            }
            else
            {
                Proxy.RetrieveAllActivityTypesCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (ActivityTypesRetrievalError != null)
                            ActivityTypesRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (ActivityTypesRetrieved != null)
                            ActivityTypesRetrieved(this, new DataEventArgs<ICollection<ActivityType>>(e.Result));
                    }
                };
                proxy.RetrieveAllActivityTypesAsync();
            }
        }

        #endregion


    }
}
