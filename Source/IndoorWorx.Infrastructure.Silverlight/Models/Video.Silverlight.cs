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

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Video
    {
        private bool selected = false;
        public virtual bool IsSelected
        {
            get { return selected; }
            set
            {                
                selected = value;
                FirePropertyChanged("IsSelected");
            }
        }

        private bool playing;
        public virtual bool IsPlaying
        {
            get { return playing; }
            set
            {
                playing = value;
                FirePropertyChanged("IsPlaying");
            }
        }

        private Video selectedTrainingSet;
        public virtual Video SelectedTrainingSet
        {
            get { return selectedTrainingSet; }
            set
            {
                selectedTrainingSet = value;
                FirePropertyChanged("SelectedTrainingSet");
            }
        }
    }
}
