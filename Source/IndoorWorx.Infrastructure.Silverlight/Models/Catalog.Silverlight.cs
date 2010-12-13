using System;
using System.Linq;
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
    public partial class Catalog
    {
        private Video selectedVideo;
        public virtual Video SelectedVideo
        {
            get { return selectedVideo; }
            set
            {
                if (selectedVideo != null)
                    selectedVideo.IsSelected = false;
                selectedVideo = value;
                if (selectedVideo != null)
                {
                    selectedVideo.IsSelected = true;
                    //selectedVideo.SelectedTrainingSet = selectedVideo.TrainingSets.FirstOrDefault();
                }
                FirePropertyChanged("SelectedVideo");
            }
        }
    }
}
