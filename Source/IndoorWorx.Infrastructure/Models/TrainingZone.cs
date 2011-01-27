using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public class TrainingZone : BaseModel
    {
        private string name = string.Empty;
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

        private Range range = new Range();
        [DataMember]
        public virtual Range Range
        {
            get { return range; }
            set
            {
                range = value;
                FirePropertyChanged("Range");
            }
        }

        private Colour colorRepresentation = new Colour();
        [DataMember]
        public virtual Colour ColorRepresentation
        {
            get { return colorRepresentation; }
            set
            {
                colorRepresentation = value;
                FirePropertyChanged("ColorRepresentation");
            }
        }

        public virtual TrainingZone SetRange(Range range)
        {
            this.Range = range;
            return this;
        }

        public virtual TrainingZone SetColour(byte alpha, byte red, byte green, byte blue)
        {
            this.ColorRepresentation.Alpha = alpha;
            this.ColorRepresentation.Red = red;
            this.ColorRepresentation.Green = green;
            this.ColorRepresentation.Blue = blue;
            return this;
        }
    }
}
