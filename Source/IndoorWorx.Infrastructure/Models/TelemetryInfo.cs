using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class TelemetryInfo : BaseModel
    {
        private Uri telemetryUri;
        [DataMember]
        public virtual Uri TelemetryUri
        {
            get { return telemetryUri; }
            set
            {
                telemetryUri = value;
                FirePropertyChanged("TelemetryUri");
            }
        }

        private int recordingInterval = 2;
        [DataMember]
        public virtual int RecordingInterval
        {
            get { return recordingInterval; }
            set
            {
                recordingInterval = value;
                FirePropertyChanged("RecordingInterval");
            }
        }
    }
}
