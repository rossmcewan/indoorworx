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
    }
}
