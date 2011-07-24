using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Requests
{
    [DataContract(IsReference = true)]
    public class RemoveTemplateRequest
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public TrainingSetTemplate Template { get; set; }
    }
}
