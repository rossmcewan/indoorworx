using System;
using System.Collections.Generic;
using System.Linq;
namespace VideoPlayerTelemetry.Models
{
    public class Information : BaseModel
    {
        private string text = string.Empty;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public TimeSpan StartTime
        {
            get;
            set;
        }

        public TimeSpan EndTime
        {
            get;
            set;
        }

        internal void Clear()
        {
            this.Text = string.Empty;
        }
    }
}