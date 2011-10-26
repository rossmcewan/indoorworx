using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class Video : AuditableModel
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

        private Uri streamUri;
        [DataMember]
        public virtual Uri StreamUri
        {
            get { return streamUri; }
            set
            {
                streamUri = value;
                FirePropertyChanged("StreamUri");
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

        private TimeSpan duration;
        [DataMember]
        public virtual TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                FirePropertyChanged("Duration");
            }
        }

        private ICollection<VideoReview> reviews = new List<VideoReview>();
        [DataMember]
        public virtual ICollection<VideoReview> Reviews
        {
            get { return reviews; }
            set
            {
                reviews = value;
                FirePropertyChanged("Reviews");
            }
        }               

        private VideoMetadata videoMetadata = new VideoMetadata();
        [DataMember]
        public virtual VideoMetadata VideoMetadata
        {
            get { return videoMetadata; }
            set
            {
                videoMetadata = value;
                FirePropertyChanged("VideoMetadata");
            }
        }

        private int credits;
        [DataMember]
        public virtual int Credits
        {
            get { return credits; }
            set
            {
                credits = value;
                FirePropertyChanged("Credits");
            }
        }

        private int rideCredits = 1;
        [DataMember]
        public virtual int RideCredits
        {
            get { return rideCredits; }
            set
            {
                rideCredits = value;
                FirePropertyChanged("RideCredits");
            }
        }

        private TelemetryInfo telemetryInfo = new TelemetryInfo();
        [DataMember]
        public virtual TelemetryInfo TelemetryInfo
        {
            get { return telemetryInfo; }
            set
            {
                telemetryInfo = value;
                FirePropertyChanged("TelemetryInfo");
            }
        }

        private ICollection<VideoText> videoText = new List<VideoText>();
        [DataMember]
        public virtual ICollection<VideoText> VideoText
        {
            get { return videoText; }
            set
            {
                videoText = value;
                FirePropertyChanged("VideoText");
            }
        }

        private Catalog catalog;
        [DataMember]
        public virtual Catalog Catalog
        {
            get { return catalog; }
            set
            {
                catalog = value;
                FirePropertyChanged("Catalog");
            }
        }

        private ICollection<VideoInterval> intervals;
        [DataMember]
        public virtual ICollection<VideoInterval> Intervals
        {
            get { return this.intervals; }
            set
            {
                this.intervals = value;
                FirePropertyChanged("Intervals");
            }
        }

        public virtual int AverageRating
        {
            get
            {
                if (Reviews.Any())
                    return Convert.ToInt32(Reviews.Average(x => x.Rating));
                return 0;
            }
        }
    }
}
