using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class IntervalType : BaseModel
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

        private IntervalLevel defaultLevel;
        [DataMember]
        public virtual IntervalLevel DefaultLevel
        {
            get { return defaultLevel; }
            set
            {
                defaultLevel = value;
                FirePropertyChanged("DefaultLevel");
            }
        }
    }
}
