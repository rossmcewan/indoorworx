using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class Widget : BaseModel
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

        private string contentRenderer;
        public virtual string ContentRenderer
        {
            get { return contentRenderer; }
            set
            {
                contentRenderer = value;
                FirePropertyChanged("ContentRenderer");
            }
        }
    }
}
