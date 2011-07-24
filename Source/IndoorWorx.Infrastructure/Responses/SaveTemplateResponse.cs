using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Responses
{
    [DataContract]
    public class SaveTemplateResponse : ResponseBase
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public TrainingSetTemplate SavedTemplate { get; set; }

        [DataMember]
        public SaveTemplateStatus Status { get; set; }
    }
}
