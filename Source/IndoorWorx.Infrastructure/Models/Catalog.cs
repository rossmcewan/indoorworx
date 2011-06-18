using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class Catalog : BaseModel
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

        private Uri imageUri;
        [DataMember]
        public virtual Uri ImageUri
        {
            get { return imageUri; }
            set
            {
                imageUri = value;
                FirePropertyChanged("ImageUri");
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

        private ICollection<Video> videos = new List<Video>();
        [DataMember]
        public virtual ICollection<Video> Videos
        {
            get { return videos; }
            set
            {
                videos = value;
                FirePropertyChanged("Videos");
                FirePropertyChanged("HasVideos");
            }
        }

        private int sequence;
        [DataMember]
        public virtual int Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                FirePropertyChanged("Sequence");
            }
        }

        public virtual bool HasVideos
        {
            get
            {
                return Videos.Count > 0;
            }
        }
    }
}
