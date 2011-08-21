using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class VideoPart
    {
        [DataMember]
        public Guid VideoId { get; set; }

        [DataMember]
        public TimeSpan From { get; set; }

        [DataMember]
        public TimeSpan To { get; set; }
    }
}
