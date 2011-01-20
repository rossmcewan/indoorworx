using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Enums;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class VideoText : BaseModel
    {
        private string mainText = string.Empty;
        [DataMember]
        public virtual string MainText
        {
            get { return mainText; }
            set
            {
                mainText = value;
                FirePropertyChanged("MainText");
            }
        }

        private VideoTextAnimations animation = VideoTextAnimations.FadeCenter;
        [DataMember]
        public virtual VideoTextAnimations Animation
        {
            get { return animation; }
            set
            {
                animation = value;
                FirePropertyChanged("Animation");
            }
        }

        private string subText = string.Empty;
        [DataMember]
        public virtual string SubText
        {
            get { return subText; }
            set
            {
                subText = value;
                FirePropertyChanged("SubText");
            }
        }

        private TimeSpan startTime;
        [DataMember]
        public virtual TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                FirePropertyChanged("StartTime");
            }
        }

        private TrainingSet trainingSet;
        [DataMember]
        public virtual TrainingSet TrainingSet
        {
            get { return trainingSet; }
            set
            {
                trainingSet = value;
                FirePropertyChanged("TrainingSet");
            }
        }

        private TimeSpan duration;
        [DataMember]
        public virtual TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                FirePropertyChanged("Duration");
            }
        }

      
        public virtual void Clear()
        {
            this.MainText = string.Empty;
            this.SubText = string.Empty;
        }
    }
}
