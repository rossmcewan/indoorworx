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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public class IntervalDesignerPresentationModel : BaseModel, IIntervalDesignerPresentationModel
    {
        public IIntervalDesignerView View { get; set; }

        private Interval interval;
        public virtual Interval Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                FirePropertyChanged("Interval");
            }
        }

        private bool useSingleVideo = false;
        public virtual bool UseSingleVideo
        {
            get { return useSingleVideo; }
            set
            {
                useSingleVideo = value;
                useMultipleVideos = !useSingleVideo;
                FirePropertyChanged("UseSingleVideo");
                FirePropertyChanged("UseMultipleVideos");
            }
        }

        private bool useMultipleVideos = true;
        public virtual bool UseMultipleVideos
        {
            get { return useMultipleVideos; }
            set
            {
                useMultipleVideos = value;
                useSingleVideo = !useMultipleVideos;
                FirePropertyChanged("UseMultipleVideos");
                FirePropertyChanged("UseSingleVideo");
            }
        }
    }
}
