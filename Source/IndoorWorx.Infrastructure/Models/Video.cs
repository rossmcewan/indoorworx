using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private ICollection<Telemetry> telemetry = new List<Telemetry>();
        public virtual ICollection<Telemetry> Telemetry
        {
            get { return telemetry; }
            set
            {
                telemetry = value;
                FirePropertyChanged("Telemetry");
            }
        }

        private ICollection<TrainingSet> trainingSets = new List<TrainingSet>();
        public virtual ICollection<TrainingSet> TrainingSets
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
