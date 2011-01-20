using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class Activity : BaseModel
    {
        private ActivityTypes activityType;
        [DataMember]
        public virtual ActivityTypes ActivityType
        {
            get { return activityType; }
            set
            {
                activityType = value;
                FirePropertyChanged("ActivityType");
            }
        }


        private ICollection<Measurement> measurements = new List<Measurement>();
        [DataMember]
        public virtual ICollection<Measurement> Measurements
        {
            get { return measurements; }
            set
            {
                measurements = value;
                FirePropertyChanged("Measurement");
            }
        }


    }
}
