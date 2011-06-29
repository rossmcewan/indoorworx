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
        private string name;
        [DataMember]
        public virtual string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged("Name");
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

        private decimal? minimumPercentageOfFtp;
        [DataMember]
        public virtual decimal? MinimumPercentageOfFtp
        {
            get { return minimumPercentageOfFtp; }
            set
            {
                minimumPercentageOfFtp = value;
                FirePropertyChanged("MinimumPercentageOfFtp");
            }
        }

        private decimal? maximumPercentageOfFtp;
        [DataMember]
        public virtual decimal? MaximumPercentageOfFtp
        {
            get { return maximumPercentageOfFtp; }
            set
            {
                maximumPercentageOfFtp = value;
                FirePropertyChanged("MaximumPercentageOfFtp");
            }
        }

        private decimal? minimumPercentageOfFthr;
        [DataMember]
        public virtual decimal? MinimumPercentageOfFthr
        {
            get { return minimumPercentageOfFthr; }
            set
            {
                minimumPercentageOfFthr = value;
                FirePropertyChanged("MinimumPercentageOfFthr");
            }
        }

        private decimal? maximumPercentageOfFthr;
        [DataMember]
        public virtual decimal? MaximumPercentageOfFthr
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
    }
}
