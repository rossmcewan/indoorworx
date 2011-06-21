using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Responses
{
    [DataContract]
    public class AddVideoResponse
    {
        [DataMember]
        public AddVideoStatus AddVideoStatus { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
