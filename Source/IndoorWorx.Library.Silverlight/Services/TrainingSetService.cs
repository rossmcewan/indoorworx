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
using IndoorWorx.Infrastructure.Models;
using System.ServiceModel;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Responses;
using IndoorWorx.Infrastructure.Requests;

namespace IndoorWorx.Library.Services
{
    public class TrainingSetService : ITrainingSetService
    {
        private readonly Uri serviceAddress;
        private readonly IServiceLocator serviceLocator;
        public TrainingSetService(IServiceLocator serviceLocator, IConfigurationService configService)
        {
            this.serviceLocator = serviceLocator;
            this.serviceAddress = new Uri(configService.GetParameterValue("TrainingSetServiceUri"), UriKind.Absolute);
        }

        public event EventHandler<DataEventArgs<CreateTrainingSetResponse>> TrainingSetCreated;

        public event EventHandler<DataEventArgs<Exception>> CreateTrainingSetError;

        public void CreateTrainingSet(TrainingSet trainingSet)
        {
            var proxy = CreateTrainingSetServiceClient();
            proxy.CreateTrainingSetCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (CreateTrainingSetError != null)
                        CreateTrainingSetError(this, new DataEventArgs<Exception>(e.Error));
                }
                else
                {
                    if (TrainingSetCreated != null)
                        TrainingSetCreated(this, new DataEventArgs<CreateTrainingSetResponse>(e.Result));
                }
            };
            proxy.CreateTrainingSetAsync(new CreateTrainingSetRequest()
            {                
                User = ApplicationUser.CurrentUser.Username,
                TrainingSet = trainingSet
            });
        }

        private IndoorWorx.Library.TrainingSetServiceReference.TrainingSetServiceClient CreateTrainingSetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                Name = "TrainingSetServiceBinding",
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647,
            };

            EndpointAddress endpointAddress = new EndpointAddress(this.serviceAddress);

            return new IndoorWorx.Library.TrainingSetServiceReference.TrainingSetServiceClient(binding, endpointAddress);
        }
    }
}
