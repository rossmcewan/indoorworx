using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VideoPlayerTelemetry.Models;

namespace VideoPlayerTelemetry.Service
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        ICollection<Telemetry> ProvideTelemetryData();
    }
}
