using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public class IntervalLevel : BaseModel
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

        private int? minimumPercentageOfFtp;
        [DataMember]
        public virtual int? MinimumPercentageOfFtp
        {
            get { return minimumPercentageOfFtp; }
            set
            {
                minimumPercentageOfFtp = value;
                FirePropertyChanged("MinimumPercentageOfFtp");
            }
        }

        private int? maximumPercentageOfFtp;
        [DataMember]
        public virtual int? MaximumPercentageOfFtp
        {
            get { return maximumPercentageOfFtp; }
            set
            {
                maximumPercentageOfFtp = value;
                FirePropertyChanged("MaximumPercentageOfFtp");
            }
        }

        private int? minimumPercentageOfFthr;
        [DataMember]
        public virtual int? MinimumPercentageOfFthr
        {
            get { return minimumPercentageOfFthr; }
            set
            {
                minimumPercentageOfFthr = value;
                FirePropertyChanged("MinimumPercentageOfFthr");
            }
        }

        private int? maximumPercentageOfFthr;
        [DataMember]
        public virtual int? MaximumPercentageOfFthr
        {
            get { return maximumPercentageOfFthr; }
            set
            {
                maximumPercentageOfFthr = value;
                FirePropertyChanged("MaximumPercentageOfFthr");
            }
        }

        private int? minRpe;
        [DataMember]
        public virtual int? MinRPE
        {
            get { return minRpe; }
            set
            {
                minRpe = value;
                FirePropertyChanged("MinRPE");
            }
        }

        private int? maxRpe;
        [DataMember]
        public virtual int? MaxRPE
        {
            get { return maxRpe; }
            set
            {
                maxRpe = value;
                FirePropertyChanged("MaxRPE");
            }
        }

        private TimeSpan? typicalMinDuration;
        [DataMember]
        public virtual TimeSpan? TypicalMinDuration
        {
            get { return typicalMinDuration; }
            set
            {
                typicalMinDuration = value;
                FirePropertyChanged("TypicalMinDuration");
            }
        }

        private TimeSpan? typicalMaxDuration;
        [DataMember]
        public virtual TimeSpan? TypicalMaxDuration
        {
            get { return typicalMaxDuration; }
            set
            {
                typicalMaxDuration = value;
                FirePropertyChanged("TypicalMaxDuration");
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
