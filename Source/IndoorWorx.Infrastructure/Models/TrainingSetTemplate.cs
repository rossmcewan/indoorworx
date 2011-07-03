using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class TrainingSetTemplate : BaseModel
    {
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

        private ICollection<Interval> intervals = new List<Interval>();
        [DataMember]
        public virtual ICollection<Interval> Intervals
        {
            get { return intervals; }
            set
            {
                intervals = value;
                FirePropertyChanged("Intervals");
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

        private int credits;
        [DataMember]
        public virtual int Credits
        {
            get { return credits; }
            set
            {
                credits = value;
                FirePropertyChanged("Credits");
            }
        }
    }
}
