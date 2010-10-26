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

        private TrainingSet selectedTrainingSet;
        public virtual TrainingSet SelectedTrainingSet
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
