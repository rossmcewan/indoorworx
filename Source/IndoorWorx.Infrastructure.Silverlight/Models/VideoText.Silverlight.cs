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
    public partial class VideoText
    {
        public virtual bool ValuesEquals(VideoText compareTo)
        {
            return this.Animation == compareTo.Animation &&
                this.Duration == compareTo.Duration &&
                this.MainText == compareTo.MainText &&
                this.SubText == compareTo.SubText &&
                this.StartTime == compareTo.StartTime;
        }

        public virtual bool IsShown { get; set; }
    }
}
