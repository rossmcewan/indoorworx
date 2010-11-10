using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public abstract class AuditableModel : BaseModel
    {
        private DateTime? created;
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
