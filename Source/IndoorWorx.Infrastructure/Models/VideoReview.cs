using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class VideoReview : Review
    {
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
    }
}
