using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class UnitOfMeasure : BaseModel
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
    }
}
