using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class TrainingSet
    {
        public TrainingSet()
        {
            VideoParts = new List<VideoPart>();
            VideoText = new List<VideoText>();
        }

        [DataMember]
        public Guid TrainingSetTemplateId { get; set; }

        [DataMember]
        public ICollection<VideoPart> VideoParts { get; set; }

        [DataMember]
        public ICollection<VideoText> VideoText { get; set; }
    }
}
