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
using Microsoft.Practices.Composite.Events;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Catalog
    {
        public event EventHandler SelectedVideoChanging;

        public event EventHandler SelectedVideoChanged;

        private Video selectedVideo;
        public virtual Video SelectedVideo
        {
            get { return selectedVideo; }
            set
            {
                OnSelectedVideoChanging();
                bool changed = value != selectedVideo;
                if (selectedVideo != null)
                    selectedVideo.IsSelected = false;
                selectedVideo = value;
                if (selectedVideo != null)
                {
                    selectedVideo.IsSelected = true;
                }
                if(changed)
                    OnSelectedVideoChanged();
                FirePropertyChanged("SelectedVideo");
            }
        }

        protected virtual void OnSelectedVideoChanged()
        {
            if (SelectedVideoChanged != null)
                SelectedVideoChanged(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedVideoChanging()
        {
            if (SelectedVideoChanging != null)
                SelectedVideoChanging(this, EventArgs.Empty);
        }
    }
}
