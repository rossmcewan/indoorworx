using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public abstract class AuditableModel : BaseModel
    {
        private DateTime? created;
        [DataMember]
        public virtual DateTime? Created
        {
            get { return created; }
            set
            {
                created = value;
                FirePropertyChanged("Created");
            }
        }

        private string createdBy;
        [DataMember]
        public virtual string CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                FirePropertyChanged("CreatedBy");
            }
        }

        private DateTime? modified;
        [DataMember]
        public virtual DateTime? Modified
        {
            get { return modified; }
            set
            {
                modified = value;
                FirePropertyChanged("Modified");
            }
        }

        private string modifiedBy;
        [DataMember]
        public virtual string ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                FirePropertyChanged("ModifiedBy");
            }
        }
    }
}
