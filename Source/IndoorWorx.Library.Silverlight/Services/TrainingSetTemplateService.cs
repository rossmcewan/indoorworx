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
    public class TrainingSetTemplateService : ITrainingSetTemplateService
    {
        public event EventHandler<DataEventArgs<ICollection<TrainingSetTemplate>>> TrainingSetTemplatesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateRetrievalError;

        private readonly IServiceLocator serviceLocator;
        public TrainingSetTemplateService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
        }

        public void RetrieveTrainingSetTemplates()
        {
            var templates = Cache.Get("TrainingSetTemplates") as ICollection<TrainingSetTemplate>;
            if (templates != null)
            {
                if (TrainingSetTemplatesRetrieved != null)
                    TrainingSetTemplatesRetrieved(this, new DataEventArgs<ICollection<TrainingSetTemplate>>(templates));
            }
            var proxy = new IndoorWorx.Library.TrainingSetTemplateServiceReference.TrainingSetTemplateServiceClient();
            proxy.FindAllCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (TrainingSetTemplateRetrievalError != null)
                        TrainingSetTemplateRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    Cache.Add("TrainingSetTemplates", e.Result, TimeSpan.FromMinutes(10));
                    if (TrainingSetTemplatesRetrieved != null)
                        TrainingSetTemplatesRetrieved(this, new DataEventArgs<ICollection<TrainingSetTemplate>>(e.Result));
                }
            };
            proxy.FindAllAsync();
        }
    }
}
