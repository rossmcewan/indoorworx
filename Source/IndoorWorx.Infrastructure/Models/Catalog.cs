using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Catalog : BaseModel
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

        private string image;
        public virtual string Image
        {
            get { return image; }
            set
            {
                image = value;
                FirePropertyChanged("Image");
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

        private ICollection<Video> videos = new List<Video>();
        public virtual ICollection<Video> Videos
        {
            get { return videos; }
            set
            {
                videos = value;
                FirePropertyChanged("Videos");
            }
        }        
    }
}
