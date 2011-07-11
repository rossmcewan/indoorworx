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
    public partial class Interval
    {
        private double oaDateTime;
        public virtual double OADateTime
        {
            get { return oaDateTime; }
            set
            {
                oaDateTime = value;
                FirePropertyChanged("OADateTime");
            }
        }

        public int AverageEffort
        {
            get 
            { 
                var sum = this.effortFrom.GetValueOrDefault() + this.effortTo.GetValueOrDefault();
                var ave = sum / 2;
                return ave; 
            }
        }

        private int repeats;
        public virtual int Repeats
        {
            get { return repeats; }
            set
            {
                repeats = value;
                FirePropertyChanged("Repeats");
            }
        }

        private int recoveryIntervalSeconds;
        public virtual int RecoveryIntervalSeconds
        {
            get { return recoveryIntervalSeconds; }
            set
            {
                recoveryIntervalSeconds = value;
                FirePropertyChanged("RecoveryIntervalSeconds");
            }
        }

        private int recoveryIntervalMinutes;
        public virtual int RecoveryIntervalMinutes
        {
            get { return recoveryIntervalMinutes; }
            set
            {
                recoveryIntervalMinutes = value;
                FirePropertyChanged("RecoveryIntervalMinutes");
            }
        }

        private int durationMinutes;
        public virtual int DurationMinutes
        {
            get { return durationMinutes; }
            set
            {
                durationMinutes = value;
                FirePropertyChanged("DurationMinutes");
            }
        }

        private int durationSeconds;
        public virtual int DurationSeconds
        {
            get { return durationSeconds; }
            set
            {
                durationSeconds = value;
                FirePropertyChanged("DurationSeconds");
            }
        }

        private bool countDownToStartOfInterval;
        public virtual bool CountDownToStartOfInterval
        {
            get { return countDownToStartOfInterval; }
            set
            {
                countDownToStartOfInterval = value;
                FirePropertyChanged("CountDownToStartOfInterval");
            }
        }

        private int countDownToStartOfIntervalMinutes;
        public virtual int CountDownToStartOfIntervalMinutes
        {
            get { return countDownToStartOfIntervalMinutes; }
            set
            {
                countDownToStartOfIntervalMinutes = value;
                FirePropertyChanged("CountDownToStartOfIntervalMinutes");
            }
        }

        private int countDownToStartOfIntervalSeconds;
        public virtual int CountDownToStartOfIntervalSeconds
        {
            get { return countDownToStartOfIntervalSeconds; }
            set
            {
                countDownToStartOfIntervalSeconds = value;
                FirePropertyChanged("CountDownToStartOfIntervalSeconds");
            }
        }

        private string startIntervalMessage;
        public virtual string StartIntervalMessage
        {
            get { return startIntervalMessage; }
            set
            {
                startIntervalMessage = value;
                FirePropertyChanged("StartIntervalMessage");
            }
        }

        private bool countDownToEndOfInterval;
        public virtual bool CountDownToEndOfInterval
        {
            get { return countDownToEndOfInterval; }
            set
            {
                countDownToEndOfInterval = value;
                FirePropertyChanged("CountDownToEndOfInterval");
            }
        }

        private int countDownToEndOfIntervalMinutes;
        public virtual int CountDownToEndOfIntervalMinutes
        {
            get { return countDownToEndOfIntervalMinutes; }
            set
            {
                countDownToEndOfIntervalMinutes = value;
                FirePropertyChanged("CountDownToEndOfIntervalMinutes");
            }
        }

        private int countDownToEndOfIntervalSeconds;
        public virtual int CountDownToEndOfIntervalSeconds
        {
            get { return countDownToEndOfIntervalSeconds; }
            set
            {
                countDownToEndOfIntervalSeconds = value;
                FirePropertyChanged("CountDownToEndOfIntervalSeconds");
            }
        }

        private string endIntervalMessage;
        public virtual string EndIntervalMessage
        {
            get { return endIntervalMessage; }
            set
            {
                endIntervalMessage = value;
                FirePropertyChanged("EndIntervalMessage");
            }
        }
    }
}
