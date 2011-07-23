using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Requests
{
    [DataContract]
    public class PlayVideoRequest
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public Guid VideoId { get; set; }
    }
}
