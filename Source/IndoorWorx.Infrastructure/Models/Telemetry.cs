using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public class Telemetry : BaseModel
    {
        public virtual TimeSpan TimePosition { get; set; }

        public virtual double Torque { get; set; }

        public virtual double Speed { get; set; }

        public virtual double Watts { get; set; }

        public virtual double Distance { get; set; }

        public virtual int Cadence { get; set; }

        public virtual int HeartRate { get; set; }

        public virtual int Id { get; set; }

        public virtual double Altitude { get; set; }

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
                Altitude = Convert.ToDouble(elements[8])
            };
        }
    }
}
