using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class Interval : BaseModel
    {
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

        private IntervalType intervalType;
        [DataMember]
        public virtual IntervalType IntervalType
        {
            get { return intervalType; }
            set
            {
                intervalType = value;
                FirePropertyChanged("IntervalType");
            }
        }

        private IntervalLevel intervalLevel;
        [DataMember]
        public virtual IntervalLevel IntervalLevel
        {
            get { return intervalLevel; }
            set
            {
                intervalLevel = value;
                FirePropertyChanged("IntervalLevel");
            }
        }
    }
}
