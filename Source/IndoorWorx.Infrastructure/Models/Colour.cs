using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract]
    public partial class Colour : BaseModel
    {

        private byte alpha;
        [DataMember]
        public virtual byte Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                FirePropertyChanged("Alpha");
            }
        }

        private byte red;
        [DataMember]
        public virtual byte Red
        {
            get { return red; }
            set
            {
                red = value;
                FirePropertyChanged("Red");
            }
        }

        private byte green;
        [DataMember]
        public virtual byte Green
        {
            get { return green; }
            set
            {
                green = value;
                FirePropertyChanged("Green");
            }
        }

        private byte blue;
        [DataMember]
        public virtual byte Blue
        {
            get { return blue; }
            set
            {
                blue = value;
                FirePropertyChanged("Blue");
            }
        }

        
    }
}
