using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Video
    {        
        private bool selected;
        public virtual bool IsSelected
        {
            get { return selected; }
            set
            {                
                selected = value;
                FirePropertyChanged("IsSelected");
            }
        }

        private bool playing = false;
        public virtual bool IsPlaying
        {
            get { return playing; }
            set
            {
                playing = value;
               
                FirePropertyChanged("IsPlaying");
            }
        }

        private bool mediaLoading;
        public virtual bool IsMediaLoading
        {
            get { return mediaLoading; }
            set
            {
                mediaLoading = value;
                FirePropertyChanged("IsMediaLoading");
            }
        }

        private TrainingSet selectedTrainingSet;
        public virtual TrainingSet SelectedTrainingSet
        {
            get { return selectedTrainingSet; }
            set
            {
                selectedTrainingSet = value;
                selectedTrainingSet.LoadTelemetry();
                FirePropertyChanged("SelectedTrainingSet");
            }
        }
        
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            this.IsMediaLoading = true;
        }
    }
}
