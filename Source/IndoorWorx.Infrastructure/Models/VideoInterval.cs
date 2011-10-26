using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public class VideoInterval : BaseModel
    {
        public VideoInterval() { }

        public VideoInterval(TimeSpan duration, int? effort, int sequence)
        {
            this.duration = duration;
            this.effort = effort;
            this.sequence = sequence;
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

        private int? effort;
        [DataMember]
        public virtual int? Effort
        {
            get { return effort; }
            set
            {
                effort = value;
                FirePropertyChanged("Effort");
            }
        }

        private int sequence;
        [DataMember]
        public virtual int Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                FirePropertyChanged("Sequence");
            }
        }
    }
}
