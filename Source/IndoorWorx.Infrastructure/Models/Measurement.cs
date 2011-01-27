using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class Measurement : BaseModel
    {
        private string name = string.Empty;
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

        private double val;
        [DataMember]
        public virtual double Value
        {
            get { return val; }
            set
            {
                val = value;
                FirePropertyChanged("Value");
            }
        }

        private UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
        [DataMember]
        public virtual UnitOfMeasure UnitOfMeasure
        {
            get { return unitOfMeasure; }
            set
            {
                unitOfMeasure = value;
                FirePropertyChanged("UnitOfMeasure");
            }
        }

        private ICollection<TrainingZone> trainingZones = new List<TrainingZone>();
        [DataMember]
        public virtual ICollection<TrainingZone> TrainingZones
        {
            get { return trainingZones; }
            set
            {
                trainingZones = value;
                FirePropertyChanged("TrainingZones");
            }
        }
    }
}
