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
using Microsoft.Practices.Composite.Events;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Video
    {
        public event EventHandler SelectedTrainingSetChanging;
        public event EventHandler SelectedTrainingSetChanged;

        public Video()
        {
            Initialize();
        }

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
                OnSelectedTrainingSetChanging();
                bool changed = value != selectedTrainingSet;
                selectedTrainingSet = value;
                if(selectedTrainingSet != null)
                    selectedTrainingSet.LoadTelemetry();
                if(changed)
                    OnSelectedTrainingSetChanged();
                FirePropertyChanged("SelectedTrainingSet");
            }
        }
        
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.IsMediaLoading = true;
        }

        protected virtual void OnSelectedTrainingSetChanging()
        {
            if (SelectedTrainingSetChanging != null)
                SelectedTrainingSetChanging(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedTrainingSetChanged()
        {
            if (SelectedTrainingSetChanged != null)
                SelectedTrainingSetChanged(this, EventArgs.Empty);
        }
    }
}
