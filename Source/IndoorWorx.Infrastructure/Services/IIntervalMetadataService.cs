using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Services
{
    [ServiceContract]
    public interface IIntervalMetadataService
    {
        [OperationContract]
        ICollection<IntervalLevel> FetchIntervalLevels();

        [OperationContract]
        ICollection<IntervalType> FetchIntervalTypes();

        [OperationContract]
        ICollection<EffortType> FetchEffortTypes();
    }
}
