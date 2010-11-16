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

        private string telemetryData;
        [DataMember]
        public virtual string TelemetryData
        {
            get { return telemetryData; }
            set
            {
                telemetryData = value;
                FirePropertyChanged("TelemetryData");
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

        public virtual ICollection<Telemetry> Telemetry
        {
            get
            {
                List<Telemetry> telemetry = new List<Telemetry>();
                using (var reader = new StringReader(TelemetryData))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            telemetry.Add(Models.Telemetry.Parse(line));
                        }
                        catch
                        {
                            continue;
                        }
                    }                    
                }
                return telemetry;
            }
        }

        private ICollection<Video> trainingSets = new List<Video>();
        [DataMember]
        public virtual ICollection<Video> TrainingSets
        {
            get { return trainingSets; }
            set
            {
                trainingSets = value;
                FirePropertyChanged("TrainingSets");
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

        public virtual int AverageRating
        {
            get
            {
                if(Reviews.Any())
                    return Convert.ToInt32(Reviews.Average(x => x.Rating));
                return 0;
            }
        }
    }
}
