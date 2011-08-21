using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Requests
{
    [DataContract]
    public class CreateTrainingSetRequest
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public TrainingSet TrainingSet { get; set; }
    }
}
