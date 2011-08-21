using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Responses;
using IndoorWorx.Infrastructure.Requests;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface ITrainingSetService
    {
        [OperationContract]
        CreateTrainingSetResponse CreateTrainingSet(CreateTrainingSetRequest request);
    }
}
