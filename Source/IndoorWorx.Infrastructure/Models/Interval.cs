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

        private string title;
        [DataMember]
        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                FirePropertyChanged("Title");
            }
        }

        private string description;
        [DataMember]
        public virtual string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged("Description");
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

        private int? effortFrom;
        [DataMember]
        public virtual int? EffortFrom
        {
            get { return effortFrom; }
            set
            {
                effortFrom = value;
                FirePropertyChanged("EffortFrom");
            }
        }

        private int? effortTo;
        [DataMember]
        public virtual int? EffortTo
        {
            get { return effortTo; }
            set
            {
                effortTo = value;
                FirePropertyChanged("EffortTo");
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
