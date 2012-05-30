using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class VideoHistoryItem : BaseModel
    {
        private string username;
        [DataMember]
        public virtual string Username
        {
            get { return username; }
            set
            {
                username = value;
                FirePropertyChanged("Username");
            }
        }

        private Video video;
        [DataMember]
        public virtual Video Video
        {
            get { return video; }
            set
            {
                video = value;
                FirePropertyChanged("Video");
            }
        }

        private DateTime? playFrom;
        [DataMember]
        public virtual DateTime? PlayFrom
        {
            get { return playFrom; }
            set
            {
                playFrom = value;
                FirePropertyChanged("PlayFrom");
            }
        }

        private DateTime? playTo;
        [DataMember]
        public virtual DateTime? PlayTo
        {
            get { return playTo; }
            set
            {
                playTo = value;
                FirePropertyChanged("PlayTo");
            }
        }

        private DateTime? started;
        [DataMember]
        public virtual DateTime? Started
        {
            get { return started; }
            set
            {
                started = value;
                FirePropertyChanged("Started");
            }
        }

        private DateTime? finished;
        [DataMember]
        public virtual DateTime? Finished
        {
            get { return finished; }
            set
            {
                finished = value;
                FirePropertyChanged("Finished");
            }
        }

        private FinishType finishType;
        [DataMember]
        public virtual FinishType FinishType
        {
            get { return finishType; }
            set
            {
                finishType = value;
                FirePropertyChanged("FinishType");
            }
        }

        private int? rating;
        [DataMember]
        public virtual int? Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                FirePropertyChanged("Rating");
            }
        }

        private string comments;
        [DataMember]
        public virtual string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                FirePropertyChanged("Comments");
            }
        }
    }
}
