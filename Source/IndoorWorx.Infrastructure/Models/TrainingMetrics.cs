using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class TrainingMetrics : BaseModel
    {
        private double averagePower;
        [DataMember]
        public virtual double AveragePower
        {
            get { return averagePower; }
            set
            {
                averagePower = value;
                FirePropertyChanged("AveragePower");
            }
        }

        private double normalizedPower;
        [DataMember]
        public virtual double NormalizedPower
        {
            get { return normalizedPower; }
            set
            {
                normalizedPower = value;
                FirePropertyChanged("NormalizedPower");
            }
        }

        private double intensityFactor;
        [DataMember]
        public virtual double IntensityFactor
        {
            get { return intensityFactor; }
            set
            {
                intensityFactor = value;
                FirePropertyChanged("IntensityFactor");
            }
        }

        private double variabilityIndex;
        [DataMember]
        public virtual double VariabilityIndex
        {
            get { return variabilityIndex; }
            set
            {
                variabilityIndex = value;
                FirePropertyChanged("VariabilityIndex");
            }
        }
    }
}
