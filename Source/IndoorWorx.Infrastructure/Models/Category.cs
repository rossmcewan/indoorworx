using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class Category : BaseModel
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

        private ICollection<Catalog> catalogs = new List<Catalog>();
        [DataMember]
        public virtual ICollection<Catalog> Catalogs
        {
            get { return catalogs; }
            set
            {
                catalogs = value;
                FirePropertyChanged("Catalogs");
                FirePropertyChanged("Videos");
                FirePropertyChanged("HasVideos");
            }
        }

        public virtual ICollection<Video> Videos
        {
            get
            {
                var result = new List<Video>();
                foreach (var catalog in Catalogs)
                {
                    result.AddRange(catalog.Videos);
                }
                return result;
            }
        }

        public virtual bool HasVideos
        {
            get
            {
                return Catalogs.Any(x => x.Videos != null && x.Videos.Count > 0);
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

        private Uri catalogUri;
        [DataMember]
        public virtual Uri CatalogUri
        {
            get { return catalogUri; }
            set
            {
                catalogUri = value;
                FirePropertyChanged("CatalogUri");
            }
        }

        private Uri libraryUri;
        [DataMember]
        public virtual Uri LibraryUri
        {
            get { return libraryUri; }
            set
            {
                libraryUri = value;
                FirePropertyChanged("LibraryUri");
            }
        }
    }
}
