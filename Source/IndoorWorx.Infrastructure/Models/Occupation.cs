using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class Occupation : BaseModel
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
    }
}
