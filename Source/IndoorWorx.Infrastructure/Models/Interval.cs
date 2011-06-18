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
        private TimeSpan start;
        [DataMember]
        public virtual TimeSpan StartTime
        {
            get { return start; }
            set
            {
                start = value;
                FirePropertyChanged("Start");
            }
        }

        private TimeSpan end;
        [DataMember]
        public virtual TimeSpan EndTime
        {
            get { return end; }
            set
            {
                end = value;
                FirePropertyChanged("End");
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
    }
}
