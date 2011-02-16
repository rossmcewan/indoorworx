using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class Equipment : BaseModel
    {
        private ICollection<ActivityType> activityTypes = new List<ActivityType>();
        [DataMember]
        public virtual ICollection<ActivityType> ActivityTypes
        {
            get { return activityTypes; }
            set
            {
                activityTypes = value;
                FirePropertyChanged("ActivityTypes");
            }
        }

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

        private bool isActive = true;
        [DataMember]
        public virtual bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                FirePropertyChanged("IsActive");
            }
        }

        private Manufacturer manufacturer = new Manufacturer();
        [DataMember]
        public virtual Manufacturer Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                FirePropertyChanged("Manufacturer");
            }
        }

        private ICollection<EquipmentFeatures> equipmentFeatures = new List<EquipmentFeatures>();
        [DataMember]
        public virtual ICollection<EquipmentFeatures> EquipmentFeatures
        {
            get { return equipmentFeatures; }
            set
            {
                equipmentFeatures = value;
                FirePropertyChanged("EquipmentFeatures");
            }
        }
    }
}
