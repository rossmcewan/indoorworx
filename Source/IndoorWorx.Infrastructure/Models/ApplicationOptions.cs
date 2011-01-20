using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference=false)]
    public partial class ApplicationOptions : BaseModel
    {
        private VideoOptions videoOptions = new VideoOptions();
        [DataMember]
        public virtual VideoOptions VideoOptions
        {
            get { return videoOptions; }
            set
            {
                this.videoOptions = value;
                FirePropertyChanged("VideoOptions");
            }
        }
    }
}
