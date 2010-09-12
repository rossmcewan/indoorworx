using System;
using System.Net;
using System.Runtime.Serialization;

namespace RCE.Services.Contracts
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
