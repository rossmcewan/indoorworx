using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class TrainingMetric : BaseModel
    {
        private TrainingMetricType type;
        [DataMember]
        public virtual TrainingMetricType Type
        {
            get { return type; }
            set
            {
                type = value;
                FirePropertyChanged("Type");
            }
        }

        private string value;
        [DataMember]
        public virtual string Value
        {
            get { return value; }
            set
            {
                value = value;
                FirePropertyChanged("Value");
            }
        }
    }
}
