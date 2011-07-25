using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class EffortType : BaseModel
    {
        public static readonly string HRTag = "HR";

        public static readonly string PowerTag = "POWER";

        public static readonly string RPETag = "RPE";

        public virtual bool IsHR
        {
            get { return Tag == HRTag; }
        }

        public virtual bool IsPower
        {
            get { return Tag == PowerTag; }
        }

        public virtual bool IsRPE
        {
            get { return Tag == RPETag; }
        }

        private string title;
        [DataMember]
        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                FirePropertyChanged("Title");
            }
        }

        private string description;
        [DataMember]
        public virtual string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged("Description");
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
    }
}
