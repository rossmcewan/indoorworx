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

        private TimeSpan duration =  TimeSpan.Zero;
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
