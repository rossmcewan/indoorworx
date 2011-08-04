using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public class VideoInterval : Interval
    {
        //private TimeSpan startTimestamp;
        //[DataMember]
        //public virtual TimeSpan StartTimestamp 
        //{
        //    get { return startTimestamp; }
        //    set
        //    {
        //        startTimestamp = value;
        //        FirePropertyChanged("StartTimestamp");
        //    }
        //}

        //private TimeSpan endTimestamp;
        //[DataMember]
        //public virtual TimeSpan EndTimestamp
        //{
        //    get { return endTimestamp; }
        //    set
        //    {
        //        endTimestamp = value;
        //        FirePropertyChanged("EndTimestamp");
        //    }
        //}
    }
}
