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
