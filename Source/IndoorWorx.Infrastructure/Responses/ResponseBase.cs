using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Responses
{
    [DataContract]
    public class ResponseBase
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int Credits { get; set; }
    }
}
