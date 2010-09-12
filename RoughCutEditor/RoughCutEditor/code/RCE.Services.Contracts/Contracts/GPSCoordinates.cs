using System;
using System.Net;
using System.Runtime.Serialization;

namespace RCE.Services.Contracts
{
    [DataContract]
    public class GPSCoordinates
    {
        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }
    }
}
