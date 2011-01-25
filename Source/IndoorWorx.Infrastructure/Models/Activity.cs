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
        private ActivityType activityType = new ActivityType();
        [DataMember]
        public virtual ActivityType ActivityType
        {
            get { return activityType; }
            set
            {
                activityType = value;
                FirePropertyChanged("ActivityType");
            }
        }

        private Equipment equipment = new Equipment();
        [DataMember]
        public virtual Equipment Equipment
        {
            get { return equipment; }
            set
            {
                equipment = value;
                FirePropertyChanged("Equipment");
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
