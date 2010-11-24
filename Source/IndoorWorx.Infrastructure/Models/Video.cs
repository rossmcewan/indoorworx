﻿using System;
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

        private ICollection<TrainingSet> trainingSets = new List<TrainingSet>();
        [DataMember]
        public virtual ICollection<TrainingSet> TrainingSets
        {
            get { return trainingSets; }
            set
            {
                trainingSets = value;
                FirePropertyChanged("TrainingSets");
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
