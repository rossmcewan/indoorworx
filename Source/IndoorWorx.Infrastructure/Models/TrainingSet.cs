﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class TrainingSet : Video
    {
        public static readonly double DefaultRecordingInterval = 3;

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

        private TrainingMetrics trainingMetrics;
        [DataMember]
        public virtual TrainingMetrics TrainingMetrics
        {
            get { return trainingMetrics; }
            set
            {
                trainingMetrics = value;
                FirePropertyChanged("TrainingMetrics");
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
