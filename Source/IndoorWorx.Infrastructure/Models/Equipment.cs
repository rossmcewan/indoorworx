using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class Equipment : BaseModel
    {
        private ICollection<ActivityType> activityTypes = new List<ActivityType>();
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
        public virtual string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged("Name");
            }
        }

        private Manufacturer manufacturer = new Manufacturer();
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
