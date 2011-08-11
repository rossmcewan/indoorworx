using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class TrainingMetricType : BaseModel
    {
        private string title;
        [DataMember]
        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                FirePropertyChanged("Title");
            }
        }

        private string description;
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

        private string calculator;
        [DataMember]
        public virtual string Calculator
        {
            get { return calculator; }
            set
            {
                calculator = value;
                FirePropertyChanged("Calculator");
            }
        }

        private bool active;
        [DataMember]
        public virtual bool IsActive
        {
            get { return active; }
            set
            {
                active = value;
                FirePropertyChanged("IsActive");
            }
        }        
    }
}
