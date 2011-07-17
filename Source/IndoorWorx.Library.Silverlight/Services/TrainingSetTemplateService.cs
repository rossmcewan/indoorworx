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
using System.ServiceModel;

namespace IndoorWorx.Library.Services
{
    public class TrainingSetTemplateService : ITrainingSetTemplateService
    {
        public event EventHandler<DataEventArgs<ICollection<TrainingSetTemplate>>> TrainingSetTemplatesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateRetrievalError;

        private readonly IServiceLocator serviceLocator;
        private Uri serviceAddress;
        public TrainingSetTemplateService(IServiceLocator serviceLocator, IConfigurationService configurationService)
        {
            this.serviceLocator = serviceLocator;
            this.serviceAddress = new Uri(configurationService.GetParameterValue("TrainingSetTemplateServiceUri"), UriKind.Absolute);
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
                return;
            }
            var proxy = CreateTrainingSetTemplateServiceClient();
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

        private IndoorWorx.Library.TrainingSetTemplateServiceReference.TrainingSetTemplateServiceClient CreateTrainingSetTemplateServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                Name = "TrainingSetTemplateServiceBinding",
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647,
            };

            EndpointAddress endpointAddress = new EndpointAddress(this.serviceAddress);

            return new IndoorWorx.Library.TrainingSetTemplateServiceReference.TrainingSetTemplateServiceClient(binding, endpointAddress);
        }
    }
}
