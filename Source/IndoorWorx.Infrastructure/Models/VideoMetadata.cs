using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class VideoMetadata : BaseModel
    {
        private DateTime? whenFilmed;
        [DataMember]
        public virtual DateTime? WhenFilmed
        {
            get { return whenFilmed; }
            set
            {
                whenFilmed = value;
                FirePropertyChanged("WhenFilmed");
            }
        }

        private string filmedBy;
        [DataMember]
        public virtual string FilmedBy
        {
            get { return filmedBy; }
            set
            {
                filmedBy = value;
                FirePropertyChanged("FilmedBy");
            }
        }

        private string filmedWith;
        [DataMember]
        public virtual string FilmedWith
        {
            get { return filmedWith; }
            set
            {
                filmedWith = value;
                FirePropertyChanged("FilmedWith");
            }
        }
    }
}
