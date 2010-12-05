using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public partial class Telemetry : BaseModel
    {
        [DataMember]
        public virtual TimeSpan TimePosition { get; set; }

        [DataMember]
        public virtual double Torque { get; set; }

        [DataMember]
        public virtual double Speed { get; set; }

        [DataMember]
        public virtual double Watts { get; set; }

        [DataMember]
        public virtual double Distance { get; set; }

        [DataMember]
        public virtual int Cadence { get; set; }

        [DataMember]
        public virtual int HeartRate { get; set; }

        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual double Altitude { get; set; }

        public virtual double PercentageThreshold { get; set; }
        
        public static Telemetry Parse(string s)
        {
            var elements = s.Split(',');
            return new Telemetry()
            {
                TimePosition = TimeSpan.FromMinutes(Convert.ToDouble(elements[0])),
                Torque = Convert.ToDouble(elements[1]),
                Speed = Convert.ToDouble(elements[2]),
                Watts = Convert.ToDouble(elements[3]),
                Distance = Convert.ToDouble(elements[4]),
                Cadence = Convert.ToInt32(elements[5]),
                HeartRate = Convert.ToInt32(elements[6]),
                Id = Convert.ToInt32(elements[7]),
                Altitude = Convert.ToDouble(elements[8]),
                PercentageThreshold = Convert.ToDouble(elements[3]) / 300//must get this from the user
            };
        }

        public Telemetry Clone()
        {
            var result = this.MemberwiseClone() as Telemetry;
            return result;
        }
    }
}
