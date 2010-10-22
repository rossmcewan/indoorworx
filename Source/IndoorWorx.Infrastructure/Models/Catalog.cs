﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class Catalog : BaseModel
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

        private ICollection<Catalog> children = new List<Catalog>();
        public virtual ICollection<Catalog> Children
        {
            get { return children; }
            set
            {
                children = value;
                FirePropertyChanged("Children");
            }
        }

        private Catalog selectedCatalog;
        public virtual Catalog SelectedCatalog
        {
            get { return selectedCatalog; }
            set
            {
                selectedCatalog = value;
                FirePropertyChanged("SelectedCatalog");
            }
        }
    }
}