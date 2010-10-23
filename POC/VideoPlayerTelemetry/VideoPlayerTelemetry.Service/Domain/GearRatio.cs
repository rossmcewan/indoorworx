using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VideoPlayerTelemetry.Models
{
    [DataContract]
    public class GearRatio
    {
        [DataMember]
        public int BigRing { get; set; }

        [DataMember]
        public int SmallRing { get; set; }
    }
}
