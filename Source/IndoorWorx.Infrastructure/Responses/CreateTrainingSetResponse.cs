using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Responses
{
    [DataContract]
    public class CreateTrainingSetResponse : ResponseBase
    {
        [DataMember]
        public Video TrainingSet { get; set; }

        [DataMember]
        public CreateTrainingSetStatus Status { get; set; }
    }
}
