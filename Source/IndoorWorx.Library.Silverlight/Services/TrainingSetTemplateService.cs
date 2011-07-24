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
using IndoorWorx.Infrastructure.Requests;
using IndoorWorx.Infrastructure.Responses;

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

        public event EventHandler<DataEventArgs<SaveTemplateResponse>> TrainingSetTemplateSaved;

        public event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateSaveError;

        public void SaveTemplate(TrainingSetTemplate template)
        {
            var proxy = CreateTrainingSetTemplateServiceClient();
            proxy.SaveCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (TrainingSetTemplateSaveError != null)
                            TrainingSetTemplateSaveError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (TrainingSetTemplateSaved != null)
                            TrainingSetTemplateSaved(this, new DataEventArgs<SaveTemplateResponse>(e.Result));
                    }
                };
            proxy.SaveAsync(new SaveTemplateRequest()
            {
                Template = template,
                User = ApplicationUser.CurrentUser.Username
            });
        }

        public event EventHandler<DataEventArgs<Infrastructure.Responses.RemoveTemplateResponse>> TrainingSetTemplateRemoved;

        public event EventHandler<DataEventArgs<Exception>> TrainingSetTemplateRemoveError;

        public void RemoveTemplate(TrainingSetTemplate template)
        {
            var proxy = CreateTrainingSetTemplateServiceClient();
            proxy.RemoveCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (TrainingSetTemplateRemoveError != null)
                        TrainingSetTemplateRemoveError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (TrainingSetTemplateRemoved != null)
                        TrainingSetTemplateRemoved(this, new DataEventArgs<RemoveTemplateResponse>(e.Result));
                }
            };
            proxy.RemoveAsync(new RemoveTemplateRequest()
            {
                Template = template,
                User = ApplicationUser.CurrentUser.Username
            });
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
