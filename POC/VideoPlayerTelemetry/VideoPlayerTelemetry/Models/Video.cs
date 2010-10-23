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

namespace VideoPlayerTelemetry.Models
{
    public class Video : BaseModel
    {

        private string title = "Super Speedway";
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string url = "C:\\Development\\POC\\VideoPlayerTelemetry\\VideoPlayerTelemetry\\Video\\SuperSpeedway.wmv";
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }
    }
}
