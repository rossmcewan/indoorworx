using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class IndoorTrainingFrequency : BaseModel
    {
        private string description = string.Empty;
        [DataMember]
        public virtual string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged("Description");
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
