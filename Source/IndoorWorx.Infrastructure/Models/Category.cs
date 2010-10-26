using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Category : BaseModel
    {
        private string title;
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
        public virtual string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged("Description");
            }
        }

        private ICollection<Catalog> catalogs = new List<Catalog>();
        public virtual ICollection<Catalog> Catalogs
        {
            get { return catalogs; }
            set
            {
                catalogs = value;
                FirePropertyChanged("Catalogs");
            }
        }        
    }
}
