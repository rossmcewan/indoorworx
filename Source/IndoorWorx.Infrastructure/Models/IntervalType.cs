using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class IntervalType : BaseModel
    {
        public static readonly string LevelsTag = "LEVELS";

        public static readonly string SteppedTag = "STEPPED";

        public static readonly string RecoveryTag = "RECOVERY";

        private string name;
        [DataMember]
        public virtual string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged("Name");
            }
        }

        private string tag;
        [DataMember]
        public virtual string Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                FirePropertyChanged("Tag");
            }
        }

        private int sequence;
        [DataMember]
        public virtual int Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                FirePropertyChanged("Sequence");
            }
        }

        public virtual bool IsRecovery
        {
            get { return Tag == RecoveryTag; }
        }
    }
}
