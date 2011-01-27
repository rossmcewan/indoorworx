using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class Range : BaseModel
    {
        private double upperValue;
        [DataMember]
        public virtual double UpperValue
        {
            get { return upperValue;}
            set
            {
                upperValue = value;
                FirePropertyChanged("UpperValue");
            }
        }

        private double lowerValue;
        [DataMember]
        public virtual double LowerValue
        {
            get { return lowerValue; }
            set
            {
                lowerValue = value;
                FirePropertyChanged("LowerValue");
            }
        }
    }
}
