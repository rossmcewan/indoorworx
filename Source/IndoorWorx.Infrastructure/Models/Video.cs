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

        private ICollection<TrainingMetric> trainingMetrics = new List<TrainingMetric>();
        [DataMember]
        public virtual ICollection<TrainingMetric> TrainingMetrics
        {
            get { return trainingMetrics; }
            set
            {
                trainingMetrics = value;
                FirePropertyChanged("TrainingMetrics");
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

        private ICollection<Interval> intervals = new List<Interval>();
        [DataMember]
        public virtual ICollection<Interval> Intervals
        {
            get { return intervals; }
            set
            {
                intervals = value;
                FirePropertyChanged("Intervals");
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
