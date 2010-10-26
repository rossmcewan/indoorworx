using System;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class TrainingSet : BaseModel
    {
        private string name;
        public virtual string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged("Name");
            }
        }

        private string description;
        public virtual string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged("Description");
            }
        }
    }
}
