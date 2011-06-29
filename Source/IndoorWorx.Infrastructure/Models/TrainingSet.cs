using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class TrainingSet : Video
    {
        public static readonly double DefaultRecordingInterval = 2;

        private double recordingInterval = DefaultRecordingInterval;
        [DataMember]
        public virtual double RecordingInterval
        {
            get { return recordingInterval; }
            set
            {
                recordingInterval = value;
                FirePropertyChanged("RecordingInterval");
            }
        }        

        private TrainingMetricType trainingMetrics;
        [DataMember]
        public virtual TrainingMetricType TrainingMetrics
        {
            get { return trainingMetrics; }
            set
            {
                trainingMetrics = value;
                FirePropertyChanged("TrainingMetrics");
            }
        }

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

        private ICollection<VideoText> videoText = new List<VideoText>();
        [DataMember]
        public virtual ICollection<VideoText> VideoText
        {
            get { return videoText; }
            set
            {
                videoText = value;
                FirePropertyChanged("VideoText");
            }
        }

    }
}
