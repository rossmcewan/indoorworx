using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Requests
{
    [DataContract]
    public class SaveTemplateRequest
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public TrainingSetTemplate Template { get; set; }
    }
}
