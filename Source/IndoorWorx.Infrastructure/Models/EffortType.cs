using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public class EffortType : BaseModel
    {
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
        //[Description("Power")]
        //Power,
        //[Description("HeartRate")]
        //HeartRate,
        //[Description("RPE")]
        //RPE        

        //public int? GetLowValueFor(IntervalLevel value)
        //{
        //    if (title == "Power")
        //        return value.MinimumPercentageOfFtp;
        //    if (title == "HR")
        //        return value.MinimumPercentageOfFthr;
        //    if (title == "RPE")
        //        return value.MinRPE;
        //    return null;
        //}
    }
}
