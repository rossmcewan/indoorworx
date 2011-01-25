using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class ActivityType : BaseModel
    {
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

        private ICollection<Equipment> equipment = new List<Equipment>();
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
