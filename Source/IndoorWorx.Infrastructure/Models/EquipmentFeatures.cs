using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class EquipmentFeatures : BaseModel
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

        private ICollection<Equipment> equipment = new List<Equipment>();
        [DataMember]
        public virtual ICollection<Equipment> Equipment
        {
            get { return equipment; }
            set
            {
                equipment = value;
                FirePropertyChanged("Equipment");
            }
        }
    }
}
