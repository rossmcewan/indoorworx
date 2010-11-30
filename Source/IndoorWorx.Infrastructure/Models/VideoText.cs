using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Enums;

namespace IndoorWorx.Infrastructure.Models
{
    public class VideoText : BaseModel
    {
        private string mainText = string.Empty;
        public string MainText
        {
            get { return mainText; }
            set
            {
                mainText = value;
                FirePropertyChanged("MainText");
            }
        }

        private VideoTextAnimations animation = VideoTextAnimations.FadeCenter;
        public VideoTextAnimations Animation
        {
            get { return animation; }
            set
            {
                animation = value;
                FirePropertyChanged("Animation");
            }
        }

        private string subText = string.Empty;
        public string SubText
        {
            get { return subText; }
            set
            {
                subText = value;
                FirePropertyChanged("SubText");
            }
        }

        public TimeSpan StartTime
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }


        internal void Clear()
        {
            this.MainText = string.Empty;
            this.SubText = string.Empty;
        }
    }
}
