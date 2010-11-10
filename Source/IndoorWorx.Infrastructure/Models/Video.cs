using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Video : AuditableModel
    {
        public virtual Guid Id { get; set; }

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

        private Uri streamUri;
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
        public virtual string TelemetryData
        {
            get { return telemetryData; }
            set
            {
                telemetryData = value;
                FirePropertyChanged("TelemetryData");
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
        public virtual ICollection<Video> TrainingSets
        {
            get { return trainingSets; }
            set
            {
                trainingSets = value;
                FirePropertyChanged("TrainingSets");
            }
        }
    }
}
