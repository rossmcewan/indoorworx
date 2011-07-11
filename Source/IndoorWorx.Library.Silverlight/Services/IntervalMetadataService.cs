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
using Microsoft.Practices.Composite.Events;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Library.Services
{
    public class IntervalMetadataService : IIntervalMetadataService
    {
        private IntervalMetadataServiceReference.IntervalMetadataServiceClient proxy = new IntervalMetadataServiceReference.IntervalMetadataServiceClient();        
        private readonly IServiceLocator serviceLocator;
        public IntervalMetadataService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
        }

        public event EventHandler<DataEventArgs<ICollection<IntervalLevel>>> IntervalLevelsRetrieved;

        public event EventHandler<DataEventArgs<Exception>> IntervalLevelRetrievalError;

        public void RetrieveIntervalLevels()
        {
            var intervalLevels = Cache.Get("IntervalLevels") as ICollection<IntervalLevel>;
            if (intervalLevels != null)
            {
                if (IntervalLevelsRetrieved != null)
                    IntervalLevelsRetrieved(this, new DataEventArgs<ICollection<IntervalLevel>>(intervalLevels));
            }
            proxy.FetchIntervalLevelsCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (IntervalLevelRetrievalError != null)
                            IntervalLevelRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (IntervalLevelsRetrieved != null)
                            IntervalLevelsRetrieved(this, new DataEventArgs<ICollection<IntervalLevel>>(e.Result));
                    }
                };
            proxy.FetchIntervalLevelsAsync();
        }

        public event EventHandler<DataEventArgs<ICollection<IntervalType>>> IntervalTypesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> IntervalTypesRetrievalError;

        public void RetrieveIntervalTypes()
        {
            var intervalTypes = Cache.Get("IntervalTypes") as ICollection<IntervalType>;
            if (intervalTypes != null)
            {
                if (IntervalTypesRetrieved != null)
                    IntervalTypesRetrieved(this, new DataEventArgs<ICollection<IntervalType>>(intervalTypes));
            }
            proxy.FetchIntervalTypesCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (IntervalTypesRetrievalError != null)
                        IntervalTypesRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (IntervalTypesRetrieved != null)
                        IntervalTypesRetrieved(this, new DataEventArgs<ICollection<IntervalType>>(e.Result));
                }
            };
            proxy.FetchIntervalTypesAsync();
        }

        public event EventHandler<DataEventArgs<ICollection<EffortType>>> EffortTypesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> EffortTypesRetrievalError;

        public void RetrieveEffortTypes()
        {
            var intervalTypes = Cache.Get("EffortTypes") as ICollection<EffortType>;
            if (intervalTypes != null)
            {
                if (EffortTypesRetrieved != null)
                    EffortTypesRetrieved(this, new DataEventArgs<ICollection<EffortType>>(intervalTypes));
            }
            proxy.FetchEffortTypesCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (EffortTypesRetrievalError != null)
                        EffortTypesRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (EffortTypesRetrieved != null)
                        EffortTypesRetrieved(this, new DataEventArgs<ICollection<EffortType>>(e.Result));
                }
            };
            proxy.FetchEffortTypesAsync();
        }
    }
}
