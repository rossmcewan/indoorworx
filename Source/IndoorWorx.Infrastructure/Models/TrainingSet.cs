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

        private ICollection<TrainingSetText> videoText = new List<TrainingSetText>();
        [DataMember]
        public virtual ICollection<TrainingSetText> VideoText
        {
            get { return videoText; }
            set
            {
                videoText = value;
                FirePropertyChanged("VideoText");
            }
        }

        private bool @public;
        [DataMember]
        public virtual bool IsPublic
        {
            get { return @public; }
            set
            {
                @public = value;
                FirePropertyChanged("IsPublic");
            }
        }

        private ICollection<TrainingSetInterval> intervals = new List<TrainingSetInterval>();
        [DataMember]
        public virtual ICollection<TrainingSetInterval> Intervals
        {
            get { return intervals; }
            set
            {
                intervals = value;
                FirePropertyChanged("Intervals");
            }
        }

        private EffortType effortType;
        [DataMember]
        public virtual EffortType EffortType
        {
            get { return effortType; }
            set
            {
                effortType = value;
                FirePropertyChanged("EffortType");
            }
        }
    }
}
