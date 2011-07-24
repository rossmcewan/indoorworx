using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Requests;
using IndoorWorx.Infrastructure.Responses;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface ITrainingSetTemplateService
    {
        [OperationContract]
        ICollection<TrainingSetTemplate> FindAll();

        [OperationContract]
        SaveTemplateResponse Save(SaveTemplateRequest request);

        [OperationContract]
        RemoveTemplateResponse Remove(RemoveTemplateRequest request);
    }
}
